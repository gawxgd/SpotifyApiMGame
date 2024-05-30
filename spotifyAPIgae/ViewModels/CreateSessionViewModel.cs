using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spotifyAPIgae.ViewModels
{
    public class CreateSessionViewModel : AppBaseViewModel
    {
        public CreateSessionViewModel()
        {
            // Listen to changes of MailAddress and Password and update CanNavigateNext accordingly
            //this.WhenAnyValue(x => x.MailAddress, x => x.Password)
            //    .Subscribe(_ => UpdateCanNavigateNext());
            _CanNavigateNext = false;
        }

        private bool _CanNavigateNext;
        public override bool CanNavigateNext
        {
            get { return _CanNavigateNext; }
            protected set { this.RaiseAndSetIfChanged(ref _CanNavigateNext, value); }
        }
        public override bool CanNavigatePrevious
        {
            get => true;
            protected set => throw new NotSupportedException();
        }

        private void UpdateCanNavigateNext()
        {
            CanNavigateNext = true; // set acorrding to validation of ip port
        }
    }
}

