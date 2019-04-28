using System;
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
        Action<DataController.UserMessage> update;
        private static string SendTimeMarker { get
            {
                return string.Format("(send at {0})", DateTime.Now.ToLongTimeString());
            }
        }

        public static string ReceivedTimeMarker
        {
            get
            {
                return string.Format("(received at {0})", DateTime.Now.ToLongTimeString());
            }
        }

        public FormChat(GetLoginInfo getLogin)
        {
            InitializeComponent();

            this.getLogin = getLogin;
            dataController = new DataController();
            update = delegate (DataController.UserMessage m)
            {
                chatField.AppendText(FormatMessage(m));

                chatField.SelectionStart = chatField.Text.Length;
                chatField.ScrollToCaret();
            };

            try
            {
                this.connection = new CircleConnection(this, this.dataController, getLogin.login, getLogin.backComName, getLogin.forwardComName);
            }
            catch (Exception)
            {
                MessageBox.Show("Невозможно открыть заданные COM порты");
                Application.Exit();
            }

            RWTimer = new Timer();
            RWTimer.Interval = 250;
            RWTimer.Tick += RWTick;
            RWTimer.Start();

            SwitchConnBtnTimer = new Timer();
            SwitchConnBtnTimer.Interval = 1250;
            SwitchConnBtnTimer.Tick += SwitchConnBtnTick;
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
                    chatField.AppendText("# Соединение потеряно" + Environment.NewLine);
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

        public void AddMessage(DataController.UserMessage msg)
        {
            if (msg.Update)
            {
                string toReplace = msg.Text.Substring(0, msg.Text.Length - ReceivedTimeMarker.Length); //TODO
                chatField.Text = chatField.Text.Replace(toReplace, msg.Text);
                return;
            }
            chatField.Invoke(update, msg);
        }

        private static string FormatMessage(DataController.UserMessage m)
        {
            return String.Format("@{0} > {1}: {2}" + Environment.NewLine, 
                m.From, m.To, m.Text);
        }


        //здесь можно отправить приватное сообщение - часть выпилить
        private void SendMessage()
        {
            string caption = "ChatForm Error!";
            // если не нужно отправлять, а просто вывести на экран
            // вдруг приватное сообщение отправлено самому себе
            string user = null;
            string msg = "";
            if (msgInputField.Text.IndexOf('@') == 0)
            {
                int pos = msgInputField.Text.IndexOf(' ');
                if (pos == -1)
                {
                    string alertMsg = "Вы пытаетесь отправить приватное сообщение, однако не ввели сообщение" + Environment.NewLine
                                    + "@NICK MESSAGE";
                    MessageBox.Show(alertMsg, caption);
                    return;
                }
                if (pos < 2)
                {
                    string alertMsg = "Вы пытаетесь отправить приватное сообщение, однако оставили ник пустым" + Environment.NewLine
                                    + "@NICK MESSAGE";
                    MessageBox.Show(alertMsg, caption);
                    return;
                }

                user = msgInputField.Text.Substring(1, pos - 1);
                msg = msgInputField.Text.Substring(pos + 1);
                if (msg == "")
                {
                    string alertMsg = "Вы пытаетесь отправить приватное сообщение, но не ввели сообщение" + Environment.NewLine
                                    + "@NICK MESSAGE";
                    MessageBox.Show(alertMsg, caption);
                    return;
                }
            } else {
                MessageBox.Show("Вы не выбрали получателя!", caption);
                return;
            }
            msgInputField.Text = "";

            msg = String.Format("{0}{1}", msg, SendTimeMarker);
            DataController.UserMessage newMsg = new DataController.UserMessage(msg, getLogin.login, user);

            this.dataController.WriteQueue.Enqueue(newMsg);
            AddMessage(newMsg);
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

        private void FormChat_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            chatField.Text = "";
        }
    }
}
