using Microsoft.AspNetCore.Identity;

namespace VitaCraft.Models.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalResumes { get; set; }
        public int TotalPortfolios { get; set; }
        public int TotalTemplates { get; set; }
        public List<ApplicationUser> LatestUsers { get; set; }
    }
}