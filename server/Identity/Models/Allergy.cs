using System.Collections.Generic;

namespace Identity.Models
{
    public class Allergy
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<ApplicationUser> ApplicationUsers { get; set; }
            = new List<ApplicationUser>();
    }
}