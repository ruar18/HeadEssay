﻿using System;
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
        private Person currentRecommendation;

        // The index of the currently shown interest 
        private int interestIndex = 0;

        // The starting index of the currently shown friends
        private int friendsIndex = 0;

        // The index of the currently shown invitation
        private int invitationIndex = 0;
        // The currently selected recipients for an invitation
        private List<Person> invitedRecipients = new List<Person>();


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
            if (friendsIndex >= friendsList.Count - 1)
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
                friendLabelList[i - friendsIndex].Text = friendsList[i].Username;
            }
            // If not all labels have been filled, add placeholders
            for(int i = upperBound; i < friendsIndex + 5; i++)
            {
                friendLabelList[i - friendsIndex].Text = "No friend";
            }

            // If there are no friends displayed or the top friend has already been invited,
            // disable the invite button
            if (friendsIndex >= friendsList.Count || invitedRecipients.Contains(friendsList[friendsIndex]))
            {
                btnInviteFriend.Enabled = false;
            }
            // Otherwise, enable the button
            else
            {
                btnInviteFriend.Enabled = true;
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

        private void btnDeleteFriend_Click(object sender, EventArgs e)
        {

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
                // Disable the invite button if the recommended user has already been invited
                btnInviteRecommendation.Enabled = !invitedRecipients.Contains(currentRecommendation);
            }
            // If there isn't a recommended user, display the appropriate message
            else
            {
                lblRecommendation.Text = "No recommendation";
                // Disable the invite button since there is no recommended friend to invite
                btnInviteRecommendation.Enabled = false;
            }            
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
            btnInviteRecommendation.Visible = visible;
            btnInviteFriend.Visible = visible;
        }

        private void PopulateInvitation()
        {
        }
        
        // Accept an invitation
        public void AcceptInvitation(Invitation invitation)
        {
            user.AcceptInvitation(invitation);
        }

        // TODO: implement
        private void btnInvitationUp_Click(object sender, EventArgs e)
        {

        }

        // TODO: implement
        private void btnInvitationDown_Click(object sender, EventArgs e)
        {

        }

        // Add a recommended friend to the current invitation recipients
        private void btnInviteRecommendation_Click(object sender, EventArgs e)
        {
            // Add the recipient to the list
            InviteUser(currentRecommendation);
            // Disable the button. It will be re-enabled when a different user is selected for invite.
            btnInviteRecommendation.Enabled = false;
        }

        // Add an existing friend to the current invitation recipients
        private void btnInviteFriend_Click(object sender, EventArgs e)
        {
            // Invite the friend at the top of the displayed friends 
            InviteUser(user.GetAllFriends()[friendsIndex]);
            // Disable the button. It will be re-enabled when a different user is selected for invite.
            btnInviteFriend.Enabled = false;
        }

        // Adds a user to the current invitation recipients
        private void InviteUser(Person invitee)
        {
            // Add the invitee to the recipient list
            invitedRecipients.Add(invitee);
            // Get the invitee's username
            string username = invitee.Username;
            // Check if this is the first recipient
            if(txtInvitationRecipients.Text == "")
            {
                // Set the text of the textbox to display the username
                txtInvitationRecipients.Text = username;
            }
            else
            {
                // Add the current username to the end of the list
                txtInvitationRecipients.Text += $", { username }";
            }
        }

        private void btnToggleInvitations_Click(object sender, EventArgs e)
        {

        }

        private void btnNewInvitation_Click(object sender, EventArgs e)
        {
            // Show the invitation creation UI
            SetInvitationUIVisibility(visible: true);

        }

        // Send the invitation if all fields have been filled
        private void btnSendInvitation_Click(object sender, EventArgs e)
        {
            // Store the information for the invitation (life-time and interest)
            double lifeTime = 0;
            string interest = txtInvitationInterest.Text;

            // Perform error checking for missing information, stopping at the earliest error
            // Check if there is no valid lifetime set
            if(!double.TryParse(txtInvitationLifetime.Text, out lifeTime) || lifeTime <= 0)
            {
                // Show an error message
                MessageBox.Show("Please set a positive invitation life-time.");
            }
            // Check if there are no recipients selected
            else if (!invitedRecipients.Any())
            {
                // Show an error message
                MessageBox.Show("Please select at least 1 recipient.");
            }
            // Check if there is no interest set
            else if(interest == "")
            {
                // Show an error message
                MessageBox.Show("Please enter an interest");
            }
            // Otherwise, invitation can be sent
            else
            {
                // Create the invitation with the given information
                Invitation newInvitation = new Invitation(user, invitedRecipients, interest, 
                                                          Environment.TickCount, lifeTime);
                // Send the invitation
                network.DeliverInvitation(newInvitation);
                // Show a status update
                MessageBox.Show("Invitation sent!");
                // Close the invitation creation UI
                CloseInvitationUI();
            }
        }

        // Clear and close the invitation creation UI
        private void CloseInvitationUI()
        {
            // Clear the text of all fields
            txtInvitationLifetime.Text = "";
            txtInvitationRecipients.Text = "";
            txtInvitationInterest.Text = "";
            // Clear the invitation recipients selected
            invitedRecipients.Clear();

            // Hide the invitation creation UI
            SetInvitationUIVisibility(visible: false);
        }



        // Cancel the invitation and close the creation UI
        private void btnCancelInvitation_Click(object sender, EventArgs e)
        {
            CloseInvitationUI();
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
