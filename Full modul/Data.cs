using Full_modul.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Full_modul
{
    public static class Data
    {
        public static bool Check0 = false;
        public static bool Check1 = false;
        public static bool Check2 = false;
        public static bool Check3 = false;
        public static int BoxIndex = -1;
        public static int SaveFile = 1;
    }

    public static class UserInfo
    {
        private static string _username = "";
        private static string _password = "";

        public static string username
        {
            get => _username;
            set => _username = value ?? "";
        }

        public static string password
        {
            get => _password;
            set => _password = value ?? "";
        }

        public static void ClearTempData()
        {
            if (!Settings.Default.RememberMe)
            {
                _username = "";
                _password = "";
            }
        }
    }
}
