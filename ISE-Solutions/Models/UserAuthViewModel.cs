using System;

namespace ISE_Solutions.Model
{
    public class UserAuthViewModel
    {
        public Int64 ID { get; set; }
        public bool IsAuthenticate { get; set; }
        public UserAuthDetailsViewModel UserAuthDetail { get; set; }
        public string Message { get; set; }

        public UserAuthViewModel()
        {
            this.UserAuthDetail = new UserAuthDetailsViewModel();
        }
    }
    public class UserAuthDetailsViewModel
    {   
       
        public string FirstName { get; set; }
        public string LastName { get; set; }       
        public string Email { get; set; }
        public string Department { get; set; }
        public int? RoleID { get; set; }
        public string RoleName { get; set; }
        public string UserName { get; set; }


       
    }
}
