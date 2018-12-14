/*
 * Rudy Ariaz
 * December 16, 2018
 * LoginForm manages elements of the login UI, including login and registration fields, allowing
 * users to login to the network or register new users.
 */
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

        /// <summary>
        /// Construct a new LoginForm object that uses the given network.
        /// </summary>
        /// <param name="network">Network to use for login and registration purposes.</param>
        public LoginForm(Network network)
        {
            InitializeComponent();
            // Make this form fullscreen
            WindowState = FormWindowState.Maximized;
            // Set the network that the login form accesses and modifies
            this.network = network;
        }
       
        /// <summary>
        /// Runs when the login button is clicked in order to authenticate the user.
        /// </summary>
        private void btnLogin_Click(object sender, EventArgs e)
        {
            // Get the username and password entered into the textboxes
            string username = txtLoginUsername.Text, password = txtLoginPassword.Text;
            // If no username was entered, display an appropriate error message
            if(username == "")
            {
                MessageBox.Show("Please enter a username.");
            }
            // Otherwise, if no password was entered, display an appropriate error message
            else if(password == "")
            {
                MessageBox.Show("Please enter a password.");
            }
            // Otherwise, try to authenticate the user 
            else
            {
                // Try to find the user with the given credentials in the network
                Person user = network.FindUserInNetwork(username, password);
                // Instantiate the main user interface if a user was found (is not null)
                if (user != null)
                {
                    // Instantiate the user interface
                    MainUIForm frmUI = new MainUIForm(network, user);
                    // Display the user interface
                    frmUI.ShowDialog();
                    // Close the login form
                    this.Close();
                }
                // Otherwise, the user was not found in the network, so display an appropriate error message
                else
                {
                    MessageBox.Show("Invalid login information. Please try again.");
                }
            }
        }

        /// <summary>
        /// Runs when the registration button is clicked in order to register a new user.
        /// </summary>
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
            // Check if the username is already taken
            else if (!network.IsUsernameAvailable(txtRegisterUsername.Text))
            {
                // Show an error if the username is taken
                MessageBox.Show("Username is already taken. Please enter a different one.");
            }
            // If all information was entered and is valid, register the new user 
            else
            {
                // Instantiate a new Person object with the given information
                Person newUser = new Person(txtRegisterFirstName.Text, 
                                            txtRegisterLastName.Text, 
                                            txtRegisterCity.Text, 
                                            txtRegisterUsername.Text, 
                                            txtRegisterPassword.Text);
                // Add the new user to the network
                network.AddNewUser(newUser);
                // Instantiate a new MainUIForm object with the new user and given network
                MainUIForm frmUI = new MainUIForm(network, newUser);
                // Show the main user interface form 
                frmUI.ShowDialog();
                // Close the login form
                this.Close();
            }
        }
    }
}
