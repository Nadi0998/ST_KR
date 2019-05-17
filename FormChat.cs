using System;
using System.Windows.Forms;

namespace ST_Cursach
{
    public partial class FormChat : Form
    {
        private readonly GetLoginInfo getLogin;
        public string username;
        private Timer RWTimer, SwitchConnBtnTimer;
        private DataController dataController;
        private CircleConnection connection;

        private static readonly int HEADER_COUNT = 6;
        private static readonly int MSG_ID_HEADER = 0;
        private static readonly int MSG_FROM_HEADER = 1;
        private static readonly int MSG_TO_HEADER = 2;
        private static readonly int MSG_SHORT_HEADER = 3;
        private static readonly int MSG_FULL_HEADER = 4;
        private static readonly int MSG_READ_BOOL_HEADER = 5;

        public FormChat(GetLoginInfo getLogin)
        {
            InitializeComponent();

            this.getLogin = getLogin;
            username = getLogin.login;
            dataController = new DataController();

            try
            {
                this.connection = new CircleConnection(this, this.dataController, getLogin.login, getLogin.backComName, getLogin.forwardComName);
            }
            catch (Exception)
            {
                MessageBox.Show("Невозможно открыть заданные COM порты");
                Application.Exit();
            }
            //таймер для обработки маркера
            RWTimer = new Timer();
            RWTimer.Interval = 250;
            RWTimer.Tick += RWTick;
            RWTimer.Start();
            //таймер для отображения кнопки закрытия
            SwitchConnBtnTimer = new Timer();
            SwitchConnBtnTimer.Interval = 1250;
            SwitchConnBtnTimer.Tick += SwitchConnBtnTick;
        }

        private void CloseConnectionInTick(bool showConnectionLost)
        {
            DateTime time = new DateTime(DateTime.Now.Ticks);
            while (!connection.CanClose() && (DateTime.Now - time).TotalMilliseconds < 2500); // нечего передавать и время работы
            connection.Close();
            connection = new CircleConnection(this, this.dataController, getLogin.login, getLogin.backComName, getLogin.forwardComName);
            this.Invoke((Action<bool>)(delegate(bool idle)
            {
                onlineUsersList.Items.Clear();
                if (showConnectionLost)
                {
                    LogBox.AppendText("# Соединение потеряно" + Environment.NewLine);
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
            msgListView.Items.Add(FormatEnvelope(msg));
        }

        public void SetMessageRead(int msgId)
        {
            System.IO.File.AppendAllText("log.txt", String.Format("I'm {0}, setting ACK for msgId {1}" + Environment.NewLine,
                        username, msgId));
            foreach (ListViewItem item in msgListView.Items)
            {
                if (item.Text.Equals(msgId.ToString()))
                {
                    System.IO.File.AppendAllText("log.txt", String.Format("I'm {0}, setting ACK for msgId {1} inside loop" + Environment.NewLine,
                        username, msgId));
                    item.SubItems[MSG_READ_BOOL_HEADER].Text = "1";
                }
            }
        }

        //todo
        public void AddMessageSelf(DataController.UserMessage msg) { }

        private static string FormatMessage(DataController.UserMessage m)
        {
            return String.Format("@{0} > {1}: {2}" + Environment.NewLine, 
                m.From, m.To, m.Text);
        }

        private ListViewItem FormatEnvelope(DataController.UserMessage m)
        {
            string[] items = new string[HEADER_COUNT];
            items[MSG_ID_HEADER] = m.ID.ToString();
            items[MSG_FROM_HEADER] = m.From;
            items[MSG_TO_HEADER] = m.To;
            items[MSG_SHORT_HEADER] = m.Text.Length > 5 ? m.Text.Substring(0, 5) + "..." : m.Text;
            items[MSG_FULL_HEADER] = m.Text;
            items[MSG_READ_BOOL_HEADER] = "0";
            return new ListViewItem(items);
        }


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
                    string alertMsg = "Вы пытаетесь отправить сообщение, однако ничего не ввели" + Environment.NewLine
                                    + "@NICK MESSAGE";
                    MessageBox.Show(alertMsg, caption);
                    return;
                }
                if (pos < 2)
                {
                    string alertMsg = "Вы пытаетесь отправить сообщение, однако не выбрали получателя" + Environment.NewLine
                                    + "@NICK MESSAGE";
                    MessageBox.Show(alertMsg, caption);
                    return;
                }

                user = msgInputField.Text.Substring(1, pos - 1);
                msg = msgInputField.Text.Substring(pos + 1);
                if (msg == "")
                {
                    string alertMsg = "Вы пытаетесь отправить сообщение, однако ничего не ввели" + Environment.NewLine
                                    + "@NICK MESSAGE";
                    MessageBox.Show(alertMsg, caption);
                    return;
                }
                if (user.Equals(username))
                {
                    LogBox.AppendText(msgInputField.Text + Environment.NewLine);
                    msgInputField.Text = "";
                    return;
                }
            } else {
                //TODO: FIX
                LogBox.AppendText(msgInputField.Text + Environment.NewLine);
                msgInputField.Text = "";
                return;
            }
            msgInputField.Text = "";

            DataController.UserMessage newMsg = new DataController.UserMessage(msg, getLogin.login, user);

            this.dataController.WriteQueue.Enqueue(newMsg);
            //if(newMsg.To != getLogin.login)
            //{
            //    AddMessage(newMsg);
            //}
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

        private void msgListView_ItemActivate(object sender, EventArgs e)
        {
            foreach (ListViewItem item in msgListView.SelectedItems)
            {
                messageBox.Text = item.SubItems[MSG_FULL_HEADER].Text;
                //messageBox.Text = "";
                //foreach (ListViewItem.ListViewSubItem sub in item.SubItems)
                //{
                //    messageBox.Text += sub.Text + " ";
                //}
                if (item.SubItems[MSG_READ_BOOL_HEADER].Text.Equals("0") && !item.SubItems[MSG_FROM_HEADER].Text.Equals(username))
                {

                    item.SubItems[MSG_READ_BOOL_HEADER].Text = "1";
                    int id = int.Parse(item.SubItems[MSG_ID_HEADER].Text);
                    System.IO.File.AppendAllText("log.txt", String.Format("I'm {0}, msg ID {1}, from {2}, to {3}, Text {4} sending ACK" + Environment.NewLine,
                        username, id, item.SubItems[MSG_FROM_HEADER].Text, item.SubItems[MSG_TO_HEADER].Text, item.SubItems[MSG_FULL_HEADER].Text));
                    dataController.WriteQueue.Enqueue(
                        new DataController.UserMessage(id,
                            item.SubItems[MSG_TO_HEADER].Text,
                            item.SubItems[MSG_FROM_HEADER].Text));
                }


            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void msgListView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            messageBox.Clear();
            foreach (ListViewItem item in msgListView.Items)
            {
                messageBox.Text += item.Text +", ";
                foreach (ListViewItem.ListViewSubItem subItem in item.SubItems)
                    messageBox.Text += subItem.Text + " ";
                messageBox.Text += Environment.NewLine;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            messageBox.Text = "";
        }
    }
}
