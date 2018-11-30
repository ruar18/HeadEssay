﻿using System;
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

        public Network() { }

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
        private void DeleteInactiveInvitations()
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
                    // Add the friend of friend to the list
                    friendsOfFriends.Add(friendOfFriend);
                }
            }
            // Set the user's friends of friends 
            user.SetFriendsOfFriends(friendsOfFriends.Distinct().ToList());
        }

        // All unique friends of friends with the same interest 
        public void FindFriendsOfFriendsWithSameInterest(Person user)
        {
            // Find the user's friends of friends 
            FindFriendsOfFriends(user);
            // Create the list that will store the friends of friends with same interest
            List<Person> validFriendsOfFriends = user.GetFriendsOfFriends();
            // Filter the user's friends of friends to only those with 
            // the same interest
            foreach(Person friendOfFriend in validFriendsOfFriends)
            {
                // Check if the friend of friend shares an interest by checking 
                bool interestShared = friendOfFriend.GetAllInterests().Intersect(user.GetAllInterests()).Any();
                // If there is no shared interest, delete the friend of friend
                if (!interestShared)
                {
                    validFriendsOfFriends.Remove(friendOfFriend);
                }
            }
            // Set the user's friends of friends with same interest 
            user.SetFriendsOfFriendsSameInterest(validFriendsOfFriends);
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
        public void RefreshAfterFriendChange(Person user)
        {
            FindFriendsOfFriends(user);
            FindFriendsOfFriendsWithSameInterest(user);
            SameCity(user);
            SameCitySameInterest(user);
        }

        // Do the users share at least one interest?
        private bool ShareSameInterest(Person user1, Person user2)
        {
            return user1.GetAllInterests().Intersect(user1.GetAllInterests()).Any();
        }

        

        

        // Find up to 10 people in the same city that share at least one interest
        public void SameCitySameInterest(Person user)
        {

        }

        public void SameCity(Person user)
        {

        }

        

        


    }
}
