using Avalonia.Controls;
using spotifyAPIgae.ViewModels;
using System.Diagnostics;
namespace spotifyAPIgae.Views
{
    public partial class GameWindow : Window
    {
        public SpotifyUser? user {  get; set; }
        public GameWindow()
        {
            InitializeComponent();
           
        }
        public GameWindow(SpotifyUser user)
        {
            InitializeComponent();
            DataContext = new GameViewModel(user);
            this.user = user;
        }
        
    }
}
