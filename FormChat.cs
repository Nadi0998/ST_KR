﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace ST_diplom
{
    public partial class FormChat : Form
    {
        private readonly GetLoginInfo getLogin;
        private Timer RWTimer, SwitchConnBtnTimer;
        private DataController dataController;
        private CircleConnection connection;

        public FormChat(GetLoginInfo getLogin)
        {
            InitializeComponent();

            this.getLogin = getLogin;
            this.dataController = new DataController();
            try
            {
                this.connection = new CircleConnection(this, this.dataController, getLogin.login, getLogin.backComName, getLogin.forwardComName);
            }
            catch (Exception)
            {
                MessageBox.Show("Невозможно открыть заданные COM порты");
                Application.Exit();
            }

            this.RWTimer = new Timer();
            this.RWTimer.Interval = 250;
            this.RWTimer.Tick += RWTick;
            this.RWTimer.Start();

            this.SwitchConnBtnTimer = new Timer();
            this.SwitchConnBtnTimer.Interval = 1250;
            this.SwitchConnBtnTimer.Tick += SwitchConnBtnTick;
        }

        private void CloseConnectionInTick(bool showConnectionLost)
        {
            DateTime time = new DateTime(DateTime.Now.Ticks);
            while (!connection.CanClose() && (DateTime.Now - time).TotalMilliseconds < 2500);
            connection.Close();
            connection = new CircleConnection(this, this.dataController, getLogin.login, getLogin.backComName, getLogin.forwardComName);
            this.Invoke((Action<bool>)(delegate(bool idle)
            {
                onlineUsersList.Items.Clear();
                if (showConnectionLost)
                {
                    chatField.Text += "# Соединение потеряно\r\n";
                    History.AddLine("# Соединение потеряно");
                }
            }), true);
        }

        private void RWTick(object sender, EventArgs e)
        {
            bool willCloseSelf, willCloseRecv;
            int onlineUsersCount;

            connection.Exec(RWTimer.Interval, out willCloseSelf, out willCloseRecv, out onlineUsersCount);

            if (willCloseSelf) 
            {
                RWTimer.Stop();
                CloseConnectionInTick(connection.HasConnectionToLost());
            }
            else if (willCloseRecv)
            {
                CloseConnectionInTick(connection.HasConnectionToLost());
            }
            else if (onlineUsersCount > 0)
            {
                int newInterval = Math.Max(250 / onlineUsersCount, 10);
                RWTimer.Interval = newInterval;
            }
            else
            {
                RWTimer.Interval = 50;
            }
        }

        private void SwitchConnBtnTick(object sender, EventArgs e)
        {
            this.Invoke((Action<bool>)(delegate(bool enable)
            {
                switchConnectionButton.Enabled = enable;
            }), true);
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            SendMessage();
        }


        //здесь можно отправить приватное сообщение - часть выпилить
        private void SendMessage()
        {
            // если не нужно отправлять, а просто вывести на экран
            // вдруг приватное сообщение отправлено самому себе
            string user = null;
            string msg = "";
            if (msgInputField.Text.IndexOf('@') == 0)
            {
                int pos = msgInputField.Text.IndexOf(' ');
                if (pos == -1)
                {
                    string alertMsg = "Вы пытаетесь отправить приватное сообщение, однако не ввели сообщение\r\n"
                                    + "@NICK MESSAGE";
                    MessageBox.Show(alertMsg, "Чат Волчат");
                    return;
                }
                if (pos < 2)
                {
                    string alertMsg = "Вы пытаетесь отправить приватное сообщение, однако оставили ник пустым\r\n"
                                    + "@NICK MESSAGE";
                    MessageBox.Show(alertMsg, "Чат Волчат");
                    return;
                }

                user = msgInputField.Text.Substring(1, pos - 1);
                msg = msgInputField.Text.Substring(pos + 1);
                if (msg == "")
                {
                    string alertMsg = "Вы пытаетесь отправить приватное сообщение, но не ввели сообщение\r\n"
                                    + "@NICK MESSAGE";
                    MessageBox.Show(alertMsg, "Чат Волчат");
                    return;
                }
            } else {
                msg = msgInputField.Text;
            }
            msgInputField.Text = "";

            DataController.UserMessage newMsg = new DataController.UserMessage(msg, getLogin.login, user);

            this.dataController.WriteQueue.Enqueue(newMsg);
        }

        private void onlineUsersList_DoubleClick(object sender, EventArgs e)
        {
            object item = this.onlineUsersList.SelectedItem;

            if (this.msgInputField.Text.IndexOf('@') == 0) 
            {
                int privPos = this.msgInputField.Text.IndexOf(' ');
                if (privPos != -1)
                {
                    this.msgInputField.Text = this.msgInputField.Text.Remove(0, privPos + 1);
                }
                else
                {
                    this.msgInputField.Text = "";
                }
            }

            if (item != null)
            {
                this.msgInputField.Text = String.Format("@{0} {1}", item.ToString(), this.msgInputField.Text);
            }
        }

        private void SwitchConnectionButton_Click(object sender, EventArgs e)
        {
            if (switchConnectionButton.Text == "Отключиться")
            {
                switchConnectionButton.Text = "Подключиться";
                switchConnectionButton.Enabled = false;

                dataController.SystemQueue.Enqueue(new DataController.SystemMessage(DataController.SystemMessage.MessageType.Close));

                SwitchConnBtnTimer.Start();
            }
            else if (switchConnectionButton.Text == "Подключиться")
            {
                switchConnectionButton.Text = "Отключиться";
                switchConnectionButton.Enabled = false;

                RWTimer.Start();
                SwitchConnBtnTimer.Start();
            }
        }

        private void msgInputField_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Return)
            {
                SendMessage();
                e.SuppressKeyPress = true;
            }
        }

        private void HistoryButton_Click(object sender, EventArgs e)
        {
            History.OpenHistory();
        }

        private void FormChat_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            chatField.Text = "";
        }
    }
}
