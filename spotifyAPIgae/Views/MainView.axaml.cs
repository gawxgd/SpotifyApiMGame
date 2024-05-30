using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace spotifyAPIgae.Views;

public partial class MainView : UserControl
{
    public SpotifyAuth? auth;
    public SpotifyUser? user;
    public MainView()
    {
        InitializeComponent();
    }
    public async void OnSpotifyLoginAsync()
    {
        auth = new SpotifyAuth();
        user = await auth.BeginAuthorization();
        GameWindow wind = new GameWindow() { DataContext = user };
        wind.Show();

    }
    public void OnSpotifyLogin(object sender,RoutedEventArgs args)
    {
        OnSpotifyLoginAsync();
    }
}
