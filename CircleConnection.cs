using System;
using System.Collections.Generic;
using System.IO.Ports;

namespace ST_diplom
{
    class CircleConnection
    {
        enum FrameType
        {
            // Mode: Spam
            // Args: string username, byte[16] marker_weight
            Voting,
            // Mode: Marker
            // Args: int counter, int id_1, string name_1, ..., int id_n, int name_n
            IDGen,
            // Mode: Marker
            // Args: int counter {int id_1, string name_1}
            IDPublish,
            // Mode: Marker
            // Args: int new_message_id, int messages_counter {int message_id, int from_id, int to_id, int recv_count, string text}
            Data,
            // Mode: Marker
            Error,
            // Mode: Instant
            Close
        }

        private const byte START_BYTE = 0xFF;

        // read/write to port
        private SerialPort back, forward;
        private List<byte> msgBuffer;
        private List<List<byte>> framesToSend;
        private bool needStartByte;

        // form
        FormChat formChat;
        string lastFormTitle;

        // data controller
        DataController dataController;

        // current machine state
        private FrameType currentState;
        private string inputUserName, currentUserName;
        private int currentUserID;
        private List<User> usersOnline;
        private int noConnectionTimer;

        // отправленные сообщения, отчет о доставке которых еще не получен
        private Dictionary<int, UserMessage> sendedMessages;

        // VOTING
        private string maxMarkerUserName;
        private byte[] currentMarkerWeight;

        // используется мастером для повторной отправки сообщения
        private List<byte> lastSendedData;
        private int resendCount;

        public CircleConnection(FormChat formChat, DataController dataController, string username, string backComNumber, string forwardComNumber)
        {
            back = new SerialPort("COM" + backComNumber);
            back.Parity = Parity.Even;
            back.Handshake = Handshake.RequestToSend;
            back.BaudRate = 9600;
            back.ReadBufferSize = 4 * 1024;
            forward = new SerialPort("COM" + forwardComNumber);
            forward.Parity = Parity.Even;
            forward.Handshake = Handshake.RequestToSend;
            forward.BaudRate = 9600;
            forward.WriteBufferSize = 4 * 1024;

            msgBuffer = new List<byte>();
            framesToSend = new List<List<byte>>();
            needStartByte = true;

            this.formChat = formChat;
            this.lastFormTitle = "";
            this.dataController = dataController;

            currentState = FrameType.Voting;
            inputUserName = username;
            currentUserName = inputUserName;
            currentUserID = -1;
            noConnectionTimer = 0;

            sendedMessages = new Dictionary<int, UserMessage>();

            maxMarkerUserName = username;

            currentMarkerWeight = new byte[16];

            byte[] weight_1 = new byte[8];
            (new Random()).NextBytes(weight_1);
            Buffer.BlockCopy(weight_1, 0, currentMarkerWeight, 0, weight_1.Length);
            byte[] weight_2 = new byte[8];
            (new Random(DateTime.Now.Second * 1000 + DateTime.Now.Millisecond)).NextBytes(weight_2);
            Buffer.BlockCopy(weight_2, 0, currentMarkerWeight, weight_1.Length, weight_2.Length);

            resendCount = 0;

            back.Open();
            forward.Open();

            SendClose();
        }

        public bool HasConnectionToLost()
        {
            return currentState == FrameType.Data;
        }

        public void Close()
        {
            back.Close();
            forward.Close();
        }

        public bool CanClose()
        {
            return forward.BytesToWrite == 0 && framesToSend.Count <= 0;
        }

        public void Exec(int deltaTime, out bool willCloseSelf, out bool willCloseRecv, out int usersOnlineCount)
        {
            usersOnlineCount = usersOnline != null ? usersOnline.Count : 0;

            // обрабатываем системную очередь сообщений
            DataController.SystemMessage sysMsg;
            willCloseSelf = false;
            willCloseRecv = false;
            while (dataController.SystemQueue.TryDequeue(out sysMsg))
            {
                if (sysMsg.Type == DataController.SystemMessage.MessageType.Close)
                {
                    willCloseSelf = true;
                }
            }
            if (willCloseSelf)
            {
                SendClose();
                return;
            }

            noConnectionTimer += deltaTime;

            List<List<byte>> frmsToSendTmp = framesToSend;
            framesToSend = new List<List<byte>>();
            foreach (var x in frmsToSendTmp)
            {
                SendFrame(x, true);
            }

            List<List<byte>> frames = RecvFrames();

            bool votingHasSended = false;
            foreach (var data in frames)
            {
                if (data == null)
                {
                    SendError();
                    continue;
                }

                byte frameType = Utils.GetFrameType(data);
                switch (frameType)
                {
                    case (byte)FrameType.Voting:
                        if (currentState == FrameType.Voting)
                        {
                            string username;
                            byte[] weight = RecvVoting(data, out username);
                            if (currentState == FrameType.IDGen)
                            {
                                // если state сменился, то нужно отправить кадр IDGen
                                List<User> users = new List<User>();
                                users.Add(new User(this.currentUserID, this.currentUserName));
                                SendIDGen(users);
                            }
                            else
                            {
                                SendVoting(weight, username);
                                votingHasSended = true;
                            }
                        }
                        break;
                    case (byte)FrameType.IDGen:
                        noConnectionTimer = 0;
                        resendCount = 0;
                        List<User> genUsers = RecvIDGen(data);
                        if (genUsers == null)
                        {
                            SendClose();
                            willCloseRecv = true;
                            return;
                        }
                        if (this.currentState == FrameType.IDGen)
                        {
                            SendIDGen(genUsers);
                        }
                        else if (this.currentState == FrameType.IDPublish)
                        {
                            this.usersOnline = genUsers;
                            SendIDPublish();
                        }
                        break;
                    case (byte)FrameType.IDPublish:
                        noConnectionTimer = 0;
                        resendCount = 0;
                        RecvIDPublish(data);
                        if (this.currentState == FrameType.IDPublish)
                        {
                            SendIDPublish();
                        }
                        else if (this.currentState == FrameType.Data)
                        {
                            SendData(0, new List<UserMessage>());
                        }
                        break;
                    case (byte)FrameType.Data:
                        noConnectionTimer = 0;
                        resendCount = 0;
                        int newMessageID;
                        List<UserMessage> messages = RecvData(data, out newMessageID);
                        if (this.currentState == FrameType.Data)
                        {
                            SendData(newMessageID, messages);
                        }
                        break;
                    case (byte)FrameType.Error:
                        if (this.currentUserID == 0)
                        {
                            SendFrame(lastSendedData, false);
                        } else {
                            SendError();
                        }
                        break;
                    case (byte)FrameType.Close:
                        if (this.currentState != FrameType.Voting)
                        {
                            willCloseRecv = true;
                            SendClose();
                            return;
                        }
                        break;
                }
            }

            if (this.currentState == FrameType.Voting)
            {
                if (this.noConnectionTimer > 3000)
                {
                    willCloseRecv = true;
                    return;
                }
                if (!votingHasSended)
                {
                    SendVoting(currentMarkerWeight, currentUserName);
                }
            } 
            else 
            {
                if (this.noConnectionTimer > 16000)
                {
                    willCloseRecv = true;
                    SendClose();
                    return;
                }
                else if (this.currentUserID == 0)
                {
                    if ((this.resendCount == 0 && this.noConnectionTimer > 4000)
                        || (this.resendCount == 1 && this.noConnectionTimer > 8000)
                        || (this.resendCount == 2 && this.noConnectionTimer > 12000))
                    {
                        ++this.resendCount;
                        SendFrame(lastSendedData, false);
                    }
                }
            }

            // GUI update
            // обновляем окно чата, считывая все сообщения из DataController.ReadQueue
            Action<DataController.UserMessage> update = delegate(DataController.UserMessage m)
            {
                string addLine = m.IsPublic ? String.Format("{0}: {1}\r\n", m.From, m.Text) : String.Format("@{0} > {1}: {2}\r\n", m.From, m.To, m.Text);
                
                this.formChat.chatField.Text += addLine;
                History.AddLine(addLine);

                this.formChat.chatField.SelectionStart = this.formChat.chatField.Text.Length;
                this.formChat.chatField.ScrollToCaret();
            };
            DataController.UserMessage msg;
            while (this.dataController.ReadQueue.TryDequeue(out msg))
            {
                this.formChat.chatField.Invoke(update, msg);
            }

            Action<string> changeTitle = delegate(string title)
            {
                formChat.Text = "Чат волчат | " + title;
            };

            if (willCloseSelf)
            {
                lastFormTitle = "Отключено";
                formChat.Invoke(changeTitle, "Отключено | " + this.inputUserName);
            } else switch (this.currentState)
            {
                case FrameType.Voting:
                case FrameType.IDGen:
                case FrameType.IDPublish:
                    if (this.lastFormTitle != "Подключение")
                    {
                        lastFormTitle = "Подключение";
                        formChat.Invoke(changeTitle, "Подключение | " + this.currentUserName);
                    }
                    break;
                case FrameType.Data:
                    if (this.lastFormTitle != "Подключено")
                    {
                        this.lastFormTitle = "Подключено";
                        formChat.Invoke(changeTitle, "Подключено | " + this.currentUserName);
                    }
                    break;
            }
        }

        private void SendVoting(byte[] weight, string username)
        {
            List<byte> data = Utils.CreateFrame();

            Utils.AddFrameType(FrameType.Voting, data);
            Utils.AddString(username, data);
            Utils.AddBytes(weight, data);

            SendFrame(data, false);
        }

        private byte[] RecvVoting(List<byte> data, out string username)
        {
            string recvUserName = Utils.GetString(data);
            byte[] recvMarkerWeight = Utils.GetBytes(currentMarkerWeight.Length, data);

            bool isEqu = this.currentUserName == recvUserName && Array.Equals(recvMarkerWeight, currentMarkerWeight);
            //if (isEqu)
            //{
            //    //for (int i = 0; i < recvMarkerWeight.Length; ++i)
            //    //{
            //        //if (recvMarkerWeight[i] != this.currentMarkerWeight[i])
            //        //{
            //        //    isEqu = false;
            //        //    break;
            //        //}
            //    //}
            //}

            if (isEqu)
            {
                // если в полученном пакете имя совпадает с нашим и вес кадра тоже наш
                // значит наш кадр прошел все кольцо и оказался самым "влиятельным"
                // мы становимся властелином кольца

                currentState = FrameType.IDGen;
                currentUserID = 0;
                username = null;
                return null;
            }
            else
            {
                for (int i = 0; i < recvMarkerWeight.Length; ++i)
                {
                    if (recvMarkerWeight[i] > this.currentMarkerWeight[i])
                    {
                        username = recvUserName;
                        return recvMarkerWeight;
                    }
                }
                username = this.currentUserName;
                return this.currentMarkerWeight;
            }
        }

        private void SendIDGen(List<User> users)
        {
            List<byte> data = Utils.CreateFrame();

            Utils.AddFrameType(FrameType.IDGen, data);
            Utils.AddInt(users.Count, data);

            foreach (var user in users)
            {
                Utils.AddInt(user.ID, data);
                Utils.AddString(user.Name, data);
            }

            SendFrame(data, this.currentUserID == 0);
        }

        /**
         * returns list of users from received frame PLUS current user from this pc 
         **/
        private List<User> RecvIDGen(List<byte> data)
        {
            int counter = Utils.GetInt(data);

            if (this.currentState == FrameType.Voting)
            {
                // я НЕ властелин и мне прислали IDGen
                this.currentState = FrameType.IDGen;
                this.currentUserID = counter;
            }
            else if (this.currentState == FrameType.IDGen && this.currentUserID == 0)
            {
                if (counter < 2)
                {
                    return null;
                }
                // я - властелин кольца и я получил обратно сгенерированные id
                this.currentState = FrameType.IDPublish;
            }

            List<User> users = new List<User>();

            Func<string, string> getNewName = new Func<string,string> (delegate (string name)
            {
                // если это имя в чате уже есть, добавляет к нему [2], [3] и т.д.
                if (users.Find(x => x.Name == name) != null)
                {
                    int newNameIndex = 1;
                    string newName;
                    do
                    {
                        ++newNameIndex;
                        newName = String.Format("{0}*[{1}]", name, newNameIndex);
                    } while (users.Find(x => x.Name == newName) != null);
                    return newName;
                }
                else
                {
                    return name;
                }
            });

            for (int i = 0; i < counter; ++i)
            {
                int userID = Utils.GetInt(data);
                string userName = Utils.GetString(data);

                users.Add(new User(userID, userName));
            }

            // властелин кольца уже клал свои данные, повторно не нужно
            if (this.currentUserID != 0)
            {
                this.currentUserName = getNewName(this.currentUserName);
                users.Add(new User(this.currentUserID, this.currentUserName));
            }
            return users;
        }


        //only main pisya executes this
        private void SendIDPublish()
        {
            List<byte> data = Utils.CreateFrame();

            Utils.AddFrameType(FrameType.IDPublish, data);
            Utils.AddInt(this.usersOnline.Count, data);

            foreach (var user in this.usersOnline)
            {
                Utils.AddInt(user.ID, data);
                Utils.AddString(user.Name, data);
            }

            SendFrame(data, this.currentUserID == 0);
        }

        private void RecvIDPublish(List<byte> data)
        {
            if (this.currentState == FrameType.IDGen)
            {
                this.currentState = FrameType.IDPublish;
            }
            else if (this.currentState == FrameType.IDPublish)
            {
                // lord of the ring
                string logMsg = String.Format("# Соединение установлено ({0})", this.currentUserName);
                History.AddLine(logMsg);  //check if needed
                this.currentState = FrameType.Data;
                this.formChat.Invoke((Action<bool>)(delegate(bool enabled) {
                    this.formChat.msgInputField.Enabled = enabled;
                    this.formChat.sendButton.Enabled = enabled;
                    this.formChat.chatField.Text += logMsg + "\r\n";
                }), true);
            }

            int counter = Utils.GetInt(data);
            this.usersOnline = new List<User>(counter);

            for (int i = 0; i < counter; ++i)
            {
                int userID = Utils.GetInt(data);
                string userName = Utils.GetString(data);

                this.usersOnline.Add(new User(userID, userName));
            }

            Action<List<User>> action = delegate (List<User> users)
            {
                formChat.onlineUsersList.Items.Clear();
                formChat.onlineUsersList.Items.AddRange(users.ToArray());
            };
            formChat.onlineUsersList.Invoke(action, usersOnline);
        }

        private void SendData(int newMessageID, List<UserMessage> markerMessages)
        {
            DataController.UserMessage guiMsg;
            while (this.dataController.WriteQueue.TryDequeue(out guiMsg))
            {
                if (guiMsg.Text == null || guiMsg.Text == "")
                    continue;

                if (guiMsg.To == this.currentUserName)
                {
                    formChat.Invoke((Action<bool>)(delegate(bool idle)
                    {
                        string addLine = String.Format("@{0} > {1}: {2}\r\n", this.currentUserName, this.currentUserName, guiMsg.Text);
                        formChat.chatField.Text += addLine;
                        History.AddLine(addLine);
                    }), true);
                    continue;
                }

                int toUserID;
                if (guiMsg.IsPublic)
                {
                    toUserID = -1;
                } else {
                    User toUserTmp = this.usersOnline.Find(x => x.Name == guiMsg.To);
                    if (toUserTmp == null)
                    {
                        // такого юзера нет, поэтому не будем ничего отправлять
                        continue;
                    }
                    toUserID = toUserTmp.ID;
                }

                this.sendedMessages.Add(newMessageID, new UserMessage(newMessageID, this.currentUserID, toUserID, 0, guiMsg.Text));
                ++newMessageID;
            }

            List<byte> data = Utils.CreateFrame();

            Utils.AddFrameType(FrameType.Data, data);
            Utils.AddInt(newMessageID, data);
            Utils.AddInt(markerMessages.Count + this.sendedMessages.Count, data);

            foreach (var msg in markerMessages)
            {
                Utils.AddInt(msg.ID, data);
                Utils.AddInt(msg.FromID, data);
                Utils.AddInt(msg.ToID, data);
                Utils.AddInt(msg.RecvCount, data);
                Utils.AddString(msg.Text, data);
            }
            // отправляем заново сообщения, которые еще не считаются дошедшими
            foreach (var kvp in this.sendedMessages)
            {
                var msg = kvp.Value;
                Utils.AddInt(msg.ID, data);
                Utils.AddInt(msg.FromID, data);
                Utils.AddInt(msg.ToID, data);
                Utils.AddInt(msg.RecvCount, data);
                Utils.AddString(msg.Text, data);
            }

            SendFrame(data, this.currentUserID == 0);
        }

        private List<UserMessage> RecvData(List<byte> data, out int newMessageID)
        {
            if (this.currentState == FrameType.IDPublish)
            {
                string logMsg = String.Format("# Соединение установлено ({0})", this.currentUserName);
                History.AddLine(logMsg);

                this.currentState = FrameType.Data;
                this.formChat.Invoke((Action<bool>)(delegate(bool enabled)
                {
                    this.formChat.msgInputField.Enabled = enabled;
                    this.formChat.sendButton.Enabled = enabled;
                    this.formChat.chatField.Text += logMsg + "\r\n";
                }), true);
            }

            newMessageID = Utils.GetInt(data);
            int messagesCount = Utils.GetInt(data);

            List<UserMessage> markerMessages = new List<UserMessage>(messagesCount);
            for (int i = 0; i < messagesCount; ++i)
            {
                int mID = Utils.GetInt(data);
                int fromID = Utils.GetInt(data);
                int toID = Utils.GetInt(data);
                int recvCount = Utils.GetInt(data);
                string text = Utils.GetString(data);

                markerMessages.Add(new UserMessage(mID, fromID, toID, recvCount, text));
            }

            List<UserMessage> retMessages = new List<UserMessage>(markerMessages.Count);
            foreach (var msg in markerMessages)
            {
                if (msg.FromID == this.currentUserID)
                {
                    bool success;
                    if (msg.ToID == -1)
                    {
                        // все кроме себя самого должны были получить
                        success = msg.RecvCount == this.usersOnline.Count - 1;
                    } else {
                        success = msg.RecvCount == 1;
                    }

                    if (success)
                    {
                        this.sendedMessages.Remove(msg.ID);
                        DataController.UserMessage guiMsg;

                        User toUser = msg.ToID != -1 ? this.usersOnline.Find(x => x.ID == msg.ToID) : null;
                        guiMsg = new DataController.UserMessage(msg.Text, this.currentUserName, toUser != null ? toUser.Name : null);
                        this.dataController.ReadQueue.Enqueue(guiMsg);
                    } else {
                        retMessages.Add(msg);
                    }
                }
                else if (msg.ToID == this.currentUserID || msg.ToID == -1)
                {
                    DataController.UserMessage guiMsg;
                    
                    User fromUser = this.usersOnline.Find(x => x.ID == msg.FromID);
                    User toUser = msg.ToID != -1 ? this.usersOnline.Find(x => x.ID == msg.ToID) : null;
                    if (fromUser != null)
                    {
                        guiMsg = new DataController.UserMessage(msg.Text, fromUser.Name, toUser != null ? toUser.Name : null);
                        this.dataController.ReadQueue.Enqueue(guiMsg);
                    }

                    msg.RecvCountInc();
                    retMessages.Add(msg);
                }
                else
                {
                    retMessages.Add(msg);
                }
            }

            return retMessages;
        }

        private void SendError()
        {
            List<byte> data = Utils.CreateFrame();

            Utils.AddFrameType(FrameType.Error, data);

            SendFrame(data, false);
        }

        private void SendClose()
        {
            List<byte> data = Utils.CreateFrame();

            Utils.AddFrameType(FrameType.Close, data);

            SendFrame(data, false);
        }

        private List<List<byte>> RecvFrames()
        {
            List<List<byte>> ret = new List<List<byte>>();
            
            if (back.BytesToRead <= 0)
                return ret;

            byte[] arr = new byte[back.BytesToRead];
            back.Read(arr, 0, arr.Length);
            List<byte> list = new List<byte>(arr);

            int currentPos = -1;

            if (!needStartByte)
            {
                currentPos = list.IndexOf(START_BYTE);
                if (currentPos != -1)
                {
                    // нашли END
                    msgBuffer.AddRange(list.GetRange(0, currentPos));
                    ret.Add(msgBuffer);
                    msgBuffer = new List<byte>();
                    needStartByte = true;
                } else {
                    msgBuffer.AddRange(list);
                    return ret;
                }
            }

            while ((currentPos = list.IndexOf(START_BYTE, currentPos + 1)) != -1)
            {
                int startPos = currentPos + 1; // не нужно нам START_BYTE туда
                currentPos = list.IndexOf(START_BYTE, currentPos + 1);
                if (currentPos != -1)
                {
                    // frame: [start] ... [end] ...
                    ret.Add(new List<byte>(list.GetRange(startPos, currentPos - startPos)));
                } else {
                    // frame: [start] ...
                    needStartByte = false;
                    msgBuffer.AddRange(list.GetRange(startPos, list.Count - startPos));
                    break;
                }
            }

            List<List<byte>> safeRet = new List<List<byte>>(ret.Count);
            foreach (var frame in ret)
            {
                List<byte> safeFrame = new List<byte>(frame.Count);
                bool is0x7F = false;
                foreach (var b in frame)
                {
                    if (b == 0x7F)
                    {
                        is0x7F = true;
                    }
                    else
                    {
                        if (is0x7F)
                        {
                            safeFrame.Add((byte)(b | 0x7F));
                            is0x7F = false;
                        }
                        else
                        {
                            safeFrame.Add(b);
                        }
                    }
                }

                safeFrame = Hamming.From(safeFrame);

                if (safeFrame.Count > 0)
                {
                    if (Utils.CheckHash(safeFrame))
                    {
                        safeRet.Add(safeFrame);
                    }
                    else
                    {
                        // только в случае этих кадров мы отправим сообщение об ошибке
                        switch (safeFrame[0])
                        {
                            case (byte)FrameType.IDGen:
                            case (byte)FrameType.IDPublish:
                            case (byte)FrameType.Data:
                                safeRet.Add(null);
                                break;
                        }
                    }
                }
            }

            return safeRet;
        }

        private void SendFrame(List<byte> list, bool needSave)
        {
            if (needSave)
            {
                lastSendedData = list;
            }

            Utils.AddHash(list);

            List<byte> encoded = Hamming.To(list);

            // делаем так, чтобы внутри кадра не встречалось START_BYTE
            List<byte> safeList = new List<byte>(encoded.Count);
            foreach (var b in encoded)
            {
                if ((b & 0x7F) == 0x7F)
                {
                    safeList.Add(0x7F);
                    safeList.Add((byte)(b & 0x80));
                } else {
                    safeList.Add(b);
                }
            }

            safeList.Insert(0, START_BYTE); // start byte
            safeList.Add(START_BYTE); // end byte

            //checking if port has enough size to fit frame
            if (forward.WriteBufferSize - forward.BytesToWrite < safeList.Count)
            {
                framesToSend.Add(list);
                return;
            }

            byte[] arr = safeList.ToArray();
            forward.Write(arr, 0, arr.Length);
        }

        class Utils
        {
            public static List<byte> CreateFrame()
            {
                return new List<byte>();
            }

            public static void AddFrameType(FrameType type, List<byte> list)
            {
                list.Add((byte)type);
            }

            public static byte GetFrameType(List<byte> list)
            {
                byte type = list[0];
                list.RemoveAt(0);
                return type;
            }

            public static void AddBytes(byte[] arr, List<byte> list)
            {
                list.AddRange(arr);
            }

            public static byte[] GetBytes(int count, List<byte> list)
            {
                List<byte> arrList = list.GetRange(0, count);
                list.RemoveRange(0, count);
                return arrList.ToArray();
            }

            public static void AddString(string str, List<byte> list)
            {
                byte[] arr = new byte[str.Length * sizeof(char)];
                System.Buffer.BlockCopy(str.ToCharArray(), 0, arr, 0, arr.Length);
                AddInt(arr.Length, list);
                list.AddRange(arr);
            }

            public static string GetString(List<byte> list)
            {
                int len = GetInt(list);
                List<byte> strList = list.GetRange(0, len);
                list.RemoveRange(0, len);

                char[] chars = new char[len / sizeof(char)];
                System.Buffer.BlockCopy(strList.ToArray(), 0, chars, 0, strList.Count);

                return new string(chars);
            }

            public static void AddInt(int val, List<byte> list)
            {
                for (int i = 0; i < sizeof(int); ++i)
                {
                    list.Add((byte)(val & 0xFF));
                    val >>= 8;
                }
            }

            public static int GetInt(List<byte> list)
            {
                List<byte> valList = list.GetRange(0, sizeof(int));
                list.RemoveRange(0, sizeof(int));
                
                int ret = 0, i = 0;
                foreach (var e in valList)
                {
                    ret |= e << i;
                    i += 8;
                }

                return ret;
            }

            public static void AddHash(List<byte> list)
            {
                int hash = ComputeHash(list, 0, list.Count);
                AddInt(hash, list);
            }

            public static bool CheckHash(List<byte> list)
            {
                if (list.Count < sizeof(int))
                {
                    return false;
                }

                List<byte> hashBytes = list.GetRange(list.Count - sizeof(int), sizeof(int));
                list.RemoveRange(list.Count - sizeof(int), sizeof(int));

                int recvHash = GetInt(hashBytes);
                int realHash = ComputeHash(list, 0, list.Count);
                return recvHash == realHash;
            }

            private static int ComputeHash(List<byte> list, int index, int count)
            {
                int c = 12343;
                int a = 31247;
                int b = 42589;

                int value = c;
                int multiplier = 1;

                List<byte> rangeList;
                if (index == 0 && count == list.Count)
                {
                    rangeList = list;
                }
                else
                {
                    rangeList = list.GetRange(index, count);
                }

                foreach (var item in rangeList)
                {
                    value += item * multiplier;
                    value = value % b;

                    multiplier = (multiplier * a) % b;
                }

                return value % b;
            }
        }

        class User
        {
            public int ID { get; private set; }
            public string Name { get; private set; }

            public User(int id, string name)
            {
                this.ID = id;
                this.Name = name;
            }
        }

        class UserMessage
        {
            public int ID { get; private set; }
            public int ToID { get; private set; }
            public int FromID { get; private set; }
            public int RecvCount { get; private set; }
            public string Text { get; private set; }

            public UserMessage(int id, int fromID, int toID, int recvCount, string text)
            {
                this.ID = id;
                this.FromID = fromID;
                this.ToID = toID;
                this.RecvCount = recvCount;
                this.Text = text;
            }

            public void RecvCountInc()
            {
                ++RecvCount;
            }
        }
    }
}
