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

        // Create a new LoginForm, constructor
        public LoginForm(Network network)
        {
            InitializeComponent();
            // Make this form fullscreen
            WindowState = FormWindowState.Maximized;
            // Set the network that the login form accesses
            this.network = network;
        }
       

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtLoginUsername.Text, password = txtLoginPassword.Text;
            Person user = network.FindUserInNetwork(username, password);
            // Create user interface if a user was found 
            if (user != null)
            {
                // Create the user interface and show it
                MainUIForm frmUI = new MainUIForm(network, user);
                frmUI.ShowDialog();
                // Close the login form
                this.Close();
            }
            // Print error message
            // TODO: separate into cases to give more informative errors
            else
            {
                MessageBox.Show("Invalid login information. Please try again.");
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            // Check if any information has not been entered
            if(txtRegisterFirstName.Text == "" || txtRegisterLastName.Text == "" ||
               txtRegisterCity.Text == "" || txtRegisterUsername.Text == "" ||
               txtRegisterPassword.Text == "")
            {
                // Show an error if any information is missing
                MessageBox.Show("Please fill all fields to register.");
            }
            else
            {
                // Create a new Person object
                Person newUser = new Person(txtRegisterFirstName.Text, 
                                            txtRegisterLastName.Text, 
                                            txtRegisterCity.Text, 
                                            txtRegisterUsername.Text, 
                                            txtRegisterPassword.Text);
                // Add the new user to the network
                network.AddNewUser(newUser);
                // Instantiate a new MainUIForm
                MainUIForm frmUI = new MainUIForm(network, newUser);
                // Show the main user interface form 
                frmUI.ShowDialog();
            }
        }
        
    }
}
