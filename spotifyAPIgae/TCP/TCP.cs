using Newtonsoft.Json;
using spotifyAPIgae.TCP.Messages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public virtual void SendMesage(Messages.Message message, ConnectionClient client) { }
        public async virtual Task SendMessageAsync(Messages.Message message, ConnectionClient client) { }
        public virtual void SendToAll(Messages.Message mes) { }
    }
}
