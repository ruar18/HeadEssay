/*
 * Rudy Ariaz
 * December 14, 2018
 * The Person classes encapsulates information about each user in the HeadEssay network.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RudyAriazHeadEssay
{
    public class Person
    {
        /// <summary>Gets and sets the user's first name.</summary>
        public string FirstName { get; set; }
        /// <summary>Gets and sets the user's last name.</summary>
        public string LastName { get; set; }
        /// <summary>Gets and sets the user's username.</summary>
        public string Username { get; set; }
        /// <summary>Gets and sets the user's password.</summary>
        public string Password { get; set; }
        /// <summary>Gets and sets the user's city.</summary>
        /// <value>A string representation of the user's city.</value>
        public string City { get; set; }
        // Store interests (as strings) for the person
        private List<string> interests;
        // Store the person's friends as Person objects
        private List<Person> friends;
        // Store the person's friends of friends as Person objects
        private List<Person> friendsOfFriends;
        // Store the person's friends of friends with the same interest as Person objects
        private List<Person> friendsOfFriendsSameInterest;
        // Store people in the same city as the person as Person objects
        private List<Person> sameCity;
        // Store people in the same city with the same interest as Person objects
        private List<Person> sameCitySameInterest;
        // Store all incoming invitations as Invitation objects
        private List<Invitation> incomingInvitations;
        // Store the person's incoming accepted invitations as Invitation objects
        private List<Invitation> acceptedInvitations;
        // Store the person's outgoing invitations as Invitation objects
        private List<Invitation> outgoingInvitations;


        /// <summary>
        /// Constructs a new person object.
        /// Precondition: all parameters are non-null and non-empty.
        /// </summary>
        /// <param name="firstName">The person's first name.</param>
        /// <param name="lastName">The person's last name.</param>
        /// <param name="city">The person's city.</param>
        /// <param name="username">The person's user name.</param>
        /// <param name="password">The perons's password.</param>
        public Person(string firstName, string lastName, string city,
                      string username, string password)
        {
            // Set all appropriate fields (as properties) to the arguments
            FirstName = firstName;
            LastName = lastName;
            City = city;
            Username = username;
            Password = password;

            // Instantiate all lists used by the Person class (that are not directly provided by the Network class)
            interests = new List<string>();
            friends = new List<Person>();
            incomingInvitations = new List<Invitation>();
            acceptedInvitations = new List<Invitation>();
            outgoingInvitations = new List<Invitation>();
        }


        /// <summary>
        /// Gets all of this user's friends in a list by shallow-copying.
        /// </summary>
        /// <returns>A list containing all of the user's unique friends. 
        /// If "friends" is null, returns an empty list.</returns>
        public List<Person> GetAllFriends()
        {
            // Shallow copy and return the friends list
            return Copier.CopyList(friends);
        }

        /// <summary>
        /// Gets all of this user's friends of friends in a list by shallow-copying.
        /// </summary>
        /// <returns>A list containing all of the user's unique friends of friends. 
        /// If "friendsOfFriends" is null, returns an empty list.</returns>
        public List<Person> GetFriendsOfFriends()
        {
            // Shallow copy and return the friends of friends list
            return Copier.CopyList(friendsOfFriends);
        }

        /// <summary>
        /// Gets all of this user's friends of friends with the same interest in a list by shallow-copying.
        /// </summary>
        /// <returns>A list containing all of the user's unique friends of friends with the same interests.
        /// If "friendsOfFriendsSameInterest" is null, returns an empty list.</returns>
        public List<Person> GetFriendsOfFriendsSameInterest()
        {
            // Shallow copy and return the friends of friends with same interest list
            return Copier.CopyList(friendsOfFriendsSameInterest);
        }

        /// <summary>
        /// Gets up to 10 of this user's non-friends in the same city in a list by shallow-copying.
        /// </summary>
        /// <returns>A list containing up to 10 of the user's unique non-friends in the same city.
        /// If "sameCity" is null, returns an empty list.</returns>
        public List<Person> GetSameCity()
        {
            // Shallow copy and return the list of non-friends in the same city
            return Copier.CopyList(sameCity);
        }

        /// <summary>
        /// Gets up to 10 of this user's non-friends in the same city with a shared interest in a list by shallow-copying.
        /// </summary>
        /// <returns>A list containing up to 10 of the user's unique non-friends in the same city 
        /// who share at least one interest with the user. If "sameCitySameInterest" is null, returns an empty list.</returns>
        public List<Person> GetSameCitySameInterest()
        {
            // Shallow copy and return the list of non-friends in the same city with a shared interest
            return Copier.CopyList(sameCitySameInterest);
        }

        // TODO: do you need to shallow copy?
        /// <summary>
        /// Gets a copy of a list of the user's interests by shallow-copying.
        /// </summary>
        /// <returns>A list containing all of the user's interests as strings.</returns>
        public List<string> GetAllInterests()
        {
            // Shallow copy and return the list of interests
            return Copier.CopyList(interests);
        }

        /// <summary>
        /// Adds a given friend to the user's friends list. Note: one-directional friending only.
        /// </summary>
        /// <param name="friend">A non-null Person to be added as a friend.</param>
        public void AddFriend(Person friend)
        {
            // Add the new friend to the friends list
            friends.Add(friend);
        }

        /// <summary>
        /// Removes a given friend from the user's friends list. Note: one-directional unfriending only.
        /// </summary>
        /// <param name="friend">A non-null Person to be unfriended.</param>
        public void RemoveFriend(Person friend)
        {
            // Remove the friend from the friends list
            friends.Remove(friend);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="invitation"></param>
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
        public void DeleteIncomingInvitation(Invitation toDelete)
        {
            // Remove the invitation
            incomingInvitations.Remove(toDelete);
        }


        // Delete a pending incoming or accepted invitation given the index
        public void DeleteIncomingInvitation(int deletedIndex)
        {
            // Remove the invitation
            incomingInvitations.RemoveAt(deletedIndex);
        }


        // Delete an outgoing invitation given the invitation
        public void DeleteOutgoingInvitation(Invitation toDelete)
        {
            // Remove the invitation
            outgoingInvitations.Remove(toDelete);
        }

        // Delete an outgoing invitation given the index of the invitation
        public void DeleteOutgoingInvitation(int deletedIndex)
        {
            // Remove the invitation
            outgoingInvitations.RemoveAt(deletedIndex);
        }

        // Completely delete invitation from the user (whether incoming or outgoing)
        public void DeleteInvitation(Invitation toDelete)
        {
            acceptedInvitations.Remove(toDelete);
            incomingInvitations.Remove(toDelete);
            outgoingInvitations.Remove(toDelete);
        }

        // Adds an outgoing invitation to the list of outgoing invitations
        // TODO: rename params?
        public void AddOutgoingInvitation(Invitation toAdd)
        {
            outgoingInvitations.Add(toAdd);
        }

        // Return true if other is in the person's friend list, false otherwise
        public bool IsFriend(Person other) { return friends.Contains(other); }

        public void AddInterest(string interest)
        {
            interests.Add(interest);
        }
        
        /// <summary>
        /// Removes a given interest from the interests list.
        /// Precondition: "interests" is a list with unique interests only.
        /// </summary>
        /// <param name="interest">The interest to remove.</param>
        public void RemoveInterest(string interest)
        {
            // Remove the interest
            interests.Remove(interest);
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
