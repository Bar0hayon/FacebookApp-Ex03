using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacebookWrapper.ObjectModel;

namespace Ex03_FacebookApp
{
    class GeneralInformationStrategy : IMatchFinderStrategy
    {
        public User LoggedInUser { get; set; }

        public User CheckedUser { get; set; }

        public int UserAge { get; set; }
        public bool FindMatchByCondition(int i_FromAge, int i_ToAge, User.eGender i_WantedGender)
        {
            bool result = false;

            if ((UserAge >= i_FromAge && UserAge <= i_ToAge) 
                && CheckedUser.Gender == i_WantedGender 
                && (CheckedUser.RelationshipStatus == User.eRelationshipStatus.Single 
                || CheckedUser.RelationshipStatus == null))
            {
                result = true;
            }

            return result;
        }
    }
}
