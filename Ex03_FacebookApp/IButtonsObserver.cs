using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex03_FacebookApp
{
    interface IButtonsObserver
    {
        event Action<FacebookForm> OnProfileButtonClicked;
        event Action<FacebookForm> OnMatchFinderButtonClicked;
        event Action<FacebookForm> OnFriendsAndAlbumsButtonClicked;
        event Action<FacebookForm> OnLoginLogoutButtonClicked;
    }
}
