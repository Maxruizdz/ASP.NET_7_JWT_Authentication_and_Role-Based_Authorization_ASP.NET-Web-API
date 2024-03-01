using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AspNet7JWT_Autentication.Core.Entities
{
    public class ApplicationUser: IdentityUser
    {
     
        public string FirtsName  { get; set; }
        public string LastName { get; set; }
    }
}
