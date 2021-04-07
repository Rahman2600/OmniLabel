using System.Collections.Generic;

namespace Identity.DTOs
{
    public class UserDTO
    {
        public string DisplayName { get; set; }
        public string Token { get; set; }
        public string Username { get; set; }
        public bool IsVegan { get; set; }
        public ICollection<string> Allergies { get; set; }
    }
}
