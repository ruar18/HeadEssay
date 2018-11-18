using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RudyAriazHeadEssay
{
    public partial class LoginForm : Form
    {
        // HeadEssay's social network
        private Network network;

        public LoginForm(Network network)
        {
            InitializeComponent();
            this.network = network;
        }
        

        // TODO: complete, null check
        // Logs user in if information is accurate
        private void Login(string userName, string password)
        {
            // Create user interface 
            if(network.IsUserInNetwork(userName, password))
            {
                MainUIForm frmUI = new MainUIForm();

            }
            // Print error message 
            else
            {
               
            }
        }
        
        // Registers a new user
        // TODO: empty checks? check when it's done in our previous programs
        private void RegisterUser(string firstName, string lastName, string city, 
                                  string userName, string password)
        {
            Person newUser = new Person(firstName, lastName, city, userName, password);
            network.AddNewUser(newUser);
        }



    }
}
