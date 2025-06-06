using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kassa.Data
{
    public class UserInformation
    {
        private Gebruiker _loggedInUser;

        public Gebruiker LoggedInUser
        {
            get { return _loggedInUser; }
            private set
            {
                _loggedInUser = value;

            }
        }


        public UserInformation() { LoggedInUser = null; }

        public void Login(Gebruiker gebruiker) { 
            LoggedInUser = gebruiker;
        }

        public void Logout() {
            LoggedInUser = null; 
        }

    }
}
