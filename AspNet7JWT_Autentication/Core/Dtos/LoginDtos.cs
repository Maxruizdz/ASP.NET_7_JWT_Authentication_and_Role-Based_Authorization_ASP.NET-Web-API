using System.ComponentModel.DataAnnotations;

namespace AspNet7JWT_Autentication.Core.Dtos
{
    public class LoginDtos
    {
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }

      

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
