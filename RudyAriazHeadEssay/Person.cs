using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RudyAriazHeadEssay
{
    class Person
    {
        // Store name information for the person
        public string FirstName { get; set; }
        public string LastName { get; set; }
        // Store login information for the person
        public string UserName { get; set; }
        public string Password { get; set; }
        // Store location information for the person
        public string City { get; set; }
        // Store interests for the person
        private List<string> interests;
        // Store the person's friends
        private List<Person> friends;
        // Store the person's incoming pending invitations
        private List<Invitation> incomingInvitations;


        /// <summary>
        /// Create a person object. Null checks are not done here. 
        /// </summary>
        /// <param name="firstName">The person's first name.</param>
        /// <param name="lastName">The person's last name.</param>
        /// <param name="userName">The person's user name.</param>
        /// <param name="password">The perons's password.</param>
        public Person(string firstName, string lastName, string city, 
                      string userName, string password)
        {
            FirstName = firstName;
            LastName = lastName;
            City = city;
            UserName = userName;
            Password = password;
        }

        public void AddFriend(Person friend) { }

        public void RemoveFriend(Person friend) { }

        // TODO: need invitation parameter?
        public void SendInvitation(Person receiver, Invitation invitation)
        {
            
        }

        // Allows a user to receive an invitation
        public void AcceptInvitation(Invitation invitation)
        {
            incomingInvitations.Add(invitation);
        }

        public void DeleteInvitation(Invitation toDelete) { }

        public bool IsFriend(Person other) { return true; }
    }
}
