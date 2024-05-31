using Avalonia.Controls;
using Avalonia.Interactivity;

namespace spotifyAPIgae.Views
{
    public partial class JoinSessionView : UserControl
    {
        public JoinSessionView()
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
    }
}
