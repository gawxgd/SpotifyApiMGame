using Avalonia.Controls;
using spotifyAPIgae.ViewModels;
namespace spotifyAPIgae.Views
{
    public partial class GameWindow : Window
    {
        public SpotifyUser user {  get; set; }
        public GameWindow()
        {
            InitializeComponent();
            DataContext = new GameViewModel(user);
        }
        
    }
}
