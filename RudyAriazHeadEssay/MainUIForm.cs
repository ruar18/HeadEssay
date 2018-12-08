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
        // Labels displaying friends
        private List<Label> friendLabelList;

        enum RecommendationType { FriendsOfFriendsSameInterest, SameCity, SameCitySameInterest };

        // The current type of recommendations shown, initialized to show
        // friends of friends by default
        private RecommendationType recommendationState = RecommendationType.FriendsOfFriendsSameInterest;

        // The index of the currently shown recommendation
        private int recommendationIndex = 0;

        // The index of the currently shown interest 
        private int interestIndex = 0;

        // The starting index of the currently shown friends
        private int friendsIndex = 0;


        public MainUIForm(Network network, Person user)
        {
            InitializeComponent();
            // Make this form fullscreen
            WindowState = FormWindowState.Maximized;
            this.network = network;
            this.user = user;
            // Initialize the label list displaying friends 
            CreateFriendLabelList();
            PopulateAllLists();
        }

        // Logs user out
        public void LogOut()
        {

        }

        // Populate the user's information label lists
        private void PopulateAllLists()
        {
            // Populate the interest
            PopulateInterest();
            // Populate the user's friends
            PopulateFriendsList();
            // Populate the recommendation
            PopulateRecommendation();
        }
        
        // Create a list to store the friend labels
        private void CreateFriendLabelList()
        {
            friendLabelList = new List<Label> { lblFriend1, lblFriend2, lblFriend3, lblFriend4, lblFriend5 };
        }

        // Populate the user's friends list
        private void PopulateFriendsList()
        {
            // Get the user's friends
            List<Person> friendsList = user.GetAllFriends();

            // If there are no earlier friends, disable the up button
            if (friendsIndex == 0)
            {
                btnFriendUp.Enabled = false;
            }
            else
                btnFriendUp.Enabled = true;
            // If there are no later friends, disable the down button
            if (friendsIndex + 5 >= friendsList.Count)
            {
                btnFriendDown.Enabled = false;
            }
            else
                btnFriendDown.Enabled = true;

            // Determine the upper bound of the indices of friends to display
            int upperBound = Math.Min(user.GetAllFriends().Count, friendsIndex + 5);
            // Loop through the friends and display their usernames
            for(int i = friendsIndex; i < upperBound; i++)
            {
                friendLabelList[i - friendsIndex].Text = user.GetAllFriends()[i].Username;
            }
            // If not all labels have been filled, add placeholders
            for(int i = upperBound; i < friendsIndex + 5; i++)
            {
                friendLabelList[i - friendsIndex].Text = "No friend";
            }
        }

        // Shows the current selected interest
        // TODO: try a class to temporarily store the information, a mediator
        // between the user and the interface 
        private void PopulateInterest()
        {
            // Disable the up button if there are no earlier friends to show
            if (interestIndex == 0)
            {
                btnInterestUp.Enabled = false;
            }
            else
                btnInterestUp.Enabled = true;

            // Disable the down button if there are no later friends to show
            if (interestIndex == user.GetAllInterests().Count - 1)
            {
                btnInterestDown.Enabled = false;
            }
            else
                btnInterestDown.Enabled = true;

            // If there are no interests in the list, display a placeholder
            if(user.GetAllInterests().Count == 0)
            {
                lblInterest.Text = "No interests";
            }
            else
            {
                lblInterest.Text = user.GetAllInterests()[interestIndex];
            }
        }


        private void PopulateInvitation()
        {

        }

        // Populate the currently shown recommendation 
        private void PopulateRecommendation()
        {
            // Check which recommendation type should be shown
            if(recommendationState == RecommendationType.FriendsOfFriendsSameInterest)
            {
                // Find the recommendations
                network.FindFriendsOfFriendsWithSameInterest(user);
                // Get the recommendations
                List<Person> friendsOfFriends = new List<Person>();

                // If there's a friend of friend to recommend:
                if (friendsOfFriends.Any())
                {
                    // Display the username of current friend of friend
                    lblRecommendation.Text = user.GetFriendsOfFriendsSameInterest()[recommendationIndex].Username;
                }
            }

        }
        

        // Accept an invitation
        public void AcceptInvitation(Invitation invitation)
        {
            user.AcceptInvitation(invitation);
        }

        // Go to the previous interest in the list 
        private void btnInterestUp_Click(object sender, EventArgs e)
        {
            // Decrement the index
            interestIndex--;
            // Repopulate the interest
            PopulateInterest();
        }

        // Go to the next interest in the list 
        private void btnInterestDown_Click(object sender, EventArgs e)
        {
            // Increment the index
            interestIndex++;
            // Repopulate the interest 
            PopulateInterest();
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
            // Decrement the index
            friendsIndex--;
            // Repopulate the friends
            PopulateFriendsList();
        }

        private void btnFriendDown_Click(object sender, EventArgs e)
        {
            // Increment the index
            friendsIndex++;
            // Repopulate the friends
            PopulateFriendsList();
        }

        // Adds an interest to the user's interest list
        private void btnAddInterest_Click(object sender, EventArgs e)
        {
            user.AddInterest(txtAddInterest.Text);
            PopulateInterest();
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

        private void MainUIForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
