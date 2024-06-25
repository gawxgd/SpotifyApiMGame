using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using spotifyAPIgae.TCP.Messages;

namespace spotifyAPIgae.TCP
{
    public class TCPserver : TCPevents
    {
        private static TCPserver _tcpServerInstance = null;
        private List<ConnectionClient> activeClients;
        private TcpListener listener;
        private string password;
        private Task sendingTask;

        public static TCPserver GetInstance(Int32 port, string ip, string sessionName, string sessionPassword)
        {
            if (_tcpServerInstance == null)
            {
                try
                {
                    IPAddress IP;
                    if (ip == "localhost")
                    {
                        string host = Dns.GetHostName();
                        IPHostEntry IPentry = Dns.GetHostEntry(ip);
                        IP = IPentry.AddressList[0];
                    }
                    else
                    {
                        IP = IPAddress.Parse(ip);
                    }
                    return new TCPserver(port, IP, sessionName, sessionPassword);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
            return _tcpServerInstance;
        }
        private TCPserver(Int32 port, IPAddress ip, string sessionName, string sessionPassword)
        {
            activeClients = new List<ConnectionClient>();
            password = sessionPassword;
            sendingTask = Task.Factory.StartNew(() => { });

            try
            {
                listener = new TcpListener(ip, port);
                listener.Start();
                Debug.WriteLine("server started");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
        public async void Run(CancellationToken token)
        {
            List<Task> ClientsTasks = new List<Task>();
            while (!token.IsCancellationRequested)
            {
                try
                {
                    var client = new ConnectionClient(await listener.AcceptTcpClientAsync(token));
                    ClientsTasks.Add(Task.Run((Action)(() => Connection(client))));
                    Debug.WriteLine($"{DateTime.Now.ToString("HH:mm")} | New client... Authorizing {Environment.NewLine}");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
        }
        private async void Connection(ConnectionClient client)
        {
            if (PerformAuthorization(client))
            {
                activeClients.Add(client);
                Debug.WriteLine($"{DateTime.Now.ToString("HH:mm")} Has Connected {client.Name} {Environment.NewLine}");
                while (client.Connected && !client.Cancellation.IsCancellationRequested)
                {
                    try
                    {
                        string RecivedMessage = client.Reader.ReadLine();
                        if (!(RecivedMessage is null))
                        {
                            Messages.Message recived = JsonConvert.DeserializeObject<Messages.Message>(RecivedMessage);
                            if (recived.Text == "Disconnected")
                            {
                                // find better way
                                client.TcpClient.Close();
                                activeClients.Remove(client);
                                continue;
                            }
                            OnMessageReceived(new MessageReceivedEventArgs(recived));
                            Debug.WriteLine($"{recived.Time.ToString("HH:mm")} | Recived {recived.Text} From {recived.Sender} {Environment.NewLine}");
                            foreach (var cl in activeClients)
                            {
                                if (cl.Id != client.Id)
                                {
                                    SendMesage(recived, cl);
                                    Debug.WriteLine($"{recived.Time.ToString("HH:mm")} | Send {recived.Text} To {cl.Name} {Environment.NewLine}");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
        }
        private bool PerformAuthorization(ConnectionClient client)
        {
            try
            {
                string NetMessage = client.Reader.ReadLine();
                if (NetMessage is null)
                    return false;
                Messages.Authorization auth = JsonConvert.DeserializeObject<Messages.Authorization>(NetMessage);
                if (auth is null)
                    return false;
                client.Name = auth.Sender;
                if (auth.Key == password)
                {
                    SendMessageAsync(new Messages.Message("server", "Authorized", DateTime.Now), client);
                    Debug.WriteLine($"{DateTime.Now.ToString("HH:mm")} Authorized {client.Name} {Environment.NewLine}");
                    //form.Invoke(new Action(() => form.dataGridView1.Rows.Add(client.Id, client.Name)));
                    //var disconnectButton = new DataGridViewButtonCell();
                    //disconnectButton.Value = "Disconnect";
                    //form.Invoke(new Action(() => form.dataGridView1.Rows[row].Cells["Disconnecth"] = disconnectButton));
                   // row++;
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
        }
        private void SendMesage(Messages.Message message, ConnectionClient client)
        {
            try
            {
                string messageJson = JsonConvert.SerializeObject(message);
                client.Writer.WriteLine(messageJson);
                client.Writer.Flush();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
        public async Task SendMessageAsync(Messages.Message message, ConnectionClient client)
        {
            await sendingTask.ContinueWith((Action<Task>)(task => SendMesage(message, client)));
            Debug.WriteLine($"{DateTime.Now.ToString("HH:mm")} | Send {message.Text} to {client.Name} {Environment.NewLine}");
        }
        public void SendToAll(Messages.Message mes)
        {
            foreach (ConnectionClient client in activeClients)
            {
                SendMessageAsync(mes, client);
            }
        }
    }
}
