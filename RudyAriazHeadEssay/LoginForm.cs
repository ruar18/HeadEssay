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
        private Network network = new Network();

        public LoginForm()
        {
            InitializeComponent();
        }
        

        // TODO: complete, null check
        // Logs user in if information is accurate
        private void Login(string username, string password)
        {
            Person user = network.FindUserInNetwork(username, password);
            // Create user interface if a user was found 
            if(user != null)
            {
                MainUIForm frmUI = new MainUIForm(network, user);
                frmUI.ShowDialog();
            }
            // Print error message 
            else
            {
                MessageBox.Show("Invalid login information. Please try again.");
            }
        }
        
        // Registers a new user
        // TODO: empty checks? check when it's done in our previous programs
        private void RegisterUser(string firstName, string lastName, string city, 
                                  string username, string password)
        {
            Person newUser = new Person(firstName, lastName, city, username, password);
            network.AddNewUser(newUser);
            MainUIForm frmUI = new MainUIForm(network, newUser);
            frmUI.ShowDialog();
        }



    }
}
