/*
 * Rudy Ariaz
 * December 16, 2018
 * A Network object manages HeadEssay's social network of users by finding friend recommendations,
 * interfacing with the Person objects, and abstracting the network's operations from the Main UI.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RudyAriazHeadEssay
{
    public class Network
    {
        // Stores all the users on the social network
        private List<Person> users;
        // Stores all the invitations on a social network 
        private List<Invitation> invitations;

        /// <summary>
        /// Constructs a new Network object.
        /// </summary>
        public Network()
        {
            // Instantiate the lists of users and invitations 
            users = new List<Person>();
            invitations = new List<Invitation>();
            // TODO: remove temporary people
            users.Add(new Person("Rudy", "Ariaz", "Toronto", "rariaz", "hi"));
            users.Add(new Person("Willie", "stuff", "Toronto", "wchan", "hi"));
            users.Add(new Person("Tiff", "T", "Toronto", "ttruong", "hi"));
            users.Add(new Person("Henning", "L", "munich", "lhenning", "hi"));
            users.Add(new Person("whoo", "hey", "munich", "hwhoo", "hi"));
            users[0].AddFriend(users[1]);
            users[0].AddFriend(users[2]);
            users[1].AddFriend(users[3]);
            users[0].AddInterest("swimming");
            users[3].AddInterest("swimming");
            for(int i = 0; i < 20; i++)
            {
                users.Add(new Person("test", "testtt", "Toronto", i + "test", "hi"));
            }
        }

        /// <summary>
        /// Adds a new user to the network.
        /// </summary>
        /// <param name="user">A non-null Person to be added.</param>
        public void AddNewUser(Person user)
        {
            // Add the user to the users list 
            users.Add(user);
        }

        /// <summary>
        /// Deletes all invitations which have exceeded their lifespan or been manually deactivated.
        /// </summary>
        public void DeleteInactiveInvitations()
        {
            // Iterate through all of the invitations in the network
            foreach(Invitation sent in invitations)
            {
                // Check if the current invitation is inactive
                if (!sent.IsActive())
                {
                    // Delete the inactive invitation from the creator's outgoing invitations list
                    sent.Creator.DeleteOutgoingInvitation(sent);
                    // Iterate through all of the recipients of the invitation
                    foreach(Person recipient in sent.GetAllRecipients())
                    {
                        // Delete the inactive invitation from the recipients' incoming invitations list
                        recipient.DeleteIncomingInvitation(sent);
                    }
                }
            }
            // Completely remove all of the inactive invitations from the network 
            invitations.RemoveAll(invitation => !invitation.IsActive());
        }
        
        /// <summary>
        /// Adds a new sent invitation to the network and distributes it to the involved Person objects.
        /// </summary>
        /// <param name="invitation">A non-null invitation to be delivered.</param>
        public void DeliverInvitation(Invitation invitation)
        {
            // Record the invitation in the list of all invitations
            invitations.Add(invitation);
            // Add the invitation to the creator's list of outgoing invitations
            invitation.Creator.AddOutgoingInvitation(invitation);
            // Iterate through all recipients of the invitation
            foreach (Person recipient in invitation.GetAllRecipients())
            {
                // Add the invitation to the recipients' incoming invitations list 
                recipient.ReceiveInvitation(invitation);
            }
        }

        /// <summary>
        /// Constructs a list of a user's unique friends of friends.
        /// </summary>
        /// <remarks>
        /// Friends of friends cannot be a user's direct friends.
        /// </remarks>
        /// <param name="user">The Person for whom the friends of friends must be updated.</param>
        /// <returns>A list of unique friends of friends of the user.</returns>
        private List<Person> FindFriendsOfFriends(Person user)
        {
            // Instantiate a new list to store the friends of friends 
            List<Person> friendsOfFriends = new List<Person>();

            // Iterate through all of the user's friends
            foreach (Person friend in user.Friends)
            {
                // Iterate through this friend's friends (the user's friends of friends)
                foreach (Person friendOfFriend in friend.Friends)
                {
                    // Check if the friend of friend is not a friend and is not the user itself
                    if (user != friendOfFriend && !user.IsFriend(friendOfFriend))
                    {
                        // Add the friend of friend to the list of friends of friends
                        friendsOfFriends.Add(friendOfFriend);
                    }
                }
            }
            // Remove duplicates from the list 
            return friendsOfFriends.Distinct().ToList();
        }

        /// <summary>
        /// Updates a user's list of unique friends of friends.
        /// </summary>
        /// <remarks>
        /// Friends of friends cannot be a user's direct friends.
        /// </remarks>
        /// <param name="user">The Person for whom the friends of friends must be updated.</param>
        public void UpdateFriendsOfFriends(Person user)
        {
            // Update the user's friends of friends 
            user.FriendsOfFriends = FindFriendsOfFriends(user);
        }

        /// <summary>
        /// Constructs a list of a user's unique friends of friends that share at least one interest with the user..
        /// </summary>
        /// <remarks>
        /// Friends of friends cannot be a user's direct friends.
        /// </remarks>
        /// <param name="user">The Person for whom the friends of friends with a shared interest must be found.</param>
        /// <returns>A list of the user's unique friends of friends with a shared interest.</returns>
        private List<Person> FindFriendsOfFriendsSameInterest(Person user)
        {
            // Find the user's friends of friends 
            List<Person> validFriendsOfFriends = FindFriendsOfFriends(user);
            // Filter the user's friends of friends to keep only those that share an interest with the user
            validFriendsOfFriends.RemoveAll(person => !ShareSameInterest(person, user));
            // Return the list of valid friends of friends
            return validFriendsOfFriends;
        }

        /// <summary>
        /// Updates a user's list of unique friends of friends that share at least one interest with the user.
        /// </summary>
        /// <remarks>
        /// Friends of friends cannot be a user's direct friends.
        /// </remarks>
        /// <param name="user">The Person for whom the friends of friends with a shared interest must be updated.</param>
        public void UpdateFriendsOfFriendsSameInterest(Person user)
        {
            // Update the user's friends of friends with same interest 
            user.FriendsOfFriendsSameInterest = FindFriendsOfFriendsSameInterest(user);
        }

        /// <summary>
        /// Constructs a list of all of the unique users (who are not the user's friends) in a user's city.
        /// </summary>
        /// <param name="user">The user for whom the list of non-friends in the same city must be found.</param>
        /// <returns>A list of all of the unique non-friends in a user's city</returns>
        private List<Person> FindSameCity(Person user)
        {
            // Instantiate the list that will store individuals in the same city
            List<Person> sameCity = new List<Person>();
            // Iterate through all users in the network
            foreach (Person currentPerson in users)
            {
                // If the current person is not the user's friend, is not the user itself, and is in the same city,
                // it is a valid non-friend in the same city
                if (user.City == currentPerson.City && !user.IsFriend(currentPerson) && user != currentPerson)
                {
                    // Add the person to the list 
                    sameCity.Add(currentPerson);
                }
            }
            // Remove duplicates from the list and return it
            return sameCity.Distinct().ToList();
        }

        /// <summary>
        /// Updates a user's list of up to 10 unique non-friends in the same city.
        /// </summary>
        /// <param name="user">The user for whom the list of up to 10 non-friends in the same city must be updated.</param>
        public void UpdateSameCity(Person user)
        {
            // Get the user's non-friends in the same city and update the user's list with the first 10 of them
            user.SameCity = FindSameCity(user).Take(10).ToList();
        }

        /// <summary>
        /// Constructs a list of all unique non-friends in the same city who share at least one interest with the user.
        /// </summary>
        /// <param name="user">The user for whom the list of non-friends in the same city with the same interest must be found.</param>
        /// <returns>A list of all of the unique non-friends in the user's city who share at least one interest.</returns>
        private List<Person> FindSameCitySameInterest(Person user)
        {
            // Get all of the user's unique non-friends in the same city
            List<Person> validSameCity = FindSameCity(user);
            // Filter out all of the same-city non-friends who don't share an interest
            validSameCity.RemoveAll(person => !ShareSameInterest(person, user));
            // Return the list
            return validSameCity;
        }

        /// <summary>
        /// Updates a user's list of up to 10 unique non-friends in the same city who share at least one interest with the user.
        /// </summary>
        /// <param name="user">The user for whom the list of non-friends in the same city with a shared interest should be updated.</param>
        public void UpdateSameCitySameInterest(Person user)
        {
            // Update the user's list to the first 10 (or less, if there are less than 10 items) valid non-friends in the same city
            user.SameCitySameInterest = FindSameCitySameInterest(user).Take(10).ToList();
        }
        
        /// <summary>
        /// Updates all 4 recommendation lists for a given user:
        ///     - Friends of friends
        ///     - Friends of friends with same interest
        ///     - Same city
        ///     - Same city with same interest
        /// </summary>
        /// <param name="user">The user whose lists must be updated.</param>
        public void UpdateAllRecommendations(Person user)
        {
            UpdateFriendsOfFriends(user);
            UpdateFriendsOfFriendsSameInterest(user);
            UpdateSameCity(user);
            UpdateSameCitySameInterest(user);
        }

        /// <summary>
        /// Checks if a username and password combination matches that of a user in the network
        /// </summary>
        /// <param name="username">The username to be tested.</param>
        /// <param name="password">The password to be tested.</param>
        /// <returns>The unique Person object with the given username and password if it exists in the network,
        /// and null otherwise.</returns>
        public Person FindUserInNetwork(string username, string password)
        {
            // Iterate through all users in the network
            foreach(Person user in users)
            {
                // Check if there is a match of the login credentials
                if(user.Username == username && user.Password == password)
                {
                    // Return the matching user
                    return user;
                }
            }
            // Return null if the user with the given login credentials is not found 
            return null;
        }

        /// <summary>
        /// Checks if a given username is not already taken to maintain the invariant that all usernames in the network are unique.
        /// </summary>
        /// <param name="username">The username to check for availability.</param>
        /// <returns>True if the username is available, and false otherwise.</returns>
        public bool IsUsernameAvailable(string username)
        {
            // Iterate through all users in the network
            foreach(Person user in users)
            {
                // Check if the current user has the given username
                if(user.Username == username)
                {
                    // The username is not available if it matches an existing one
                    return false;
                }
            }
            // The username is available if it does not match any existing ones
            return true;
        }
        
        /// <summary>
        /// Checks if two different users share at least one interest.
        /// </summary>
        /// <param name="user1">First user to compare.</param>
        /// <param name="user2">Second user to compare.</param>
        /// <returns>True if at least one interest is shared, and false otherwise.</returns>
        private bool ShareSameInterest(Person user1, Person user2)
        {
            // Check if the intersection between the users' interests list is not the empty set
            return user1.Interests.Intersect(user2.Interests).Any();
        }
    }
}
