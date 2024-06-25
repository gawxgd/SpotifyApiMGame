using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Threading.Tasks;
using System.Threading;
using spotifyAPIgae.ViewModels;
using System.Diagnostics;
using spotifyAPIgae.TCP;

namespace spotifyAPIgae.Views
{
    public partial class JoinSessionView : UserControl
    {
        public JoinSessionView()
        {
            InitializeComponent();
        }
        private JoinSessionViewModel ViewModel => DataContext as JoinSessionViewModel;

        public void OnBackToMenu(object sender, RoutedEventArgs args)
        {
            var window = (Window)this.VisualRoot;
            if (window is GameWindow gameWindow)
            {
                var backButton = gameWindow.FindControl<Button>("BackButton");
                if (backButton != null)
                {
                    backButton.Command.Execute(null);
                }
            }
        }
        public void OnJoinGame(object sender, RoutedEventArgs args)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            TCPclient client;
            if (ViewModel?.user != null)
            {
                client = TCP.TCPclient.GetInstance(8888, "localhost", ViewModel.user.DisplayName, PasswordTextBox.Text);
                var clientTask = Task.Run(() => { client.Run(cancellationToken); });
            }
            else
            {
                client = TCP.TCPclient.GetInstance(8888, "localhost", "unable", PasswordTextBox.Text);
                var clientTask = Task.Run(() => { client.Run(cancellationToken); });
            }
            var window = (Window)this.VisualRoot;
            
            if (window is GameWindow gameWindow)
            {
                var startButton = gameWindow.FindControl<Button>("StartGameButton");
                if (startButton != null)
                {
                    startButton.Command.Execute(client);
                }
            }
        }
    }
}
