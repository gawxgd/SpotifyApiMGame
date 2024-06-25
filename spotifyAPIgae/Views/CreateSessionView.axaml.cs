using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Threading;
using System.Threading.Tasks;

namespace spotifyAPIgae.Views
{
    public partial class CreateSessionView : UserControl
    {
        public CreateSessionView()
        {
            InitializeComponent();
        }
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
        public void OnStartGame(object sender, RoutedEventArgs args)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            var server = TCP.TCPserver.GetInstance(8888, "localhost", SessionTextBox.Text, PasswordTextBox.Text);
            var serverTask = Task.Run(() => { server.Run(cancellationToken); });
            var window = (Window)this.VisualRoot;
            if (window is GameWindow gameWindow)
            {
                var startButton = gameWindow.FindControl<Button>("StartGameButton");
                if (startButton != null)
                {
                    startButton.Command.Execute(server);
                }
            }
        }
    }
}
