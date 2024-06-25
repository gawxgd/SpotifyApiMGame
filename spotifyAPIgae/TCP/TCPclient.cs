using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Windows.Input;
using spotifyAPIgae.TCP.Messages;

namespace spotifyAPIgae.TCP
{
    public class TCPclient : TCPevents
    {
        private static TCPclient _tcpClientInstance;
        private string connectionAddress;
        private Int32 port;
        private string name;
        private string password;
        private ConnectionClient client;
        private CancellationToken token;
        private Task sendingTask;

        public static TCPclient GetInstance(Int32 port, string ip, string userName, string sessionPassword)
        {
            
            if (_tcpClientInstance == null)
                return new TCPclient(port, ip, userName, sessionPassword);
            
            return _tcpClientInstance;
        }
        private TCPclient(Int32 port, string ip, string userName, string sessionPassword)
        {
            this.port = port;
            connectionAddress = ip;
            this.name = userName;
            this.password = sessionPassword;
            this.token = token;
            sendingTask = Task.Factory.StartNew(() => { });
        }
        public bool Run(CancellationToken token)
        {
            this.token = token;
            IPHostEntry entry;
            try
            {
                entry = Dns.GetHostEntry(connectionAddress);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to resolve address");
                return false;
            }
            
            IPAddress ip = entry.AddressList[0];
            TcpClient tcpClient = new TcpClient();
            ConnectionClient client;
            try
            {
                //connectbox.ChangeProgressbar(25); zrobic progressbar
                Task ConnectionTask = tcpClient.ConnectAsync(ip, port);
                client = new ConnectionClient(tcpClient);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("server not started");
                return false;
            }

            try
            {
                return AuthorizeClient(client);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }

        }
        private bool AuthorizeClient(ConnectionClient client)
        {
            Messages.Authorization Auth = new Messages.Authorization(name, password);
            string authmes = JsonSerializer.Serialize(Auth);
            ((TextWriter)client.Writer).WriteLine(authmes);
            ((TextWriter)client.Writer).Flush();
            string recived = ((TextReader)client.Reader).ReadLine();
            if (recived is null)
            {
                Debug.WriteLine("authorization failed");
                return false;
            }
            //connectbox.CloseBox(true);
            this.client = client;
            //userName = connectbox.textBox3.Text;
            MessageOps(client);
            return true;
        }
        private void MessageOps(ConnectionClient client)
        {
            try
            {
                while (client.Connected)
                {
                    if (token.IsCancellationRequested)
                    {
                        client.TcpClient.Close();
                        //form.Invoke(new Action(() => { form.disconnectToolStripMenuItem.Enabled = false; form.connectToolStripMenuItem.Enabled = true; }));
                    }
                    string RecivedMessage = ((TextReader)client.Reader).ReadLine();

                    if (!(RecivedMessage is null))
                    {
                        Messages.Message recivedOG = JsonSerializer.Deserialize<Messages.Message>(RecivedMessage);
                        if (recivedOG.Text == "disconnected")
                        {
                            //form.Invoke(new Action(() => form.connectLabel.Text = "Disconnected"));
                            client.TcpClient.Close();
                            //form.Invoke(new Action(() => { form.disconnectToolStripMenuItem.Enabled = false; form.connectToolStripMenuItem.Enabled = true; }));
                            break;
                        }
                        //form.Invoke((Action)(() => form.createBubble(recivedOG.Sender, recivedOG.Text, recivedOG.Time)));
                        if (recivedOG.Text == "PIWO")
                        {
                           // form.Invoke(new Action(() => form.createPiwo()));
                        }
                        OnMessageReceived(new MessageReceivedEventArgs(recivedOG));
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public override void SendMesage(Messages.Message message, ConnectionClient client)
        {
            try
            {
                string messageJson = Newtonsoft.Json.JsonConvert.SerializeObject(message);
                client.Writer.WriteLine(messageJson);
                client.Writer.Flush();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
        public override async Task SendMessageAsync(Messages.Message message, ConnectionClient client)
        {
            await sendingTask.ContinueWith((Action<Task>)(task => SendMesage(message, client)));
            Debug.WriteLine($"{DateTime.Now.ToString("HH:mm")} | Send {message.Text} to {client.Name} {Environment.NewLine}");
        }
        public override void SendToAll(Messages.Message mes)
        {
            SendMessageAsync(mes, client);
        }

    }
}
