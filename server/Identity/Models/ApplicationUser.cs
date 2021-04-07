using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public bool IsVegan { get; set; }
        public ICollection<Allergy> Allergies { get; set; } = new List<Allergy>();
    }
}