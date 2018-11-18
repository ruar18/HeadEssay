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
    public partial class StartForm : Form
    {
        private Network network;

        public StartForm()
        {
            InitializeComponent();
            // Create the network
            network = new Network();
        }


        // Starts login process 
        // TODO: check accessibility (compare to first form in NebulaCraft)
        private void StartLogin()
        {
            LoginForm frmLogin = new LoginForm(network);
        }
    }
}
