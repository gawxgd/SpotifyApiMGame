using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace spotifyAPIgae.Views;

public partial class MainView : UserControl
{
    public SpotifyAuth? auth;
    public SpotifyUser? sUser;
    public MainView()
    {
        InitializeComponent();
    }
    public async void OnSpotifyLoginAsync()
    {
        auth = new SpotifyAuth();
        sUser = await auth.BeginAuthorization();
        GameWindow wind = new GameWindow() { user = sUser };
        wind.Show();

    }
    public void OnSpotifyLogin(object sender,RoutedEventArgs args)
    {
        OnSpotifyLoginAsync();
    }
}
