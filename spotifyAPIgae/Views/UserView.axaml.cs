using Avalonia.Controls;
using Avalonia.Interactivity;

namespace spotifyAPIgae.Views 
{ 
    public partial class UserView : UserControl
    {
        public UserView()
        {
            InitializeComponent();
        }
        public void OnJoinSession(object sender, RoutedEventArgs args)
        {
        }
        public void OnCreateSession(object sender, RoutedEventArgs args)
        {
            var window = (Window)this.VisualRoot;
            if (window is GameWindow gameWindow)
            {
                var nextButton = gameWindow.FindControl<Button>("NextButton");
                if (nextButton != null)
                {
                    // Programmatically trigger the click event on the Next button
                    nextButton.Command.Execute(null);
                }
            }
        }


    }
}
