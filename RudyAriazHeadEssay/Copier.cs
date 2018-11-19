﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RudyAriazHeadEssay
{
    public static class Copier
    {
        // Do a shallow copy
        public static List<T> CopyList<T>(List<T> toCopy)
        {
            List<T> copyOfList = new List<T>();
            foreach (T item in toCopy)
            {
                copyOfList.Add(item);
            }
            return copyOfList;
        }

    }
}
