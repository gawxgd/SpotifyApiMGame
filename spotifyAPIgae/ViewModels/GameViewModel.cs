using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace spotifyAPIgae.ViewModels
{
    public class GameViewModel : ViewModelBase
    {
        private AppBaseViewModel _currentViewModel;
        private AppBaseViewModel[] ViewModels;
        public SpotifyUser sUser {  get; private set; }
        public GameViewModel(SpotifyUser user)
        {
            this.sUser = user;
            Debug.WriteLine($"GameViewModel initialized with user: {sUser.DisplayName}");

            ViewModels = new AppBaseViewModel[]{ new UserViewModel() { user = this.sUser }, new CreateSessionViewModel(), new JoinSessionViewModel() { user = this.sUser } };
            _currentViewModel = ViewModels[0];
            var canNavNext = this.WhenAnyValue(x => x.CurrentViewModel.CanNavigateNext);
            var canNavMenu = this.WhenAnyValue(x => x.CurrentViewModel.CanNavigateToMenu);

            NavigateNextCommand = ReactiveCommand.Create(NavigateNext, canNavNext);
            NavigateToMenuCommand = ReactiveCommand.Create(NavigateToMenu, canNavMenu);
        }
        public AppBaseViewModel CurrentViewModel
        {
            get { return _currentViewModel; }
            private set { this.RaiseAndSetIfChanged(ref _currentViewModel, value); }
        }
        public ICommand NavigateNextCommand { get; }

        private void NavigateNext()
        {
            var index = Array.IndexOf(ViewModels, CurrentViewModel) + 1;
            CurrentViewModel = ViewModels[index];
        }
        public ICommand NavigateToMenuCommand { get; }

        private void NavigateToMenu()
        {
            CurrentViewModel = ViewModels.First();
        }
    }
}
