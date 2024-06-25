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
namespace spotifyAPIgae.ViewModels
{
    public class ChatViewModel : AppBaseViewModel
    {
        private string _newMessage;
        private string _currentUser;
        // add tcp and sending messages
        public ChatViewModel()
        {
            Messages = new ObservableCollection<MessageModel>();
            SendMessageCommand = ReactiveCommand.Create(SendMessage);
            _currentUser = "You"; // You can set this to the current user's name dynamically
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
                Messages.Add(new MessageModel
                {
                    Message = NewMessage,
                    Sender = _currentUser,
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
        public void SubscribeReceivingMessages(TCPevents tcpEvent)
        {
            tcpEvent.MessageReceived += HandleMessageReceived;
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
