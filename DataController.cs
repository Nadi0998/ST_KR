using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST_diplom
{
    public class DataController
    {
        // сообщения, принятые по сети
        public ConcurrentQueue<UserMessage> ReadQueue { get; private set; }
        // сообщения для отправки по сети
        public ConcurrentQueue<UserMessage> WriteQueue { get; private set; }
        public ConcurrentQueue<SystemMessage> SystemQueue { get; private set; }

        public DataController()
        {
            ReadQueue = new ConcurrentQueue<UserMessage>();
            WriteQueue = new ConcurrentQueue<UserMessage>();
            SystemQueue = new ConcurrentQueue<SystemMessage>();
        }

        public class UserMessage
        {
            public string Text { set; get; }
            public string From { get; private set; }
            public string To { get; private set; }
            public bool Update { get; set; }
            public DateTime SendTime { get; private set; }

            public UserMessage(string text, string from, string to)
            {
                this.Text = text;
                this.From = from;
                this.To = to;
                Update = false;
                SendTime = DateTime.Now;
            }
        }

        public class SystemMessage
        {
            public enum MessageType
            {
                Close
            }

            public MessageType Type { get; private set; }

            public SystemMessage(MessageType type)
            {
                Type = type;
            }
        }
    }
}
