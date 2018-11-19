﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RudyAriazHeadEssay
{
    // TODO: why is this public? 
    public class Network
    {
        // Store all the users on the social network in an adjacency list
        private List<Person> users = new List<Person>();
        private Dictionary<Person, List<Person>> connections =
                  new Dictionary<Person, List<Person>>();

        public Network() { }

        /// <summary>
        /// Delete all invitations which have exceeded their lifespan.
        /// </summary>
        public void DeleteInactiveInvitations() { }

        public bool IsUserInNetwork(string userName, string password)
        {
            return true;
        }
        
        // Finds unique friends of friends for user
        // TODO: test if HashSet works as expected
        public HashSet<Person> FriendsOfFriends(Person user)
        {
            HashSet<Person> fOfF = new HashSet<Person>();
            foreach(Person friend in user.GetAllFriends())
            {
                foreach(Person friendOfFriend in friend.GetAllFriends())
                {
                    fOfF.Add(friendOfFriend);
                }
            }
            return fOfF;
        }


        // All friends of friends with the same interest 
        public HashSet<Person> FriendsOfFriendsWithSameInterest(Person user)
        {
            HashSet<Person> fOfFWithSameInterest = FriendsOfFriends(user);
            // Remove users if they don't have the same interest 
            foreach(Person friendOfFriend in fOfFWithSameInterest)
            {
                if(!friendOfFriend.interests)
            }
        }

        // TODO: check if there are restrictions on users size 
        // TODO: catch exception
        // Add a user that was not in the graph before
        public void AddNewUser(Person user)
        {
            if (user == null)
            {
                throw new System.ArgumentNullException("Parameter cannot be null", "user");
            }
            else
            {
                users.Add(user);
                connections.Add(user, new List<Person>());
            }

        }
    }
}
