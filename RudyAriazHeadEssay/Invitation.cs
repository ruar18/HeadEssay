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
        // Store time information for the invitation (in minutes):
        // Lifespan of the invitation
        private double lifeTime;
        // Time created
        private int timeCreated;
        // Store the creator's advertised interest
        public string Interest { get; }
        // The Person object that created the invitation
        public Person Creator { get; }
        // The people that the invitation was sent to, along with whether or not they accepted the invitation
        private Dictionary<Person, InvitationStatus> recipientStates;
        // The number of milliseconds in a minute
        private const int MS_IN_MINUTE = 60000;

        // Creates an invitation object
        public Invitation(Person creator, Dictionary<Person, InvitationStatus> recipients, 
                          string interest, int timeCreated, double lifeTime )
        {
            Creator = creator;
            this.recipientStates = recipients;
            Interest = interest;
            this.timeCreated = timeCreated;
            this.lifeTime = lifeTime;
        }

        // Checks if the invitation is still active
        public bool IsActive()
        {
            return Environment.TickCount - timeCreated <= lifeTime * MS_IN_MINUTE;
        }


        // Gets the remaining lifetime of the invitation in minutes
        public double TimeLeft()
        {
            return lifeTime - (Environment.TickCount - timeCreated) / (1.0 * MS_IN_MINUTE);
        }

        // Updates the recipients to reflect that user has accepted or rejected the invitation
        // TODO: check if error checking is needed
        public void UpdateStatus(Person user, InvitationStatus status)
        {
            recipientStates[user] = status;
        }

        // Shallow copy the recipient list 
        public List<Person> GetAllRecipients()
        {
            // Return a new list of the keys of the recipient states (the Person objects)
            return new List<Person>(this.recipientStates.Keys);
        }


        // String representation of the invitation for a given user
        // user must be creator or recipient of invitation
        // TODO: use stringbuilder?
        public string ToString(Person user)
        {
            string invitation = $"Interest: { Interest }\n";
            // Add creator information if the user is not the creator
            if(user != Creator)
            {
                invitation += $"Creator: { Creator.Username }\n";
            }
            // Add recipient and status information
            invitation += "Recipients: ";
            foreach(KeyValuePair<Person, InvitationStatus> recipient in recipientStates)
            {
                // TODO: check if status is printed nicely
                invitation += $"{ recipient.Key.Username }: { recipient.Value }, ";
            }
            // Remove trailing comma and space
            invitation = invitation.Substring(invitation.Length - 2);

            // Add time information
            invitation += $"{ this.TimeLeft() } minutes left";
            
            // Return the string
            return invitation;
        }
    }
}
