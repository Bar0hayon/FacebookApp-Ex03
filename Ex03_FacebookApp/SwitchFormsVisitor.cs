using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ex03_FacebookApp
{
    public class SwitchFormsVisitor
    {
        public void SwitchForms(FacebookForm i_FormToClose, FacebookForm i_FormToShow)
        {
            if (i_FormToClose != i_FormToShow)
            {
                i_FormToClose.Close();
                new Thread(() => 
                AppManager.ShowForm(i_FormToShow)).Start();
            }
        }
    }
}
