using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spotifyAPIgae.TCP.Messages
{
    public class MessageReceivedEventArgs : EventArgs
    {
        public Message message {get; }
        public MessageReceivedEventArgs(Message msg )
        {
            message = msg;
        }
    }
}
