using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RudyAriazHeadEssay
{
    public class Person
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
        // Store the person's friends-of-friends
        private List<Person> friendsOfFriends;
        // Store the person's incoming pending invitations
        private List<Invitation> incomingInvitations;
        // Store the person's outgoing invitations
        private List<Invitation> outgoingInvitations;


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
            // Initialize lists
            interests = new List<string>();
            friends = new List<Person>();
            friendsOfFriends = new List<Person>();
            incomingInvitations = new List<Invitation>();
            outgoingInvitations = new List<Invitation>();
        }
        
      
        // Do a shallow copy of the friends 
        public List<Person> GetAllFriends()
        {
            return Copier.CopyList(friends);
        }

        public List<string> GetAllInterests()
        {
            return Copier.CopyList(interests);
        }

        // Adds the friend's friends to the person's friends-of-friends
        // Only if not already in the person's friend list
        // TODO: optimize with hashset?
        private void AddFriendsOfFriend(Person friend)
        {
            foreach(Person friendOfFriend in friend.friends)
            {
                if (!this.friends.Contains(friendOfFriend))
                {
                    this.friendsOfFriends.Add(friendOfFriend);
                }
            } 
        }

        // Friend adds are not necessarily mutual
        public void AddFriend(Person friend)
        {
            friends.Add(friend);
            AddFriendsOfFriend(friend);
        }

        // TODO: Optimize
        private void RemoveFriendsOfFriend()
        {
            foreach(Person friendOfFriend in this.friendsOfFriends)
            {
               
            }
        }

        public void RemoveFriend(Person friend)
        {
            friends.Remove(friend);
        }

        // TODO: need invitation parameter?
        public void SendInvitation(Person receiver, Invitation invitation)
        {
            receiver.incomingInvitations.Add(invitation);
        }

        // Allows a user to receive an invitation
        public void AcceptInvitation(Invitation invitation)
        {
            incomingInvitations.Add(invitation);
        }

        // Can delete a sent or received invitation
        public void DeleteInvitation(Invitation toDelete) { }

        public bool IsFriend(Person other) { return true; }

        public void AddInterest(string interest) { }
        
    }
}
