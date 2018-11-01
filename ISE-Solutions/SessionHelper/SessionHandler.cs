using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISE_Solutions.SessionHelper
{
    public static class SessionHandler
    {
        //*** Session String Values ***********************

        private static string _userID = "UserID";
        private static string _userRole = "UserRole";
        private static string _userName = "UserName";
        private static string _roleID = "RoleID";

        //*** Sets and Gets **********************************************************

        public static Int64 UserID
        {
            get
            {
                if (HttpContext.Current.Session[SessionHandler._userID] == null)
                { return 0; }
                else
                { return Convert.ToInt64(HttpContext.Current.Session[SessionHandler._userID]); }
            }
            set
            { HttpContext.Current.Session[SessionHandler._userID] = value; }

        }
        public static string UserRole
        {
            get
            {
                if (HttpContext.Current.Session[SessionHandler._userRole] == null)
                { return string.Empty; }
                else
                { return HttpContext.Current.Session[SessionHandler._userRole].ToString(); }
            }
            set
            { HttpContext.Current.Session[SessionHandler._userRole] = value; }
        }

        public static string UserName
        {
            get
            {
                if (HttpContext.Current.Session[SessionHandler._userName] == null)
                { return string.Empty; }
                else
                { return HttpContext.Current.Session[SessionHandler._userName].ToString(); }
            }
            set
            { HttpContext.Current.Session[SessionHandler._userName] = value; }

        }

        public static int RoleID
        {
            get
            {
                if (HttpContext.Current.Session[SessionHandler._roleID] == null)
                { return 0; }
                else
                { return Convert.ToInt32(HttpContext.Current.Session[SessionHandler._roleID]); }
            }
            set
            { HttpContext.Current.Session[SessionHandler._roleID] = value; }

        }


    }
}