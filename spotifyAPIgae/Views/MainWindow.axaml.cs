using Avalonia.Controls;
using Avalonia.Interactivity;
using System;

namespace spotifyAPIgae.Views;


public partial class MainWindow : Window
{
    public MainWindow()
    {
        CustomInitialize();
        InitializeComponent();
        var mainView = this.FindControl<MainView>("mView");
        mainView.SpotifyLoginCompleted += OnSpotifyLoginCompleted;
    }
    private void CustomInitialize()
    {
        Width = 400;
        CanResize =  false;
    }
    private void OnSpotifyLoginCompleted(object? sender, EventArgs e)
    {
        this.Close();
    }
    public void ClickHandler(object sender, RoutedEventArgs args)
    {
    }
}
