using ReactiveUI;
using spotifyAPIgae.TCP.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Threading;
using spotifyAPIgae.TCP;
using System.Diagnostics;
namespace spotifyAPIgae.ViewModels
{
    public class ChatViewModel : AppBaseViewModel
    {
        private string _newMessage;
        private string _currentUser;
        private string userName;
        public TCPevents tcp { get; set; }
        // add tcp and sending messages
        public ChatViewModel(string userName)
        {
            Messages = new ObservableCollection<MessageModel>();
            SendMessageCommand = ReactiveCommand.Create(SendMessage);
            this.userName = userName;
        }
        public ObservableCollection<MessageModel> Messages { get; }
        public string NewMessage
        {
            get => _newMessage;
            set => this.RaiseAndSetIfChanged(ref _newMessage, value);
        }
        public ICommand SendMessageCommand { get; }
        private void SendMessage()
        {
            if (!string.IsNullOrWhiteSpace(NewMessage))
            {
                tcp.SendToAll(new Message(userName, NewMessage, DateTime.Now));
                Messages.Add(new MessageModel
                {
                    Message = NewMessage,
                    Sender = "You",
                    Alignment = Avalonia.Layout.HorizontalAlignment.Right // Assuming current user messages are aligned right
                });
                NewMessage = string.Empty;
            }
        }
      
        private void HandleMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            Dispatcher.UIThread.Invoke(() => 
            {
                Messages.Add(new MessageModel
                {
                    Message = e.message.Text,
                    Sender = e.message.Sender,
                    Alignment = Avalonia.Layout.HorizontalAlignment.Left // Messages from others are aligned left
                });
            });
        }
        public void SubscribeReceivingMessages()
        {
            tcp.MessageReceived += HandleMessageReceived;
        }
        public override bool CanNavigateNext { get => false; protected set => throw new NotImplementedException(); }
        public override bool CanNavigateToMenu { get => true; protected set => throw new NotImplementedException(); }
        public override bool CanNavigateToGame { get => false; protected set => throw new NotImplementedException(); }
    }
    public class MessageModel
    {
        public string Message { get; set; }
        public string Sender { get; set; }
        public Avalonia.Layout.HorizontalAlignment Alignment { get; set; }
    }
}
