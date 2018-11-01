using System.ComponentModel.DataAnnotations;

namespace ISE_Solutions.Model
{
    public class HomeLoginViewModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
       
        public string Message { get; set; }

    }
}
