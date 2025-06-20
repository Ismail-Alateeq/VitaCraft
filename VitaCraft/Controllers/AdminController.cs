using Microsoft.AspNetCore.Mvc;
using VitaCraft.Repository;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using VitaCraft.Models.ViewModels;
using VitaCraft.Models;

namespace VitaCraft.Controllers
{

    public class AdminController : Controller
    {
        private readonly IResumeRepository _resumeRepo;
        private readonly IPortFolioRepository _portfolioRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(
            IResumeRepository resumeRepo,
            IPortFolioRepository portfolioRepo,
            UserManager<ApplicationUser> userManager)
        {
            _resumeRepo = resumeRepo;
            _portfolioRepo = portfolioRepo;
            _userManager = userManager;
        }
        [Authorize]

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var resumes = await _resumeRepo.GetAllAsync();
            var portfolios = await _portfolioRepo.GetAllAsync();

            var model = new AdminDashboardViewModel
            {
                TotalUsers = users.Count,
                TotalResumes = resumes.Count,
                TotalPortfolios = portfolios.Count,
                TotalTemplates = 24,
                LatestUsers = users.OrderByDescending(u => u.Id).Take(5).ToList()
            };

            return View(model);
        }
    }
}