using Identity.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Identity.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public string DisplayName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{4,8}$", ErrorMessage = "Password is not safe enough")]
        public string Password { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public bool isVegan { get; set; }

        public ICollection<Allergy> Allergies { get; set; }
    }
}
