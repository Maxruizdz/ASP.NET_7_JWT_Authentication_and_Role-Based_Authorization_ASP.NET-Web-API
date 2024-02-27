using System.ComponentModel.DataAnnotations;

namespace AspNet7JWT_Autentication.Core.Dtos
{
    public class UpdatePermissionDto
    {
        [Required(ErrorMessage = "Username is required")]
        public int UserName { get; set; }

        
    }
}
