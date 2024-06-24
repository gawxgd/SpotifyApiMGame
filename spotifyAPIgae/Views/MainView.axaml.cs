using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Diagnostics;
using System.Net.Http.Headers;
using Avalonia.VisualTree;
using System;
namespace spotifyAPIgae.Views;

public partial class MainView : UserControl
{
    public SpotifyAuth? auth;
    public SpotifyUser? sUser;

    public event EventHandler? SpotifyLoginCompleted;
    public MainView()
    {
        InitializeComponent();
    }
    public async void OnSpotifyLoginAsync()
    {
        auth = new SpotifyAuth();
        sUser = await auth.BeginAuthorization();
        GameWindow wind = new GameWindow((SpotifyUser)sUser.Clone());
        wind.Show();
        SpotifyLoginCompleted?.Invoke(this, EventArgs.Empty);
    }
    public void OnSpotifyLogin(object sender,RoutedEventArgs args)
    {
        OnSpotifyLoginAsync();
    }
}
