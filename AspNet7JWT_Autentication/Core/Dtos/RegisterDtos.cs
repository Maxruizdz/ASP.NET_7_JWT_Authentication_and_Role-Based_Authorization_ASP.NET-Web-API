﻿using System.ComponentModel.DataAnnotations;

namespace AspNet7JWT_Autentication.Core.Dtos
{
    public class RegisterDtos
    {
        [Required(ErrorMessage="Username is required")]
        public int UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public int Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public int Password { get; set; }



    }
}