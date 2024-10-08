﻿using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spotifyAPIgae.ViewModels
{
    public class UserViewModel : AppBaseViewModel
    {
        public override bool CanNavigateNext
        {
            get => true;
            protected set => throw new NotSupportedException();
        }
        public override bool CanNavigateToMenu
        {
            get => true;
            protected set => throw new NotSupportedException();
        }
        private SpotifyUser _user;
        public SpotifyUser user
        {
            get => _user;
            set => this.RaiseAndSetIfChanged(ref _user, value);
        }
        public override bool CanNavigateToGame { get => false; protected set => throw new NotImplementedException(); }
    }
}
