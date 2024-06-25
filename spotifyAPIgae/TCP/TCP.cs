using spotifyAPIgae.TCP.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spotifyAPIgae.TCP
{
    public class TCPevents
    {
        public event EventHandler<MessageReceivedEventArgs>? MessageReceived;
        protected void OnMessageReceived(MessageReceivedEventArgs e)
        {
            MessageReceived?.Invoke(this, e);
        }
    }
}
