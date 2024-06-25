using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using spotifyAPIgae.ViewModels;
using System.Threading;
using System.Threading.Tasks;

namespace spotifyAPIgae.Views
{
    public partial class ChatView : UserControl
    {
        public ChatView()
        {
            InitializeComponent();
        }
        private void chatTextBox_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var viewModel = DataContext as ChatViewModel;
                if (viewModel != null && viewModel.SendMessageCommand.CanExecute(null))
                {
                    viewModel.SendMessageCommand.Execute(null);
                }
            }
        }
    }
}
