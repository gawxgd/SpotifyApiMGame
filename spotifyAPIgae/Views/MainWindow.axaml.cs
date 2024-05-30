using Avalonia.Controls;
using Avalonia.Interactivity;

namespace spotifyAPIgae.Views;


public partial class MainWindow : Window
{
    public MainWindow()
    {
        CustomInitialize();
        InitializeComponent();
    }
    private void CustomInitialize()
    {
        Width = 400;
        CanResize =  false;
    }
    public void ClickHandler(object sender, RoutedEventArgs args)
    {
    }
}
