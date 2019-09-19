using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FacebookWrapper.ObjectModel;

namespace Ex03_FacebookApp
{
    public partial class MatchFinderForm : FacebookForm
    {
        private List<User> m_MatchesList = new List<User>();
        private IMatchFinderStrategy m_StrategyCondition = new GeneralInformationStrategy();

        public MatchFinderForm()
        {
            InitializeComponent();
        }

        protected override void ButtonFriendsList_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
            base.ButtonFriendsList_Click(sender, e);
        }

        protected override void ButtonProfile_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
            base.ButtonProfile_Click(sender, e);
        }

        private void ButtonFindMatch_Click(object sender, EventArgs e)
        {
            findMatch();
        }

        private void findMatch()
        {
            if (findMatchRequestIsLegal())
            {
                m_MatchesList.Clear();
                try
                {
                    friendListBindingSource.DataSource = LoggedInUser.Friends;
                    foreach (User friend in LoggedInUser.Friends)
                    {
                        if (radioButtonShowResidence.Checked == true)
                        {
                            m_StrategyCondition = new ShowResidenceStrategy() { UserAge = getAgeFromBirthday(friend.Birthday), LoggedInUser = LoggedInUser, CheckedUser = friend };
                        }
                        else 
                        {
                            m_StrategyCondition = new GeneralInformationStrategy() { UserAge = getAgeFromBirthday(friend.Birthday), LoggedInUser = LoggedInUser, CheckedUser = friend };
                        }

                        if (!m_StrategyCondition.FindMatchWithStrategy((int)numericUpDownMinAge.Value, (int)numericUpDownMaxAge.Value, (User.eGender)LoggedInUser.Gender))
                        {
                            friendListBindingSource.Remove(friend);
                        }
                    }

                    if (m_MatchesList.Count == 0)
                    {
                        MessageBox.Show("no matches were found :(", "MatchFinder Resault", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show(
                    "Request is not legal! \n" +
                    "please make sure that all of the filds are filled\n" +
                    "and that the 'minimum age' <= 'maximum age'",
                    "MatchFinder Resault",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private bool findMatchRequestIsLegal()
        {
            return comboBoxMatchGender.SelectedItem != null
                &&
                numericUpDownMinAge.Value <= numericUpDownMaxAge.Value;
        }

        private int getAgeFromBirthday(string i_Birthday)
        {
            const int amountOfDaysInAYear = 365;
            DateTime birthdayDateTime = DateTime.Parse(i_Birthday);
            DateTime currentTime = DateTime.Now;
            int ageInDays = (int)(currentTime - birthdayDateTime).TotalDays;
            int age = ageInDays / amountOfDaysInAYear;
            return age;
        }
    }
}