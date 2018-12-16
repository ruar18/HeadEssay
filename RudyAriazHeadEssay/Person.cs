/*
 * Rudy Ariaz
 * December 16, 2018
 * The Person classes encapsulates information about each user in the HeadEssay network, including information
 * about their name, login credentials, location, interests, friends, recommendations, and invitations.
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
        /// <returns>A list containing all of the user's unique friends.</returns>
        public List<Person> GetAllFriends()
        {
            // Shallow copy and return the friends list
            return Copier.CopyList(friends);
        }

        /// <summary>
        /// Gets all of this user's friends of friends in a list by shallow-copying.
        /// </summary>
        /// <returns>A list containing all of the user's unique friends of friends.</returns>
        public List<Person> GetFriendsOfFriends()
        {
            // Shallow copy and return the friends of friends list
            return Copier.CopyList(friendsOfFriends);
        }

        /// <summary>
        /// Gets all of this user's friends of friends with the same interest in a list by shallow-copying.
        /// </summary>
        /// <returns>A list containing all of the user's unique friends of friends with the same interests.</returns>
        public List<Person> GetFriendsOfFriendsSameInterest()
        {
            // Shallow copy and return the friends of friends with same interest list
            return Copier.CopyList(friendsOfFriendsSameInterest);
        }

        /// <summary>
        /// Gets up to 10 of this user's non-friends in the same city in a list by shallow-copying.
        /// </summary>
        /// <returns>A list containing up to 10 of the user's unique non-friends in the same city.</returns>
        public List<Person> GetSameCity()
        {
            // Shallow copy and return the list of non-friends in the same city
            return Copier.CopyList(sameCity);
        }

        /// <summary>
        /// Gets up to 10 of this user's non-friends in the same city with a shared interest in a list by shallow-copying.
        /// </summary>
        /// <returns>A list containing up to 10 of the user's unique non-friends in the same city who share
        /// at least one interest with the user.</returns>
        public List<Person> GetSameCitySameInterest()
        {
            // Shallow copy and return the list of non-friends in the same city with a shared interest
            return Copier.CopyList(sameCitySameInterest);
        }
        
        /// <summary>
        /// Gets a copy of a list of the user's interests by shallow-copying.
        /// </summary>
        /// <returns>A list containing all of the user's unique interests as strings.</returns>
        public List<string> GetAllInterests()
        {
            // Shallow copy and return the list of interests
            return Copier.CopyList(interests);
        }

        /// <summary>
        /// Adds a given friend to the user's friends list. 
        /// </summary>
        /// <remarks>
        /// One-directional friending only.
        /// </remarks>
        /// <param name="friend">A non-null Person to be added as a friend.</param>
        public void AddFriend(Person friend)
        {
            // Add the new friend to the friends list
            friends.Add(friend);
        }

        /// <summary>
        /// Removes a given friend from the user's friends list.
        /// </summary>
        /// <remarks>
        /// One-directional unfriending only.
        /// </remarks>
        /// <param name="friend">A non-null Person to be unfriended.</param>
        public void RemoveFriend(Person friend)
        {
            // Remove the friend from the friends list
            friends.Remove(friend);
        }

        /// <summary>
        /// Accept an incoming invitation by adding it to "acceptedInvitations" and marking it as accepted
        /// from the perspective of this Person.
        /// </summary>
        /// <param name="invitation">Non-null incoming invitation to accept.</param>
        public void AcceptInvitation(Invitation invitation)
        {
            // Add the invitation to the accepted list
            acceptedInvitations.Add(invitation);
            // Modify the invitation acceptance state
            invitation.UpdateStatus(this, InvitationStatus.Accepted);
        }
        
        /// <summary>
        /// Receives an incoming invitation into the person's pending list. 
        /// </summary>
        /// <param name="invitation">Non-null invitation to receive.</param>
        public void ReceiveInvitation(Invitation invitation)
        {
            // Add the invitation to the received list
            incomingInvitations.Add(invitation);
        }

        /// <summary>
        /// Deletes an incoming invitation, whether it is accepted or pending. Marks the invitation
        /// as rejected by this Person.
        /// </summary>
        /// <param name="invitation">The non-null incoming invitation to delete.</param>
        public void DeleteIncomingInvitation(Invitation invitation)
        {
            // Remove the invitation (from both list if it has been accepted, otherwise it is only removed
            // from the incoming invitations list)
            incomingInvitations.Remove(invitation);
            acceptedInvitations.Remove(invitation);
            // Change the acceptance state of the invitation to "rejected"
            invitation.UpdateStatus(this, InvitationStatus.Rejected);
        }
        
        /// <summary>
        /// Deletes an outgoing invitation from this user's outgoing invitation list.
        /// </summary>
        /// <remarks>
        /// When this is called by the Network class, the network completely removes the invitation from the network.
        /// </remarks>
        /// <param name="invitation">The non-null outgoing invitation to delete.</param>
        public void DeleteOutgoingInvitation(Invitation invitation)
        {
            // Remove the invitation
            outgoingInvitations.Remove(invitation);
        }
        
        /// <summary>
        /// Adds an outgoing invitation to this Person's outgoing invitations list.
        /// </summary>
        /// <param name="invitation">The non-null outgoing invitation to add.</param>
        public void AddOutgoingInvitation(Invitation invitation)
        {
            // Add the invitation to the list
            outgoingInvitations.Add(invitation);
        }

        /// <summary>
        /// Checks if a given user is a friend of this Person.
        /// </summary>
        /// <remarks>
        /// This method need not be commutative. It only checks if this Person has friended "other", not vice-versa.
        /// </remarks>
        /// <param name="other">The Person to check for being the user's friend.</param>
        /// <returns>True if "other" is a friend of this Person, false otherwise.</returns>
        public bool IsFriend(Person other)
        {
            // Check if "other" is in the user's friends list
            return friends.Contains(other);
        }

        /// <summary>
        /// Adds an interest to the user's interest list.
        /// Precondition: "interest" is not currently in the interests list.
        /// </summary>
        /// <param name="interest">The non-duplicate interest to add to the interests list.</param>
        public void AddInterest(string interest)
        {
            // Add the interest to the interests list 
            interests.Add(interest);
        }
        
        /// <summary>
        /// Removes a given interest from the interests list.
        /// Precondition: "interests" is a list with unique interests only.
        /// </summary>
        /// <param name="interest">The interest to remove.</param>
        public void RemoveInterest(string interest)
        {
            // Remove the interest from the interests list
            interests.Remove(interest);
        }
        
        /// <summary>
        /// Sets the friends of friends list to an updated version.
        /// </summary>
        /// <remarks>
        /// No data validation occurs. 
        /// </remarks>
        public List<Person> FriendsOfFriends
        {
            // Set the friends of friends list to the new one provided
            set
            {
                friendsOfFriends = value;
            }
        }

        /// <summary>
        /// Sets the friends of friends (with same interest) list to an updated version.
        /// </summary>
        /// <remarks>
        /// No data validation occurs. 
        /// </remarks>
        public List<Person> FriendsOfFriendsSameInterest
        {
            // Set the friends of friends (with same interest) list to the new one provided
            set
            {
                friendsOfFriendsSameInterest = value;
            }
        }

        /// <summary>
        /// Sets the list of users in the same city to an updated version.
        /// </summary>
        public List<Person> SameCity
        {
            // Set the list of users in the same city to the new one provided
            set
            {
                sameCity = value;
            }
        }
        
        /// <summary>
        /// Sets the list of users in the same city with a shared interest to an updated version.
        /// </summary>
        public List<Person> SameCitySameInterest
        {
            // Set the list of users in the same city with a shared interest to the new one provided
            set
            {
                sameCitySameInterest = value;
            }
        }

        /// <summary>
        /// Gets this Person's outgoing invitations by shallow-copying.
        /// </summary>
        /// <returns>A list containing all of the user's outgoing invitations.</returns>
        public List<Invitation> GetOutgoingInvitations()
        {
            // Shallow copy and return the list of outgoing invitations 
            return Copier.CopyList(outgoingInvitations);
        }

        /// <summary>
        /// Gets this Person's incoming invitations by shallow-copying.
        /// </summary>
        /// <returns>A list containing all of the user's incoming invitations.</returns>
        public List<Invitation> GetIncomingInvitations()
        {
            // Shallow copy and return the list of incoming invitations 
            return Copier.CopyList(incomingInvitations);
        }

        /// <summary>
        /// Gets this Person's incoming accepted invitations by shallow-copying.
        /// </summary>
        /// <returns>A list containing all of the user's incoming accepted invitations.</returns>
        public List<Invitation> GetAcceptedInvitations()
        {
            // Shallow copy and return the list of accepted incoming invitations
            return Copier.CopyList(acceptedInvitations);
        }
    }
}
