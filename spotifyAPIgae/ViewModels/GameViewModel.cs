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
    public class GameViewModel: ViewModelBase
    {
        private AppBaseViewModel _currentViewModel;
        private readonly AppBaseViewModel[] ViewModels = {new UserViewModel(), new CreateSessionViewModel()};
        public GameViewModel()
        {
            _currentViewModel = ViewModels[0];
            var canNavNext = this.WhenAnyValue(x => x.CurrentViewModel.CanNavigateNext);
            var canNavPrev = this.WhenAnyValue(x => x.CurrentViewModel.CanNavigatePrevious);

            NavigateNextCommand = ReactiveCommand.Create(NavigateNext, canNavNext);
            NavigatePreviousCommand = ReactiveCommand.Create(NavigatePrevious, canNavPrev);
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
        public ICommand NavigatePreviousCommand { get; }

        private void NavigatePrevious()
        {
            var index = Array.IndexOf(ViewModels, CurrentViewModel) - 1;
            CurrentViewModel = ViewModels[index];
        }
    }
}
