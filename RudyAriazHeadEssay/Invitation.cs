/*
 * Rudy Ariaz
 * December 16, 2018
 * Invitation objects are used to encapsulate information regarding each of a Person's incoming and outgoing
 * invitations from other or to other users, including information about time, recipients, creator, and interest.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RudyAriazHeadEssay
{
    // Stores information regarding the status of a given invitation
    public enum InvitationStatus
    {
        Accepted,
        Rejected,
        Pending
    }

    public class Invitation
    {
        // Lifespan of the invitation in minutes 
        private double lifeTime;
        // Time when the invitation was created in milliseconds since the computer started
        private int timeCreated;
        /// <summary>Get (and set in the Invitation constructor only) the creator's interest used in the invitation.</summary>
        /// <value>A string representation of the interest used in the invitation.</value>
        public string Interest { get; }
        /// <summary>Get (and set in the Invitation constructor only) the creator of the invitation.</summary>
        /// <value>The Person object who is the creator of the invitation.</value>
        public Person Creator { get; }
        // A mapping between the Person recipients and the acceptance state of their invitation
        private Dictionary<Person, InvitationStatus> recipientStates;
        // Cosntant: the number of milliseconds in a minute, used to convert between milliseconds and minutes
        private const int MS_IN_MINUTE = 60000;
        // Stores whether or not the invitation was manually deactivated
        private bool active = true;


        /// <summary>
        /// Constructs a complete Invitation with the given information.
        /// Precondition: none of the arguments are null or empty.
        /// </summary>
        /// <param name="creator">The Person object who sent the invitation.</param>
        /// <param name="recipients">A mapping between the recipients and their acceptance states of the invitation.</param>
        /// <param name="interest">A string storing the interest used in the invitation.</param>
        /// <param name="timeCreated">An integer storing the number of milliseconds elapsed since the computer started.</param>
        /// <param name="lifeTime">A double storing the life-time of the invitation in minutes.</param>
        public Invitation(Person creator, Dictionary<Person, InvitationStatus> recipients, 
                          string interest, int timeCreated, double lifeTime )
        {
            // Set the invitation's fields to the given arguments
            Creator = creator;
            this.recipientStates = recipients;
            Interest = interest;
            this.timeCreated = timeCreated;
            this.lifeTime = lifeTime;
        }

        /// <summary>
        /// Checks if the invitation is still active, has expired, or has been deactivated.
        /// </summary>
        /// <returns>True if the invitation is still active, false if it has expired or has been deactivated.</returns>
        public bool IsActive()
        {
            // If the invitation has been deactivated, it is automatically inactive
            if (!active) return false;
            // Otherwise, calculate time elapsed in milliseconds, and compare to the life-time in milliseconds.
            // If time elapsed is greater, the invitation is inactive. Otherwise, it is still active.
            return Environment.TickCount - timeCreated <= lifeTime * MS_IN_MINUTE;
        }

        /// <summary>
        /// Gets the remaining life-time of the invitation in minutes.
        /// </summary>
        /// <returns>The time left until expiration in minutes. Rounded to 1 decimal place,
        /// or 3 if the life-time is less than 0.1 minutes.</returns>
        public double TimeLeft()
        {
            // Calculate the unrounded remaining time in minutes. Subtract the time elapsed
            // from the total life-time.
            double unRounded = lifeTime - (Environment.TickCount - timeCreated) / (1.0 * MS_IN_MINUTE);
            // Round the remaining life-time to 1 decimal place, unless there is less than 0.1 minutes
            // in which case round to a higher-precision double (3 decimal places). Return this value.
            return unRounded >= 0.1? Math.Round(unRounded, 1) : Math.Round(unRounded, 3);
        }
        
        /// <summary>
        /// Updates the recipient dictionary to reflect that user has accepted or rejected the invitation.
        /// Precondition: "user" is a recipient of this invitation.
        /// </summary>
        /// <param name="user">A recipient of this invitation whose acceptance state should be updated.</param>
        /// <param name="status">The new status of the invitation for "user".</param>
        public void UpdateStatus(Person user, InvitationStatus status)
        {
            // Update the status value at the "user" key in the dictionary
            recipientStates[user] = status;
        }

        /// <summary>
        /// Get the recipient list for this invitation by shallow-copying.
        /// </summary>
        /// <returns>A copy of a list of all recipient Person objects for this invitation.</returns>
        public List<Person> GetAllRecipients()
        {
            // Return a new list of the keys of the recipient states - the Person objects
            return new List<Person>(this.recipientStates.Keys);
        }

        
        /// <summary>
        /// Builds and returns a string representation of the invitation content.
        /// </summary>
        /// <param name="user">The non-null Person object (who must be the creator or recipient of this invitation)
        /// accessing the invitation description.</param>
        /// <returns>A string representation of the invitation.</returns>
        public string ToString(Person user)
        {
            // String builder representation of the invitation description
            // Used for efficiency when appending new information for each invitation recipient
            // Initialize the string builder with the invitation's interest information
            StringBuilder invitationSB = new StringBuilder($"Interest: { Interest }\r\n");

            // Add creator information if the user is not the creator
            if(user != Creator)
            {
                // Add the formatted creator information
                invitationSB.Append($"Creator: { Creator.Username }\r\n");
            }

            // Add recipient and status information
            invitationSB.Append("Recipients:\r\n");

            // Will store the invitation status for each recipient
            string status = "";

            // Iterate through every (recipient, status) pair in the recipientStates dictionary in order to
            // add all recipients to the invitation description
            foreach(KeyValuePair<Person, InvitationStatus> recipient in recipientStates)
            {
                // Format the InvitationStatus value in a user-friendly manner stored in the "status" string
                switch (recipient.Value)
                {
                    case InvitationStatus.Accepted:
                        status = "Accepted";
                        break;
                    case InvitationStatus.Rejected:
                        status = "Rejected";
                        break;
                    case InvitationStatus.Pending:
                        status = "Pending";
                        break;
                }
                
                // Add the recipient username and their invitation status to the description
                invitationSB.Append($"{ recipient.Key.Username }- { status }, ");
            }

            // Remove trailing comma and space while converting the string builder into a string
            string invitationString = invitationSB.ToString(0, invitationSB.Length - 2);

            // Add information about time remaining for the invitation to the description
            invitationString += $"\r\n{ this.TimeLeft() } minutes left";
            
            // Return the string
            return invitationString;
        }

        /// <summary>
        /// Marks this invitation as inactive. It will be deleted from the creator and all recipients
        /// once the network updates its invitation information.
        /// </summary>
        public void Deactivate()
        {
            active = false;
        }

        /// <summary>
        /// Returns the acceptance state of the invitation for a given recipient.
        /// Precondition: "recipient" must be a recipient of this invitation. 
        /// </summary>
        /// <param name="recipient">A recipient of this invitation.</param>
        /// <returns>InvitationStatus of Accepted, Pending, or Rejected depending on the invitation state
        /// for "recipient".</returns>
        public InvitationStatus InvitationStateOfRecipient(Person recipient)
        {
            // Return the acceptance value at the recipient's key
            return recipientStates[recipient];
        }
    }
}
