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
        // The currently shown recommendation
        Person currentRecommendation;

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
            // Invitation creation UI should be hidden at first
            SetInvitationUIVisibility(visible: false);
        }
        

        // Create a list to store the friend labels
        private void CreateFriendLabelList()
        {
            friendLabelList = new List<Label> { lblFriend1, lblFriend2, lblFriend3, lblFriend4, lblFriend5 };
        }

        // Hide or show the invitation creation UI
        private void SetInvitationUIVisibility(bool visible)
        {
            // Set visibility for labels
            lblInvitationCreation.Visible = visible;
            lblPromptLifetime.Visible = visible;
            lblPromptRecipients.Visible = visible;
            lblPromptInterest.Visible = visible;

            // Set visibility for textboxes
            txtInvitationLifetime.Visible = visible;
            txtInvitationRecipients.Visible = visible;
            txtInvitationInterest.Visible = visible;

            // Set visibility for buttons
            btnSendInvitation.Visible = visible;
            btnCancelInvitation.Visible = visible;
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
        // TODO: optimize with dictionaries 
        private void PopulateRecommendation()
        {
            // The list of recommendations
            List<Person> recommendations;

            if (recommendationState == RecommendationType.FriendsOfFriendsSameInterest)
            {
                // Find the recommendations
                network.FindFriendsOfFriendsWithSameInterest(user);
                // Get the recommendations
                recommendations = user.GetFriendsOfFriendsSameInterest();
            }
            else if (recommendationState == RecommendationType.SameCity)
            {
                // Find the recommendations
                network.FindSameCity(user);
                // Get the recommendations
                recommendations = user.GetSameCity();
            }
            else
            {
                // Find the recommendations
                network.FindSameCitySameInterest(user);
                // Get the recommendations
                recommendations = user.GetSameCitySameInterest();
            }

            // Disable the up button if there are no earlier recommendations to show
            if (recommendationIndex == 0)
                btnRecommendationUp.Enabled = false;
            else
                btnRecommendationUp.Enabled = true;

            // Disable the down button if there are no later friends to show
            if (recommendationIndex >= recommendations.Count - 1)
                btnRecommendationDown.Enabled = false;
            else
                btnRecommendationDown.Enabled = true;

            // If there's a recommended user:
            if (recommendations.Any())
            {
                // Get the current recommended user
                currentRecommendation = recommendations[recommendationIndex];
                // Display the username of current recommendation
                lblRecommendation.Text = currentRecommendation.Username;
            }
            // If there isn't a recommended user, display the appropriate message
            else
            {
                lblRecommendation.Text = "No recommendation";
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
            // Decrement the index
            recommendationIndex--;
            // Repopulate the recommendation
            PopulateRecommendation();
        }

        private void btnRecommendationDown_Click(object sender, EventArgs e)
        {
            // Increment the index
            recommendationIndex++;
            // Repopulate the recommendation
            PopulateRecommendation();
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
            // Add the interest
            user.AddInterest(txtAddInterest.Text);
            // Display the interests again
            PopulateInterest();
            // Repopulate recommendations to adjust to the added interest
            PopulateRecommendation();
        }

        // Go to the next type of recommendation
        private void btnNextRecommendationList_Click(object sender, EventArgs e)
        {
            if (recommendationState == RecommendationType.FriendsOfFriendsSameInterest)
            {
                recommendationState = RecommendationType.SameCity;
                lblRecommendationList.Text = "Same City";
            }
            else if (recommendationState == RecommendationType.SameCity)
            {
                recommendationState = RecommendationType.SameCitySameInterest;
                lblRecommendationList.Text = "Same City, Same Interest";
            }
            else
            {
                recommendationState = RecommendationType.FriendsOfFriendsSameInterest;
                lblRecommendationList.Text = "Friends of Friends with Same Interest";
            }

            // Repopulate the recommendation to accomodate for the change in type
            PopulateRecommendation();
        }

        // Add a recommended friend to the user's friends list
        private void btnAddRecommendedFriend_Click(object sender, EventArgs e)
        {
            // Add the recommendation as a friend
            user.AddFriend(currentRecommendation);
            // Repopulate recommendations to account for the friendship 
            PopulateRecommendation();
            // Repopulate friends to display the change
            PopulateFriendsList();
        }

        private void btnToggleInvitations_Click(object sender, EventArgs e)
        {

        }

        private void btnNewInvitation_Click(object sender, EventArgs e)
        {
            // Show the invitation creation UI
            SetInvitationUIVisibility(visible: true);

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

        // Cancel the invitation and close the creation UI
        private void btnCancelInvitation_Click(object sender, EventArgs e)
        {
            // Clear the text of all fields
            txtInvitationLifetime.Text = "";
            txtInvitationRecipients.Text = "";
            txtInvitationInterest.Text = "";

            // Hide the invitation creation UI
            SetInvitationUIVisibility(visible: false);
        }

        // Logs user out
        private void btnLogOut_Click(object sender, EventArgs e)
        {
            // Create a new login form, passing in the current network
            LoginForm frmLogin = new LoginForm(network);
            // Show the new form
            frmLogin.ShowDialog();
            // Close the current UI form
            this.Close();
        }
    }
}
