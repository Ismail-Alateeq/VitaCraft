using System.Collections.Generic;

namespace VitaCraft.Models
{
    public class EndUser : ApplicationUser
    {
        public List<Resume> resumes { get; set; } 
        public List<PortFolio> portFolios  { get; set; }
    }
}
