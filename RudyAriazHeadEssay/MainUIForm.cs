using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RudyAriazHeadEssay
{
    public partial class MainUIForm : Form
    {
        private Network network;
        // Logged in user
        private Person user;
        // Label lists 

        public MainUIForm(Network network, Person user)
        {
            InitializeComponent();
            this.network = network;
            this.user = user;
        }

        // Logs user out
        public void LogOut()
        {

        }

        // Populate the user's information label lists
        private void PopulateAllLists()
        {
            // Populate all the lists 
        }

        private void PopulateList<T>(List<T> info, List<Label> labels, int startIndex)
        {

        }

        private void PopulateInvitation(Invitation invitation)
        {

        }


        // Accept an invitation
        public void AcceptInvitation(Invitation invitation)
        {
            user.AcceptInvitation(invitation);
        }

        private void btnInterestUp_Click(object sender, EventArgs e)
        {

        }

        private void btnInterestDown_Click(object sender, EventArgs e)
        {

        }

        private void btnRecommendationUp_Click(object sender, EventArgs e)
        {

        }

        private void btnRecommendationDown_Click(object sender, EventArgs e)
        {

        }

        private void btnInvitationUp_Click(object sender, EventArgs e)
        {

        }

        private void btnInvitationDown_Click(object sender, EventArgs e)
        {

        }

        private void btnFriendUp_Click(object sender, EventArgs e)
        {

        }

        private void btnFriendDown_Click(object sender, EventArgs e)
        {

        }

        private void btnAddInterest_Click(object sender, EventArgs e)
        {

        }

        private void btnNextRecommendationList_Click(object sender, EventArgs e)
        {

        }

        private void btnAddRecommendedFriend_Click(object sender, EventArgs e)
        {

        }

        private void btnToggleInvitations_Click(object sender, EventArgs e)
        {

        }

        private void btnNewInvitation_Click(object sender, EventArgs e)
        {

        }

        private void btnDeleteFriend_Click(object sender, EventArgs e)
        {

        }

        private void btnAcceptInvitation_Click(object sender, EventArgs e)
        {

        }

        private void btnRejectInvitation_Click(object sender, EventArgs e)
        {

        }
    }
}
