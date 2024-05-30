using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spotifyAPIgae.ViewModels
{
    public abstract class AppBaseViewModel : ViewModelBase
    {
        public abstract bool CanNavigateNext { get; protected set; }
        public abstract bool CanNavigatePrevious { get; protected set; }
    }
}
