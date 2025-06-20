using Microsoft.AspNetCore.Identity;

namespace VitaCraft.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string farstName { get; set; }
        public string lastName { get; set; }
    }
}
