using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs.Account
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Username is required")] public string Username { get; set; }
        [Required(ErrorMessage = "Username is required")] public string Password { get; set; }
    }
}