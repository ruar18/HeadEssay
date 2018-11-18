using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RudyAriazHeadEssay
{
    // TODO: why is this public? 
    public class Network
    {
        // Store all the users on the social network
        private List<Person> users;

        public Network() { }

        /// <summary>
        /// Delete all invitations which have exceeded their lifespan.
        /// </summary>
        public void DeleteInactiveInvitations() { }

        public bool IsUserInNetwork(string userName, string password)
        {
            return true;
        }

        // TODO: check if there are restrictions on users size 
        // TODO: catch exception
        public void AddNewUser(Person user)
        {
            if(user == null)
            {
                throw new System.ArgumentNullException("Parameter cannot be null", "user");
            }
            else
            {
                users.Add(user);
            }
           
        }
    }
}
