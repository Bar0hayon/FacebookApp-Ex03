using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacebookWrapper.ObjectModel;

namespace Ex03_FacebookApp
{
    public interface IMatchFinderStrategy
    {
        bool FindMatchByCondition(int i_FromAge, int i_ToAge, User.eGender i_WandtedGender);

        User LoggedInUser { get; set; }

        User CheckedUser { get; set; }

        int UserAge { get; set; }
    }
}
