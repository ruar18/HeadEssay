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


        // Gets the remaining lifetime of the invitation in minutes (rounded to 1 decimal place)
        public double TimeLeft()
        {
            double unRounded = lifeTime - (Environment.TickCount - timeCreated) / (1.0 * MS_IN_MINUTE);
            // Round the remaining lifetime
            return Math.Round(unRounded, 1);
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
            string invitation = $"Interest: { Interest }\r\n";
            // Add creator information if the user is not the creator
            if(user != Creator)
            {
                invitation += $"Creator: { Creator.Username }\r\nn";
            }
            // Add recipient and status information
            invitation += "Recipients:\r\n";

            // Will store the invitation status for each recipient
            string status = "";
            foreach(KeyValuePair<Person, InvitationStatus> recipient in recipientStates)
            {
                // Format the status
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
                
                invitation += $"{ recipient.Key.Username }- { status }, ";
            }
            // Remove trailing comma and space
            invitation = invitation.Substring(0, invitation.Length - 2);

            // Add time information
            invitation += $"\r\n{ this.TimeLeft() } minutes left";
            
            // Return the string
            return invitation;
        }
    }
}
