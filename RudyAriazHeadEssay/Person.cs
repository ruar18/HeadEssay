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
        public string Username { get; set; }
        public string Password { get; set; }
        // Store location information for the person
        public string City { get; set; }
        // Store interests for the person
        private List<string> interests;
        // Store the person's friends
        private List<Person> friends;
        // Store the person's friends of friends
        private List<Person> friendsOfFriends;
        // Store the person's friends of friends with the same interest
        private List<Person> friendsOfFriendsSameInterest;
        // People in the same city as the person
        private List<Person> sameCity;
        // People in the same city with the same interest
        private List<Person> sameCitySameInterest;
        // Store all incoming invitations
        private List<Invitation> incomingInvitations;
        // Store the person's incoming accepted invitations
        private List<Invitation> acceptedInvitations;
        // Store the person's outgoing invitations
        private List<Invitation> outgoingInvitations;


        /// <summary>
        /// Create a person object. Null checks are not done here. 
        /// </summary>
        /// <param name="firstName">The person's first name.</param>
        /// <param name="lastName">The person's last name.</param>
        /// <param name="username">The person's user name.</param>
        /// <param name="password">The perons's password.</param>
        public Person(string firstName, string lastName, string city,
                      string username, string password)
        {
            FirstName = firstName;
            LastName = lastName;
            City = city;
            Username = username;
            Password = password;
            // Initialize lists
            interests = new List<string>();
            friends = new List<Person>();
            friendsOfFriends = new List<Person>();
            incomingInvitations = new List<Invitation>();
            acceptedInvitations = new List<Invitation>();
            outgoingInvitations = new List<Invitation>();
        }


        // Do a shallow copy of the friends 
        public List<Person> GetAllFriends()
        {
            return Copier.CopyList(friends);
        }

        
        // Do a shallow copy of friends of friends
        public List<Person> GetFriendsOfFriends()
        {
            return Copier.CopyList(friendsOfFriends);
        }

        // Do a shalow copy of friends of friends with same interest
        public List<Person> GetFriendsOfFriendsSameInterest()
        {
            return Copier.CopyList(friendsOfFriendsSameInterest);
        }

        // Do a shallow copy of non-friends in same city
        public List<Person> GetSameCity() { return Copier.CopyList(sameCity); }

        // Do a shallow copy of non-friends in same city with shared interest
        public List<Person> GetSameCitySameInterest()
        {
            return Copier.CopyList(sameCitySameInterest);
        }

        public List<string> GetAllInterests()
        {
            return Copier.CopyList(interests);
        }
        

        // Friend adds are not necessarily mutual
        public void AddFriend(Person friend)
        {
            friends.Add(friend);
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
            acceptedInvitations.Add(invitation);
            // Modify the invitation information
            invitation.UpdateStatus(this, InvitationStatus.Accepted);
        }

        // Receive an incoming invitation into the person's pending list 
        public void ReceiveInvitation(Invitation toReceive)
        {
            incomingInvitations.Add(toReceive);
        }

        // Can delete a pending incoming or accepted invitation
        public void DeleteIncomingInvitation(Invitation toDelete) { }

        // Can delete an outgoing invitation
        public void DeleteOutgoingInvitation(Invitation toDelete) { }

        // Adds an outgoing invitation to the list of outgoing invitations
        public void AddOutgoingInvitation(Invitation toAdd) { }

        // Return true if other is in the person's friend list, false otherwise
        public bool IsFriend(Person other) { return true; }

        public void AddInterest(string interest)
        {
            interests.Add(interest);
        }

        public void SetFriendsOfFriends(List<Person> people) { friendsOfFriends = people; }
        public void SetFriendsOfFriendsSameInterest(List<Person> people)
        {
            friendsOfFriendsSameInterest = people;
        }
        public void SetSameCity(List<Person> people)
        {
            sameCity = people;
        }
        public void SetSameCitySameInterest(List<Person> people)
        {
            sameCitySameInterest = people;
        }

        public List<Invitation> GetOutgoingInvitations()
        {
            return Copier.CopyList(outgoingInvitations);
        }
        public List<Invitation> GetIncomingInvitations()
        {
            return Copier.CopyList(incomingInvitations);
        }
        public List<Invitation> GetAcceptedInvitations()
        {
            return Copier.CopyList(acceptedInvitations);
        }
        

    }
}
