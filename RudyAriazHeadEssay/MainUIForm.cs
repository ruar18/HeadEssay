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
        // Strings to display as descriptive headings for the types of recommendations shown
        private string[] recommendationHeadings = new string[3] { "Friends of Friends with Same Interest",
                                                                      "Same City",
                                                                      "Same City, Same Interest" };

        // The index of the currently shown interest 
        private int interestIndex = 0;

        // The starting index of the currently shown friends
        private int friendsIndex = 0;
        // The friend that is currently shown at the top of the friends list
        private Person currentTopFriend;

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
        // Indicates whether or not an invitation is currently being created by the user. Initially false.
        private bool ongoingInvitation = false;

        
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
            SetInvitationCreationUIVisibility(visible: false);
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
            // Set the current top friend if it exists, or null if it does not
            currentTopFriend = friendsList.Any()? friendsList[friendsIndex] : null;

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

            // Ensure that the invite friend button is enabled or disabled depending on whether the current top friend has been already invited or not
            SetFriendInviteButton();

            // The remove friend button should be disabled only if there is no friend to remove or there is an ongoing invitation
            // since the user cannot remove friends while an invitation is being created 
            btnRemoveFriend.Enabled = friendsList.Any() && !ongoingInvitation;
        }

        /// <summary>
        /// Enables the friend invite button only if:
        ///     1. There is a currently selected friend
        ///     2. The current friend has not been invited yet
        /// Disables the button otherwise.
        /// </summary>
        private void SetFriendInviteButton()
        {
            // Enable the button if there is a current uninvited friend selected
            btnInviteFriend.Enabled = currentTopFriend != null && !invitedRecipients.Contains(currentTopFriend);
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
            // Scroll the friends list in the downwards direction
            ScrollList(ref friendsIndex, PopulateFriendsList, 1);
        }

        

        /*** RECOMMENDATIONS METHODS ***/
        /// <summary>
        /// Displays the user's selected (and updated) recommendation in a label
        /// and manages related UI components.
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
                // Display the username of current recommendation in a label
                lblRecommendation.Text = currentRecommendation.Username;
            }
            // Otherwise, display a placeholder
            else
            {
                // Record that there is no current recommendation
                currentRecommendation = null;
                // Display the placeholder in the label
                lblRecommendation.Text = "No recommendation";
            }

            // Enable or disable the recommendation invitation button depending on the current recommendation displayed
            SetRecommendationInviteButton();
            // The add friend button should be enabled if and only if there is a recommendation displayed
            btnAddRecommendedFriend.Enabled = recommendations.Any();
        }

        /// <summary>
        /// Enables the recommendation invite button only if:
        ///     1. There is a current recommendation
        ///     2. The current recommendation has not been invited yet
        /// Disables the button otherwise.
        /// </summary>
        private void SetRecommendationInviteButton()
        {
            // Enable the button if there is a current uninvited recommendation
            btnInviteRecommendation.Enabled = currentRecommendation != null && !invitedRecipients.Contains(currentRecommendation);
        }

        /// <summary>
        /// Updates all of the user's recommended friends lists.
        /// </summary>
        private void UpdateRecommendations()
        {
            // Update each of the possible recommendation lists
            network.UpdateFriendsOfFriends(user);
            network.UpdateFriendsOfFriendsWithSameInterest(user);
            network.UpdateSameCity(user);
            network.UpdateSameCitySameInterest(user);
        }

        /// <summary>
        /// Changes the displayed recommendation type to the next type, cycling between:
        ///     "Friends of friends with shared interest", "Non-friends in the same city", "Non-friends in the city with a shared interest".
        /// Runs when the "Next Recommendation List" button is pressed.
        /// </summary>
        private void btnNextRecommendationList_Click(object sender, EventArgs e)
        {
            // Increment the recommendation type displayed (modulo 3 to maintain the cycling between the 3 options)
            // Casting is used to enable arithmetic on RecommendationType values
            recommendationState = (RecommendationType)((int)(recommendationState + 1) % 3);
            // Update the recommendation heading with the string corresponding to the current recommendation type
            lblRecommendationListHeading.Text = recommendationHeadings[(int)recommendationState];
            // Reset the recommendation index to 0 for viewing the next recommendation list
            recommendationIndex = 0;
            // Repopulate the recommendation to accomodate for the change in recommendation type
            PopulateRecommendation();
        }

        /// <summary>
        /// Adds the currently shown friend recommendation to the user's friends list. Runs when the "Add Recommended Friend" is pressed.
        /// Precondition: currentRecommendation is non-null.
        /// </summary>
        private void btnAddRecommendedFriend_Click(object sender, EventArgs e)
        {
            // Add the current recommendation as a friend
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
            // Scroll the recommendations list in the downwards direction
            ScrollList(ref recommendationIndex, PopulateRecommendation, 1);
        }



        /*** INVITATIONS METHODS ***/
        /// <summary>
        /// Displays the currently selected outgoing or incoming invitation while managing related UI elements.
        /// Runs whenever an update to invitation information occurs.
        /// </summary>
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
                // Get the user's incoming invitations
                invitations = user.GetIncomingInvitations();
            }
            // Otherwise, outgoing invitations must be displayed
            else
            {
                // Indicate that the outgoing invitations are shown
                lblInvitationListHeading.Text = "Outgoing Invitations";
                // Get the user's outgoing invitations
                invitations = user.GetOutgoingInvitations();
            }

            // Set the invitation font to regular. This will be changed to bold if the invitation has been accepted by the user.
            txtInvitation.Font = new Font(Font, FontStyle.Regular);
            // By default, disable the accept invitation button
            btnAcceptInvitation.Enabled = false;

            // Check if there are any invitations to display
            if (invitations.Any())
            {
                // Restrict the index of the invitation to be shown to within the invitation list bounds
                invitationIndex = Math.Max(0, Math.Min(invitationIndex, invitations.Count - 1));
                // Get the invitation to be displayed 
                Invitation selectedInvitation = invitations[invitationIndex];
                // Display the invitation in a label
                txtInvitation.Text = selectedInvitation.ToString(user);

                // Check if the user is a recipient of the invitation
                if (invitationState == InvitationType.Incoming)
                {
                    // Check if the user has accepted the invitation
                    if(selectedInvitation.InvitationStateOfRecipient(user) == InvitationStatus.Accepted)
                    {
                        // Set the invitation font to bold to show that it has been accepted
                        txtInvitation.Font = new Font(Font, FontStyle.Bold);
                    }
                    // Otherwise, the user has not accepted the invitation
                    else
                    {
                        // Enable the accept invitation button since there is an invitation to accept
                        btnAcceptInvitation.Enabled = true;
                    }
                }
            }
            // Otherwise, display a placeholder
            else
            {
                // Show the placeholder in the textbox
                txtInvitation.Text = "No invitation";
            }

            // Disable or enable the invitation up or down buttons according to list position
            SetScrollButtonActivity(btnInvitationDown, btnInvitationUp, invitationIndex, invitations);
            // The delete invitation button should be enabled if and only if there is an invitation to delete
            btnDeleteInvitation.Enabled = invitations.Any();
        }

        /// <summary>
        /// Shows or hides the invitation creation UI. Some UI components are enabled or disabled instead of shown or hidden.
        /// </summary>
        /// <param name="visible">Indicates whether the invitation creation UI should be shown (true) or hidden (false).</param>
        private void SetInvitationCreationUIVisibility(bool visible)
        {
            // Set visibility for prompt labels
            lblInvitationCreation.Visible = visible;
            lblPromptLifetime.Visible = visible;
            lblPromptRecipients.Visible = visible;
            lblPromptInterest.Visible = visible;

            // Set visibility for textbox fields in the invitation creation UI
            txtInvitationLifetime.Visible = visible;
            txtInvitationRecipients.Visible = visible;
            txtInvitationInterest.Visible = visible;

            // Set visibility for buttons used in the invitation creatino UI
            btnSendInvitation.Visible = visible;
            btnCancelInvitation.Visible = visible;
            btnInviteRecommendation.Visible = visible;
            btnInviteFriend.Visible = visible;

            // When the invitation creation UI is visible, the "New Invitation" button should be disabled and vice versa
            btnNewInvitation.Enabled = !visible;
            // When the invitation creation UI is visible, the "Remove Friend" button should be disabled
            btnRemoveFriend.Enabled = !visible;
            
        }
        
        /// <summary>
        /// Adds the currently selected recommended friend to the current invitation recipients while managing related
        /// UI components. Runs when the "Invite" button of the recommendation UI is pressed.
        /// </summary>
        private void btnInviteRecommendation_Click(object sender, EventArgs e)
        {
            // Add the recipient to the recipient list
            invitedRecipients.Add(currentRecommendation);
            // Show the invitee in the list 
            ShowInvitedUser(currentRecommendation);
            // Disable the button. It will be re-enabled when a different user is selected for invite, 
            // or when a new invitation is created by the user.
            btnInviteRecommendation.Enabled = false;
        }

        /// <summary>
        /// Adds the currently selected friend to the current invitation recipients while managing related
        /// UI components. Runs when the "Invite" button of the friends UI is pressed.
        /// </summary>
        private void btnInviteFriend_Click(object sender, EventArgs e)
        {
            // Add the recipient to the recipient list
            invitedRecipients.Add(currentTopFriend);
            // Show the invitee
            ShowInvitedUser(currentTopFriend);
            // Disable the button. It will be re-enabled when a different user is selected for invite, 
            // or when a new invitation is created by the user.
            btnInviteFriend.Enabled = false;
        }

        /// <summary>
        /// Shows a newly-invited user's username in the recipients textbox.
        /// </summary>
        /// <param name="invitee">The invited user for whom to show the username.</param>
        private void ShowInvitedUser(Person invitee)
        {
            // Get the invitee's username
            string username = invitee.Username;
            // Check if this is the first recipient for proper formatting
            if(txtInvitationRecipients.Text == "")
            {
                // Set the text of the textbox to display the username
                txtInvitationRecipients.Text = username;
            }
            // Otherwise, the username can be appended onto the end of the textbox's text
            else
            {
                // Add the current username to the end of the list
                txtInvitationRecipients.Text += $", { username }";
            }
        }

        /// <summary>
        /// Changes the invitation type shown in the invitation list (incoming to outgoing and vice versa).
        /// Runs when the "Toggle Invitations" button is pressed.
        /// </summary>
        private void btnToggleInvitations_Click(object sender, EventArgs e)
        {
            // Implicitly cast InvitationType to integer to toggle between the invitation types
            invitationState = 1 - invitationState;
            // Repopulate the invitations to account for the type change
            PopulateInvitation();
        }

        /// <summary>
        /// Begins a new outgoing invitation. Runs when the "New Invitation" button is pressed.
        /// </summary>
        private void btnNewInvitation_Click(object sender, EventArgs e)
        {
            // Indicate that there is an ongoing invitation being created 
            ongoingInvitation = true;
            // Enable or disable the invite buttons according to whether there are friends and recommendations to invite
            SetFriendInviteButton();
            SetRecommendationInviteButton();
            // Show the invitation creation UI
            SetInvitationCreationUIVisibility(visible: true);
        }
        
        /// <summary>
        /// Sends a newly-created outgoing invitation if all fields have been filled.
        /// </summary>
        private void btnSendInvitation_Click(object sender, EventArgs e)
        {
            // Store the life-time for the information (intially set to 0)
            double lifeTime = 0;
            // Store the interest for the information (found in the interest textbox of the invitation)
            string interest = txtInvitationInterest.Text;

            // Perform error checking for missing information, stopping at the earliest error
            // Check if there is no valid lifetime set (non-numerical or non-positive)
            if(!double.TryParse(txtInvitationLifetime.Text, out lifeTime) || lifeTime <= 0)
            {
                // Show an appropriate error message in a MessageBox
                MessageBox.Show("Please set a positive invitation life-time.");
            }
            // Check if there are no recipients selected
            else if (!invitedRecipients.Any())
            {
                // Show an appropriate error message in a MessageBox
                MessageBox.Show("Please select at least 1 recipient.");
            }
            // Check if there is no interest set
            else if(interest == "")
            {
                // Show an appropriate error message in a MessageBox
                MessageBox.Show("Please enter an interest");
            }
            // Otherwise, the invitation can be sent
            else
            {
                // Create a dictionary of recipient states with the given recipients
                // No recipients have accepted the invitation yet, so set the invitation status as "Pending" for all recipients
                // Precondition: invited recipients are unique
                Dictionary<Person, InvitationStatus> recipientStates = 
                    invitedRecipients.ToDictionary(person => person, person => InvitationStatus.Pending);

                // Create the invitation with the given information
                Invitation newInvitation = new Invitation(user, recipientStates, interest, 
                                                          Environment.TickCount, lifeTime);
                // Send the invitation
                network.DeliverInvitation(newInvitation);
                // Show a status update in a MessageBox
                MessageBox.Show("Invitation sent!");
                // Close the invitation creation elements
                CloseInvitationCreation();

                // Set the invitation details to display the newly-sent invitation
                invitationState = InvitationType.Outgoing;
                invitationIndex = user.GetOutgoingInvitations().Count - 1;
                // Update the displayed invitation list 
                PopulateInvitation();
            }
        }
        
        /// <summary>
        /// Clears and close the invitation creation UI.
        /// </summary>
        private void CloseInvitationCreationUI()
        {
            // Clear the text of all fields used in the invitation creation
            txtInvitationLifetime.Text = "";
            txtInvitationRecipients.Text = "";
            txtInvitationInterest.Text = "";
            
            // Hide the invitation creation UI
            SetInvitationCreationUIVisibility(visible: false);
        }

        /// <summary>
        /// Closes a currently-created invitation if there is one.
        /// </summary>
        private void CloseInvitationCreation()
        {
            // Indicate that there is no ongoing invitation created
            ongoingInvitation = false;
            // Clear the recipient list
            invitedRecipients.Clear();
            // Close the invitation creation UI
            CloseInvitationCreationUI();
        }

        /// <summary>
        /// Cancels a currently-created outgoing invitation. Runs when the "Cancel Invitation" button is pressed.
        /// </summary>
        private void btnCancelInvitation_Click(object sender, EventArgs e)
        {
            // Close the invitation created
            CloseInvitationCreation();
        }
        
        /// <summary>
        /// Adds a pending incoming invitation to the accepted list.
        /// </summary>
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
            // Scroll the invitations list in the downwards direction
            ScrollList(ref invitationIndex, PopulateInvitation, 1);
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
