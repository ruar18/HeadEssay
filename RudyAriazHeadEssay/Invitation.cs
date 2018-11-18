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
        private string interest;
        // The Person object that created the invitation
        private Person creator;
        // The people that the invitation was sent to 
        private List<Person> recipients;

        public Invitation() { }

    }
}
