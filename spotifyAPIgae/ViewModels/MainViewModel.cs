using Avalonia.Media.Imaging;
using System.Threading.Tasks;
using System;

namespace spotifyAPIgae.ViewModels;

public class MainViewModel : ViewModelBase
{
    public Task<Bitmap?> ImageFromWebsite { get; }
}
