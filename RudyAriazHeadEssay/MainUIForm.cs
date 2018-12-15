/*
 * Rudy Ariaz
 * December 16, 2018
 * MainUIForm class manages all main user-interface components, and stores the current network
 * and logged-in person.
 */
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
        // The current network
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
        // The types of invitations to display
        enum InvitationType { Incoming, Outgoing };
        // The type of invitation shown, intialized to show incoming invitations
        private InvitationType invitationState = InvitationType.Outgoing;
        

        public MainUIForm(Network network, Person user)
        {
            InitializeComponent();
            // Make this form fullscreen
            WindowState = FormWindowState.Maximized;
            // Store the current network and logged-in user
            this.network = network;
            this.user = user;
            // Display the logged-in user's full name
            lblFullName.Text = $"{ user.FirstName } { user.LastName }";
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
            // Populate the invitation
            PopulateInvitation();
        }

        // Disables or enables up/down buttons for scrolling depending on list position
        private void SetScrollButtonActivity<T>(Button btnDown, Button btnUp, int itemIndex, List<T> scrolledList)
        {
            // If there are no earlier items, disable the up button 
            if(itemIndex == 0)
            {
                btnUp.Enabled = false;
            }
            // Otherwise, enable it
            else
            {
                btnUp.Enabled = true;
            }

            // If there are no later items, disable the down button
            if(itemIndex >= scrolledList.Count - 1)
            {
                btnDown.Enabled = false;
            }
            // Otherwise, enable it 
            else
            {
                btnDown.Enabled = true;
            }
        }

        // Populate the user's friends list
        private void PopulateFriendsList()
        {
            // Get the user's friends
            List<Person> friendsList = user.GetAllFriends();

            // If the index of the first friend to be displayed exceeds the top bound of the list,
            // set it to the last index of the list, unless the list is empty and the index should be set to 0.
            friendsIndex = Math.Max(0, Math.Min(friendsIndex, friendsList.Count - 1));

            // Disable or enable the friend up or down buttons according to list position
            SetScrollButtonActivity(btnFriendDown, btnFriendUp, friendsIndex, friendsList);

            // Determine the exclusive upper bound of the indices of friends to display
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
            if (!friendsList.Any() || invitedRecipients.Contains(friendsList[friendsIndex]))
            {
                btnInviteFriend.Enabled = false;
            }
            // Otherwise, enable the button
            else
            {
                btnInviteFriend.Enabled = true;
            }

            // The remove friend button should be disabled if there is no friend to remove, and vice versa
            btnRemoveFriend.Enabled = friendsList.Any();
        }

        // Shows the current selected interest
        // TODO: try a class to temporarily store the information, a mediator
        // between the user and the interface 
        private void PopulateInterest()
        {
            // Get the user's interests
            List<string> interests = user.GetAllInterests();

            // If the index of the interest to be displayed exceeds the top bound of the list,
            // set it to the last index of the list, unless the list is empty and the index should be set to 0.
            interestIndex = Math.Max(0, Math.Min(interestIndex, interests.Count - 1));

            // Disable or enable the interest up or down buttons according to list position
            SetScrollButtonActivity(btnInterestDown, btnInterestUp, interestIndex, interests);

            // If there are no interests in the list, display a placeholder
            if (user.GetAllInterests().Count == 0)
            {
                lblInterest.Text = "No interests";
                // Disable the remove interest button since there is no interest to remove
                btnRemoveInterest.Enabled = false;
            }
            else
            {
                lblInterest.Text = interests[interestIndex];
                // Enable the remove interest button 
                btnRemoveInterest.Enabled = true;
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

        // Delete a friend
        private void btnDeleteFriend_Click(object sender, EventArgs e)
        {
            // TODO: remove by index
            // Removes the friend at the current displayed index
            user.RemoveFriend(user.GetAllFriends()[friendsIndex]);
            // Repopulate recommendations
            PopulateRecommendation();
            // Repopulate friends
            PopulateFriendsList();
        }

        // Adds an interest to the user's interest list
        private void btnAddInterest_Click(object sender, EventArgs e)
        {
            // If no interest was entered, display an appropriate error message
            if(txtAddInterest.Text == "")
            {
                MessageBox.Show("Please enter an interest.");
            }
            // If the interest entered is a duplicate, display an appropriate error message
            else if (user.GetAllInterests().Contains(txtAddInterest.Text))
            {
                // Show the error message
                MessageBox.Show("This interest was already added. Please enter a different one.");
            }
            // Otherwise, add the interest
            else
            {
                // Add the interest
                user.AddInterest(txtAddInterest.Text);
                // Clear the add interest textbox
                txtAddInterest.Clear();
                // Display the interests again
                PopulateInterest();
                // Repopulate recommendations to adjust to the added interest
                PopulateRecommendation();
            }
        }

        private void btnRemoveInterest_Click(object sender, EventArgs e)
        {
            // Remove the interest
            user.RemoveInterest(lblInterest.Text);
            // Display the interests again
            PopulateInterest();
            // Repopulate recommendations to adjust to the removed interest
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

            // Disable or enable the recommendation up or down buttons according to list position
            SetScrollButtonActivity(btnRecommendationDown, btnRecommendationUp, recommendationIndex, recommendations);

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

            // When the invitation creation UI is visible, the New Invitation button should be disabled
            // and vice versa
            btnNewInvitation.Enabled = !visible;
            // The remove friend button should be disabled during invitation creation
            btnRemoveFriend.Enabled = !visible;
        }



        // TODO: store the current invitations?
        // TODO: refactor this to make it simpler
        private void PopulateInvitation()
        {
            // Delete any inactive invitations prior to displaying them
            network.DeleteInactiveInvitations();
            // Will store the list of invitations 
            List<Invitation> invitations = null;

            // Check if incoming invitations must be displayed
            if (invitationState == InvitationType.Incoming)
            {
                
                // Indicate that the incoming invitations are shown
                lblInvitationList.Text = "Incoming Invitations";
                // Enable the accept invitation button
                btnAcceptInvitation.Enabled = true;

                // Get the user's incoming invitations
                invitations = user.GetIncomingInvitations();
            }
            else
            {
                // Indicate that the outgoing invitations are shown
                lblInvitationList.Text = "Outgoing Invitations";
                // Disable the accept invitation button
                btnAcceptInvitation.Enabled = false;

                // Get the user's outgoing invitations
                invitations = user.GetOutgoingInvitations();
            }

            // Check if there are any invitations
            if (invitations.Any())
            {
                // If the index of the invitation to be displayed exceeds the top bound of the invitation list,
                // set it to the last index of the list, unless the list is empty and the index should be set to 0.
                // TODO: abstract this?
                invitationIndex = Math.Max(0, Math.Min(invitationIndex, invitations.Count - 1));

                // The invitation to be displayed 
                Invitation selectedInvitation = invitations[invitationIndex];

                // Display the one at the current selected index
                txtInvitation.Text = selectedInvitation.ToString(user);
                // If the user is a recipient and the invitation has been accepted by the user, 
                // display it in bold text and disable the accept invitation button
                if (invitationState == InvitationType.Incoming && 
                    selectedInvitation.InvitationStateOfRecipient(user) == InvitationStatus.Accepted)
                {
                    txtInvitation.Font = new Font(Font, FontStyle.Bold);
                    btnAcceptInvitation.Enabled = false;
                }
                // If the invitation has not been accepted yet or the user is the creator, 
                // display in regular text and enable the accept invitation button
                else
                {
                    txtInvitation.Font = new Font(Font, FontStyle.Regular);
                    btnAcceptInvitation.Enabled = true;
                }

                // Enable the delete button since there is an invitation to delete
                btnDeleteInvitation.Enabled = true;
            }
            // Otherwise, display a placeholder
            else
            {
                txtInvitation.Text = "No invitation";
                // Disable the delete button since there is no invitation to delete
                btnDeleteInvitation.Enabled = false;
                // Display the placeholder with a regular font
                txtInvitation.Font = new Font(Font, FontStyle.Regular);
            }

            // Disable or enable the invitation up or down buttons according to list position
            SetScrollButtonActivity(btnInvitationDown, btnInvitationUp, invitationIndex, invitations);

            // Disable the down button if there are no later invitations to show
            if (invitationIndex >= invitations.Count - 1)
            {
                btnInvitationDown.Enabled = false;
            }
            // Otherwise, enable it 
            else
            {
                btnInvitationDown.Enabled = true;
            }
        }
        

        // TODO: implement
        private void btnInvitationUp_Click(object sender, EventArgs e)
        {
            // Decrement the invitation index
            invitationIndex--;
            // Repopulate the invitation
            PopulateInvitation();
        }

        // TODO: implement
        private void btnInvitationDown_Click(object sender, EventArgs e)
        {
            // Increment the invitation index
            invitationIndex++;
            // Repopulate the invitation
            PopulateInvitation();
        }

        // Add a recommended friend to the current invitation recipients
        private void btnInviteRecommendation_Click(object sender, EventArgs e)
        {
            // Add the recipient to the list
            ShowInvitedUser(currentRecommendation);
            // Disable the button. It will be re-enabled when a different user is selected for invite,
            // or when the invitation is sent.
            btnInviteRecommendation.Enabled = false;
        }

        // Add an existing friend to the current invitation recipients
        private void btnInviteFriend_Click(object sender, EventArgs e)
        {
            // Invite the friend at the top of the displayed friends 
            ShowInvitedUser(user.GetAllFriends()[friendsIndex]);
            // Disable the button. It will be re-enabled when a different user is selected for invite,
            // or when the invitation is sent.
            btnInviteFriend.Enabled = false;
        }

        // Adds a user to the current invitation recipients
        private void ShowInvitedUser(Person invitee)
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

        // Change incoming to outgoing invitations and vice versa
        private void btnToggleInvitations_Click(object sender, EventArgs e)
        {
            if(invitationState == InvitationType.Incoming)
            {
                invitationState = InvitationType.Outgoing;
            }
            else
            {
                invitationState = InvitationType.Incoming;
            }
            PopulateInvitation();
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
                // Create a dictionary of recipient states with the given recipients
                // No recipients have accepted the invitation yet
                // Precondition: invited recipients are unique
                Dictionary<Person, InvitationStatus> recipientStates = 
                    invitedRecipients.ToDictionary(x => x, x => InvitationStatus.Pending);

                // Create the invitation with the given information
                Invitation newInvitation = new Invitation(user, recipientStates, interest, 
                                                          Environment.TickCount, lifeTime);
                // Send the invitation
                network.DeliverInvitation(newInvitation);
                // Show a status update
                MessageBox.Show("Invitation sent!");
                // Close the invitation creation UI
                CloseInvitationUI();

                // Enable the invitation button for a new invitation
                btnInviteFriend.Enabled = true;
                btnInviteRecommendation.Enabled = true;

                // Set the invitation details to display the added invitation
                invitationState = InvitationType.Outgoing;
                invitationIndex = user.GetOutgoingInvitations().Count - 1;
                // Update the displayed invitation list 
                PopulateInvitation();
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

        
        // Adds a pending incoming invitation to the accepted list
        private void btnAcceptInvitation_Click(object sender, EventArgs e)
        {
            // Get the current displayed invitation
            Invitation selectedInvitation = user.GetIncomingInvitations()[invitationIndex];
            // If the invitation has expired, display an appropriate error message
            if (!selectedInvitation.IsActive())
            {
                MessageBox.Show("Invitation has already expired.");
            }
            // Otherwise, the invitation is accepted by the user
            else
            {
                user.AcceptInvitation(selectedInvitation);
            }
            // Repopulate the invitation information
            PopulateInvitation();
        }


        // Delete an incoming or outgoing invitation
        // TODO: treat differently based on outgoing/incoming
        private void btnDeleteInvitation_Click(object sender, EventArgs e)
        {
            // Will store the current displayed invitation
            Invitation selectedInvitation = null;

            // Get the invitation
            if (invitationState == InvitationType.Outgoing)
            {
                selectedInvitation = user.GetOutgoingInvitations()[invitationIndex];
                selectedInvitation.Deactivate();
            }
            else
            {
                selectedInvitation = user.GetIncomingInvitations()[invitationIndex];
                user.DeleteIncomingInvitation(selectedInvitation);
            }
            
            // Repopulate the current invitation
            PopulateInvitation();
        }

        // Allow the user to directly refresh invitation information
        private void btnRefreshInvitations_Click(object sender, EventArgs e)
        {
            PopulateInvitation();
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
