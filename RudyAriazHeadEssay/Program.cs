/*
 * Rudy Ariaz
 * December 16, 2018
 * The Program class is the entry point for the application, which first creates the social network and the 
 * login form that accesses the network.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RudyAriazHeadEssay
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // Create a new login form that uses a new network
            Application.Run(new LoginForm(new Network()));
        }
    }
}
