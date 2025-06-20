using Microsoft.CodeAnalysis;

namespace VitaCraft.Models
{
    public class PortFolio : UserProfile
    {

        public int portFolioId { get; set; }
        
        public List<Service> services { get; set; } 
        public List<Projectt> projects { get; set; }
        public List<Skill> skills { get; set; }
        public string? createdDate { get; set; }
        public string? modifiedDate { get; set; }
        public EndUser EndUser { get; set; }
        public string EndUserId { get; set; }

    }
}
