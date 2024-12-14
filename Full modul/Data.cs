using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Full_modul
{
    public static class Constants
    {
        public static string ConnectionString = "Data Source=DESKTOP-TBLQV8A\\SQLEXPRESS;Initial Catalog=calculator;Integrated Security=True;TrustServerCertificate=True";
    }

    static class Data
    {
        public static bool Check0 = false;
        public static bool Check1 = false;
        public static bool Check2 = false;
        public static bool Check3 = false;
        public static int BoxIndex = -1;
    }
    static class UserInfo
    {
        public static string username = "";
        public static string password = "";
    }
}
