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
    public partial class MainUIForm : Form
    {
        private Network network;
        private Person user;

        public MainUIForm(Network network)
        {
            InitializeComponent();
            this.network = network;
        }

        // Logs user out
        public void LogOut()
        {

        }

        // Accept an invitation
        public void AcceptInvitation(Invitation invitation)
        {
            user.AcceptInvitation(invitation);
        }
    }
}
