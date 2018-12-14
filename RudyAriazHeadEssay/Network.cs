using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RudyAriazHeadEssay
{
    public class Network
    {
        // Store all the users on the social network
        private List<Person> users = new List<Person>();
        // Explicitly store all the invitations on a social network 
        private List<Invitation> invitations = new List<Invitation>();

        // Create a new network
        public Network()
        {
            // TODO: remove temporary people
            users.Add(new Person("Rudy", "Ariaz", "Toronto", "rariaz", "hi"));
            users.Add(new Person("Willie", "stuff", "Toronto", "wchan", "hi"));
            users.Add(new Person("Tiff", "T", "Toronto", "ttruong", "hi"));
            users.Add(new Person("Henning", "L", "munich", "lhenning", "hi"));
            users[0].AddFriend(users[1]);
            users[0].AddFriend(users[2]);
            users[1].AddFriend(users[3]);
            users[0].AddInterest("swimming");
            users[3].AddInterest("swimming");
        }

        // TODO: check if there are restrictions on users size 
        // TODO: catch exception
        // Add a user that was not in the graph before
        public void AddNewUser(Person user)
        {
            users.Add(user);
        }

        /// <summary>
        /// Delete all invitations which have exceeded their lifespan.
        /// Does this semi-lazily: only when refreshed.
        /// </summary>
        public void DeleteInactiveInvitations()
        {
            // Go through all of the invitations in the network
            foreach(Invitation sent in invitations)
            {
                // Check if the current invitation is inactive
                if (!sent.IsActive())
                {
                    // Delete the inactive invitation from the sent list
                    sent.Creator.DeleteOutgoingInvitation(sent);
                    // Delete the inactive invitation from the incoming lists
                    foreach(Person recipient in sent.GetAllRecipients())
                    {
                        recipient.DeleteIncomingInvitation(sent);
                    }
                }
            }
        }

        // Add the invitations to all the correct lists
        public void DeliverInvitation(Invitation invitation)
        {
            // Record the invitation in the list of all invitations
            invitations.Add(invitation);
            // Add the invitation to the sender's list of outgoing invitations
            invitation.Creator.AddOutgoingInvitation(invitation);
            // Add the invitation to the recipients' recipient list 
            foreach (Person recipient in invitation.GetAllRecipients())
            {
                recipient.ReceiveInvitation(invitation);
            }
        }

        // Finds unique friends of friends for user
        public void FindFriendsOfFriends(Person user)
        {
            // Create a new list to store the friends of friends 
            List<Person> friendsOfFriends = new List<Person>();
            // Go through all of the user's friends
            foreach (Person friend in user.GetAllFriends())
            {
                // Go through all of the friend's friends
                foreach (Person friendOfFriend in friend.GetAllFriends())
                {
                    // Check if the friend of friend is not a friend and is not the user itself
                    if (user != friendOfFriend && !user.GetAllFriends().Contains(friendOfFriend))
                    {
                        // Add the friend of friend to the list
                        friendsOfFriends.Add(friendOfFriend);
                    }
                }
            }
            // Set the user's friends of friends 
            user.SetFriendsOfFriends(friendsOfFriends.Distinct().ToList());
        }

        // All unique friends of friends with the same interest  
        // TODO: remove
        public void FindFriendsOfFriendsWithSameInterest(Person user)
        {
            // Find the user's friends of friends 
            FindFriendsOfFriends(user);
            // Create the list that will store the friends of friends with same interest
            List<Person> validFriendsOfFriends = user.GetFriendsOfFriends();
            // Filter the user's friends of friends to only those with the same interest
            validFriendsOfFriends.RemoveAll(person => !ShareSameInterest(person, user));
            // Set the user's friends of friends with same interest 
            user.SetFriendsOfFriendsSameInterest(validFriendsOfFriends);
        }

        // Up to 10 unique non-friends in the same city
        public void FindSameCity(Person user)
        {
            // Create the list that will store individuals in the same city
            List<Person> sameCity = new List<Person>();
            // Go through all people in the network
            foreach(Person currentPerson in users)
            {
                // If the current person is not the user's friend and is not the user itself, it is a valid user
                // Check if the current person is in the same city
                if (user != currentPerson && !user.IsFriend(currentPerson) && user.City == currentPerson.City)
                {
                    // Add the person to the list 
                    sameCity.Add(currentPerson);
                }
            }
            // Set the user's non-friends in the same city after removing duplicates 
            user.SetSameCity(sameCity.Distinct().ToList());
        }

        // Find up to 10 non-friends in the same city that share at least one interest
        public void FindSameCitySameInterest(Person user)
        {
            // Find the user's non-friends in the same city
            FindSameCity(user);
            // Get the user's non-friends in the same city
            List<Person> validSameCity = user.GetSameCity();
            // Get the user's interests
            List<string> interests = user.GetAllInterests();
            // Filter out all of the same-city non-friends who don't share an interest
            validSameCity.RemoveAll(person => !ShareSameInterest(person, user));
            // Set the user's list 
            user.SetSameCitySameInterest(validSameCity);
        }

        // Check if username and password matches that of a person in the network
        public Person FindUserInNetwork(string username, string password)
        {
            foreach(Person user in users)
            {
                // Found a match
                if(user.Username == username && user.Password == password)
                {
                    return user;
                }
            }
            // Return null if user is not found 
            return null;
        }


        // After user's friend is added/removed, update all of the recommendation lists
        public void GenerateRecommendationLists(Person user)
        {

            FindFriendsOfFriends(user);
            FindFriendsOfFriendsWithSameInterest(user);
            FindSameCity(user);
            FindSameCitySameInterest(user);
        }


        /// <summary>
        /// Checks if two users share at least one interest.
        /// </summary>
        /// <param name="user1">First user to compare.</param>
        /// <param name="user2">Second user to compare.</param>
        /// <returns>True if at least one interest is shared, false otherwise.</returns>
        private bool ShareSameInterest(Person user1, Person user2)
        {
            return user1.GetAllInterests().Intersect(user2.GetAllInterests()).Any();
        }
    }
}
