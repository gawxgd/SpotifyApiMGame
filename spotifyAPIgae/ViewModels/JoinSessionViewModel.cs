using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spotifyAPIgae.ViewModels
{
    internal class JoinSessionViewModel : AppBaseViewModel
    {
        public override bool CanNavigateNext { get => false; protected set => throw new NotImplementedException(); }
        public override bool CanNavigateToMenu { get => true; protected set => throw new NotImplementedException(); }
        private SpotifyUser _user;
        public SpotifyUser user
        {
            get => _user;
            set => this.RaiseAndSetIfChanged(ref _user, value);
        }
    }
}
