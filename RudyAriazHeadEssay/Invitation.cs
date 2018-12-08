using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RudyAriazHeadEssay
{
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
        // The people that the invitation was sent to 
        private List<Person> recipients;
        // The number of milliseconds in a minute
        private const int MS_IN_MINUTE = 60000;

        // Creates an invitation object
        public Invitation(Person creator, List<Person> recipients, 
                          string interest, int timeCreated, double lifeTime )
        {
            Creator = creator;
            this.recipients = recipients;
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

        // Shallow copy the recipient list 
        public List<Person> GetAllRecipients()
        {
            return Copier.CopyList(recipients);
        }


    }
}
