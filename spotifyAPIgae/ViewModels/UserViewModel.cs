using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace spotifyAPIgae.ViewModels
{
    public class UserViewModel : AppBaseViewModel
    {
        public SpotifyUser user { get; set; }
        public UserViewModel() 
        {
            
        }

        public override bool CanNavigateNext
        {
            get => true;
            protected set => throw new NotSupportedException();
        }
        public override bool CanNavigatePrevious
        {
            get => true;
            protected set => throw new NotSupportedException();
        }
    }
}
