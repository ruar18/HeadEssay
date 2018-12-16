/*
 * Rudy Ariaz
 * December 16, 2018
 * MainUIForm class manages all main user-interface components, including functionality to change interests,
 * befriend friend recommendations, send and browse invitations, and manage friends. The class 
 * stores the current network and logged-in user to be able to interact with them.
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
        // The logged-in user
        private Person user;
        // A list of labels displaying friends
        private List<Label> friendLabelList;

        // The possible friend recommendations that can be shown
        enum RecommendationType
        {
            FriendsOfFriendsSameInterest,
            SameCity,
            SameCitySameInterest
        };
        // The current type of recommendations shown, initialized to show friends of friends by default
        private RecommendationType recommendationState = RecommendationType.FriendsOfFriendsSameInterest;
        // The index of the currently shown recommendation
        private int recommendationIndex = 0;
        // The currently shown recommendation
        private Person currentRecommendation;
        // Strings to display as descriptions for the types of recommendations shown
        private string[] recommendationDescriptions = new string[3] { "Friends of Friends with Same Interest",
                                                                      "Same City",
                                                                      "Same City, Same Interest" };

        // The index of the currently shown interest 
        private int interestIndex = 0;

        // The starting index of the currently shown friends
        private int friendsIndex = 0;

        // The index of the currently shown invitation
        private int invitationIndex = 0;
        // The currently selected recipients for an invitation
        private List<Person> invitedRecipients = new List<Person>();
        // The possible types of invitations to display
        enum InvitationType
        {
            Incoming,
            Outgoing
        };
        // The type of invitation currently shown, intialized to show incoming invitations
        private InvitationType invitationState = InvitationType.Outgoing;
        
        /// <summary>
        /// Constructs a new MainUIForm object with the given network and logged-in user.
        /// </summary>
        /// <param name="network">The network that the main UI interacts with.</param>
        /// <param name="user">The currently logged-in user.</param>
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
            // Populate all UI lists
            PopulateAllLists();
            // Hide invitation creation UI at first
            SetInvitationUIVisibility(visible: false);
        }
        
        /// <summary>
        /// Populates the user's information label lists.
        /// </summary>
        private void PopulateAllLists()
        {
            // Populate the currently displayed interest
            PopulateInterest();
            // Populate the user's currently displayed friends
            PopulateFriendsList();
            // Populate the user's currently displayed friend recommendation
            PopulateRecommendation();
            // Populate the user's currently displayed invitation
            PopulateInvitation();
        }
        
        /*** UTILITY METHODS ***/
        /// <summary>
        /// Disables or enables up and down buttons for scrolling through a given list depending on the current list position.
        /// </summary>
        /// <typeparam name="T">The type of element in the information list.</typeparam>
        /// <param name="btnDown">The "scroll down" button.</param>
        /// <param name="btnUp">The "scroll up" button</param>
        /// <param name="itemIndex">The index that the currently displayed subset of the list begins from.</param>
        /// <param name="scrolledList">The list to be scrolled with the up and down buttons.</param>
        private void SetScrollButtonActivity<T>(Button btnDown, Button btnUp, int itemIndex, List<T> scrolledList)
        {
            // If there are no earlier items to view (the value at the first index is already shown), disable the up button 
            if(itemIndex == 0)
            {
                btnUp.Enabled = false;
            }
            // Otherwise, enable it
            else
            {
                btnUp.Enabled = true;
            }

            // If there are no later items to view (the value at the last index is already shown), disable the down button
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

        /// <summary>
        /// Scrolls a given UI list up or down.
        /// </summary>
        /// <param name="index">Index of list to change when scrolling.</param>
        /// <param name="populateMethod">Method to use for repopulating the list while scrolling.</param>
        /// <param name="direction">Direction of scrolling. 1 for down, -1 for up.</param>
        private void ScrollList(ref int index, Action populateMethod, int direction)
        {
            // Change the index for scrolling
            index += direction;
            // Repopulate the UI list
            populateMethod();
        }

        /// <summary>
        /// Returns a given list index, restricted to within the list lower bound and upper bound indices.
        /// </summary>
        /// <param name="index">The index to compare to the list indices.</param>
        /// <param name="listLength">The length of the list indexed.</param>
        /// <returns>The original index, if it does not exceed the list bounds, or 
        /// the lower or upper bounds of the list indices if it does.</returns>
        private int RestrictWithinBounds(int index, int listLength)
        {
            // If the index exceeds the list upper bound, the last index of the list will be returned.
            // If the list is empty, the last index is effectively 0.
            return Math.Max(0, Math.Min(index, listLength - 1));
        }


        /*** INTERESTS METHODS ***/
        /// <summary>
        /// Displays the current selected interest in a label.
        /// </summary>
        private void PopulateInterest()
        {
            // Get the user's interests
            List<string> interests = user.GetAllInterests();
            // Ensure that the index of the interest to be displayed is within bounds
            interestIndex = RestrictWithinBounds(interestIndex, interests.Count);
            // Disable or enable the interest up or down buttons according to list position
            SetScrollButtonActivity(btnInterestDown, btnInterestUp, interestIndex, interests);

            // If there are no interests in the list, display a placeholder
            if (!interests.Any())
            {
                // Display the placeholder in the label
                lblInterest.Text = "No interests";
                // Disable the remove interest button since there is no interest to remove
                btnRemoveInterest.Enabled = false;
            }
            // Otherwise, display the selected interest
            else
            {
                // Display the interest in a label
                lblInterest.Text = interests[interestIndex];
                // Enable the remove interest button 
                btnRemoveInterest.Enabled = true;
            }
        }
        
        /// <summary>
        /// Adds an interest to the user's interests list. Runs when the "Add Interest" button is pressed.
        /// </summary>
        private void btnAddInterest_Click(object sender, EventArgs e)
        {
            // If no interest was entered, display an appropriate error message
            if (txtAddInterest.Text == "")
            {
                // Show the error message in a MessageBox
                MessageBox.Show("Please enter an interest.");
            }
            // If the interest entered is a duplicate for the user, display an appropriate error message
            else if (user.GetAllInterests().Contains(txtAddInterest.Text))
            {
                // Show the error message in a MessageBox
                MessageBox.Show("This interest was already added. Please enter a different one.");
            }
            // Otherwise, add the interest to the user's list
            else
            {
                // Add the interest to the list
                user.AddInterest(txtAddInterest.Text);
                // Clear the "Add Interest" textbox for future use
                txtAddInterest.Clear();
                // Repopulate the interests list
                PopulateInterest();
                // Repopulate recommendations to account for the added interest
                PopulateRecommendation();
            }
        }

        /// <summary>
        /// Removes an interest from the user's interests list. Runs when the "Remove Interest" button is pressed.
        /// </summary>
        private void btnRemoveInterest_Click(object sender, EventArgs e)
        {
            // Remove the currently shown interest
            user.RemoveInterest(lblInterest.Text);
            // Repopulate the current interest shown 
            PopulateInterest();
            // Repopulate recommendations to account for the removed interest
            PopulateRecommendation();
        }

        /// <summary>
        /// Scrolls one interest up the interests list. Runs when the up button of the interest list is pressed.
        /// </summary>
        private void btnInterestUp_Click(object sender, EventArgs e)
        {
            // Scroll the interests list in the upwards direction
            ScrollList(ref interestIndex, PopulateInterest, -1);
        }

        /// <summary>
        /// Scrolls one interest down the interests list. Runs when the down button of the interest list is pressed.
        /// </summary>
        private void btnInterestDown_Click(object sender, EventArgs e)
        {
            // Scroll the interests list in the downwards direction
            ScrollList(ref interestIndex, PopulateInterest, 1);
        }


        /*** FRIENDS METHODS ***/
        /// <summary>
        /// Displays up to 5 friends' usernames at a time in a list of labels. Manages UI elements
        /// associated with this display of friends.
        /// </summary>
        private void PopulateFriendsList()
        {
            // Get the user's friends
            List<Person> friendsList = user.GetAllFriends();
            // Ensure that the first index of the friends to be displayed is within bounds
            friendsIndex = RestrictWithinBounds(friendsIndex, friendsList.Count);
            // Disable or enable the friend up or down buttons according to list position
            SetScrollButtonActivity(btnFriendDown, btnFriendUp, friendsIndex, friendsList);
            // Determine the exclusive upper bound of the indices of friends to display
            int upperBound = Math.Min(user.GetAllFriends().Count, friendsIndex + 5);

            // Iterate through the friends that should be displayed
            for (int i = friendsIndex; i < upperBound; i++)
            {
                // Display the username of the friend in a label
                // i - friendsIndex is within the interval [0, 5) in order to index into a label in the label list
                friendLabelList[i - friendsIndex].Text = friendsList[i].Username;
            }
            // If not all labels have been filled, display placeholders
            for (int i = upperBound; i < friendsIndex + 5; i++)
            {
                // Display the placeholder text in a label
                friendLabelList[i - friendsIndex].Text = "No friend";
            }

            // If there are no friends displayed or the top friend has already been invited, disable the invite button
            if (!friendsList.Any() || invitedRecipients.Contains(friendsList[friendsIndex]))
            {
                btnInviteFriend.Enabled = false;
            }
            // Otherwise, enable the button so that the top friend can be invited
            else
            {
                btnInviteFriend.Enabled = true;
            }

            // The remove friend button should be disabled if there is no friend to remove, and vice versa
            btnRemoveFriend.Enabled = friendsList.Any();
        }

        /// <summary>
        /// Initializes a list of 5 labels that can display the user's friends.
        /// </summary>
        private void CreateFriendLabelList()
        {
            // Initialize the list with the existing labels
            friendLabelList = new List<Label> { lblFriend1, lblFriend2, lblFriend3, lblFriend4, lblFriend5 };
        }
        
        /// <summary>
        /// Removes the friend whose username is displayed at the top of the friends list from the user's 
        /// friends list. Runs when the "Remove Friend" button is pressed.
        /// </summary>
        private void btnRemoveFriend_Click(object sender, EventArgs e)
        {
            // Removes the friend at the current top index
            user.RemoveFriendAt(friendsIndex);
            // Repopulate recommendations to account for the friend change
            PopulateRecommendation();
            // Repopulate friends to display the friend change
            PopulateFriendsList();
        }

        /// <summary>
        /// Scrolls one friend up the friends list. Runs when the up button of the friends list is pressed.
        /// </summary>
        private void btnFriendUp_Click(object sender, EventArgs e)
        {
            // Scroll the friends list in the upwards direction
            ScrollList(ref friendsIndex, PopulateFriendsList, -1);
        }

        /// <summary>
        /// Scrolls one friend down the friends list. Runs when the down button of the friends list is pressed.
        /// </summary>
        private void btnFriendDown_Click(object sender, EventArgs e)
        {
            // Increment the index
            friendsIndex++;
            // Repopulate the friends
            PopulateFriendsList();
        }

        

        /*** RECOMMENDATIONS METHODS ***/
        /// <summary>
        /// Displays the user's selected (and updated) recommendation in a label
        /// and managing related UI components.
        /// </summary>
        /// <remarks>
        /// Runs whenever a change in recommendations might occur.
        /// </remarks>
        private void PopulateRecommendation()
        {
            // The list of recommendations
            List<Person> recommendations;

            // Update the user's recommendations
            UpdateRecommendations();

            // Check if the recommendation to be displayed is of a friend of friend with a shared interest
            if (recommendationState == RecommendationType.FriendsOfFriendsSameInterest)
            {
                // Get the appropriate recommendations
                recommendations = user.GetFriendsOfFriendsSameInterest();
            }
            // Check if the recommendation to be displayed is of a non-friend in the same city as the user
            else if (recommendationState == RecommendationType.SameCity)
            {
                // Get the appropriate recommendations
                recommendations = user.GetSameCity();
            }
            // Otherwise, the recommendation to be displayed is of a non-friend in the same city with a shared interest
            else
            {
                // Get the appropriate recommendations
                recommendations = user.GetSameCitySameInterest();
            }

            // Disable or enable the recommendation up or down buttons according to the list position
            SetScrollButtonActivity(btnRecommendationDown, btnRecommendationUp, recommendationIndex, recommendations);

            // Check if there's a user to recommend
            if (recommendations.Any())
            {
                // Get the current recommended user
                currentRecommendation = recommendations[recommendationIndex];
                // Display the username of current recommendation
                lblRecommendation.Text = currentRecommendation.Username;
                // Disable the invite button if the recommended user has already been invited
                btnInviteRecommendation.Enabled = !invitedRecipients.Contains(currentRecommendation);
            }
            // Otherwise, display a placeholder
            else
            {
                // Display 
                lblRecommendation.Text = "No recommendation";
                // Disable the invite button since there is no recommended friend to invite
                btnInviteRecommendation.Enabled = false;
            }            
        }

        /// <summary>
        /// Updates all of the user's recommended friends lists.
        /// </summary>
        private void UpdateRecommendations()
        {
            // Update each of the possible lists
            network.UpdateFriendsOfFriends(user);
            network.UpdateFriendsOfFriendsWithSameInterest(user);
            network.UpdateSameCity(user);
            network.UpdateSameCitySameInterest(user);
        }

        // Go to the next type of recommendation
        private void btnNextRecommendationList_Click(object sender, EventArgs e)
        {
            recommendationState = (RecommendationType)((int)(recommendationState + 1) % 3);
            lblRecommendationListHeading.Text = recommendationDescriptions[(int)recommendationState];
            // Reset the recommendation index to 0 for viewing the next recommendation list
            recommendationIndex = 0;
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

        /// <summary>
        /// Scrolls one recommendation up the recommendations list. Runs when the up button of the recommendations list is pressed.
        /// </summary>
        private void btnRecommendationUp_Click(object sender, EventArgs e)
        {
            // Scroll the recommendations list in the upwards direction
            ScrollList(ref recommendationIndex, PopulateRecommendation, -1);
        }

        /// <summary>
        /// Scrolls one recommendation down the recommendations list. Runs when the down button of the recommendations list is pressed.
        /// </summary>
        private void btnRecommendationDown_Click(object sender, EventArgs e)
        {
            // Increment the index
            recommendationIndex++;
            // Repopulate the recommendation
            PopulateRecommendation();
        }



        /*** INVITATIONS METHODS ***/
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
                lblInvitationListHeading.Text = "Incoming Invitations";
                // Enable the accept invitation button
                btnAcceptInvitation.Enabled = true;

                // Get the user's incoming invitations
                invitations = user.GetIncomingInvitations();
            }
            else
            {
                // Indicate that the outgoing invitations are shown
                lblInvitationListHeading.Text = "Outgoing Invitations";
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
                CloseCreationInvitationUI();

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
        private void CloseCreationInvitationUI()
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
            CloseCreationInvitationUI();
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


        /// <summary>
        /// Scrolls one invitation up the invitations list. Runs when the up button of the invitations list is pressed.
        /// </summary>
        private void btnInvitationUp_Click(object sender, EventArgs e)
        {
            // Scroll the invitations list in the upwards direction
            ScrollList(ref invitationIndex, PopulateInvitation, -1);
        }

        /// <summary>
        /// Scrolls one invitation down the invitations list. Runs when the down button of the invitations list is pressed.
        /// </summary>
        private void btnInvitationDown_Click(object sender, EventArgs e)
        {
            // Increment the invitation index
            invitationIndex++;
            // Repopulate the invitation
            PopulateInvitation();
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

        private void MainUIForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
