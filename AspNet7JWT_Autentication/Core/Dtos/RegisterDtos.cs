using System.ComponentModel.DataAnnotations;

namespace AspNet7JWT_Autentication.Core.Dtos
{
    public class RegisterDtos
    {
        [Required(ErrorMessage="Username is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }



    }
}
