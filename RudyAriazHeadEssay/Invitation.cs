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
        // time created and lifespan of the invitation
        private int lifeTime, timeCreated;
        // Store the creator's advertised interest
        public string Interest { get; }
        // The Person object that created the invitation
        public Person Creator { get; }
        // The people that the invitation was sent to 
        private List<Person> recipients;
        // The number of milliseconds a minute
        private const int MS_IN_MINUTE = 60000;

        public Invitation(Person creator, List<Person> recipients, 
                          string interest, int timeCreated, int lifeTime )
        {
            Interest = "test";
        }

        // Checks if the invitation is still active, accurate to the milisecond
        public bool IsActive()
        {
            return Environment.TickCount - timeCreated <= lifeTime * MS_IN_MINUTE;
        }


        // Gets the remaining lifetime of the invitation in minutes
        public int TimeLeft()
        {
            return lifeTime - (Environment.TickCount - timeCreated) / MS_IN_MINUTE;
        }

        public List<Person> GetAllRecipients()
        {
            return Copier.CopyList(recipients);
        }


    }
}
