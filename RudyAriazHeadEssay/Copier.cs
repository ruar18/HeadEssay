/*
 * Rudy Ariaz
 * December 16, 2018
 * Copier is a utility class that provides shallow-copying functionality to other classes.
 * It was designed to enable data hiding of lists.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RudyAriazHeadEssay
{
    public static class Copier
    {
        /// <summary>
        /// Shallow-copies a generic list and returns a non-null copy.
        /// </summary>
        /// <typeparam name="T">The type of elements of the list to be copied.</typeparam>
        /// <param name="toCopy">The list to be copied.</param>
        /// <returns>A shallow copy of the elements of the original list in their original order.
        /// If "toCopy" is null, returns an empty list of the same type.</returns>
        public static List<T> CopyList<T>(List<T> toCopy)
        {
            // Check if the list to copy is null
            if (toCopy == null)
            {
                // Return an empty list if "toCopy" is null
                return new List<T>();
            }
            // Instantiate the copy of "toCopy"
            List<T> copyOfList = new List<T>();
            // Iterate through the elements to be copied
            foreach (T item in toCopy)
            {
                // Add each element to the list copy
                copyOfList.Add(item);
            }
            // Return the list copy
            return copyOfList;
        }
    }
}
