using Microsoft.AspNetCore.Mvc;
using VitaCraft.Models.DTOs;
using VitaCraft.Repository;
using System.Threading.Tasks;
using VitaCraft.Models;
using VitaCraft.Interfaces;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using VitaCraft.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Build.Evaluation;

namespace VitaCraft.Controllers
{
    public class PortFolioController : Controller
    {
        private readonly IPortFolioRepository _portfolioRepository;
        private readonly IPortFolioOpenAiService _service;

        public PortFolioController(IPortFolioRepository portfolioRepository, IPortFolioOpenAiService service, ApplicationDbContext context)
        {
            _portfolioRepository = portfolioRepository;
            _service = service;
        }

        [Authorize]
        // GET: Portfolio
        public async Task<IActionResult> Index()
        {
            var portfolios = await _portfolioRepository.GetByUserId(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (portfolios == null)
            {
                portfolios = new List<PortFolio>();
            }

            var p = MapToPortfolioJsonDtoList(portfolios);
            return View(p);
        }
        [HttpGet]

        public IActionResult Create()
        {
            return View(new PortFolioDTO());
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PortFolioDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                if (model.ProfileImage != null && model.ProfileImage.Length > 0)
                {
                    using var ms = new MemoryStream();
                    await model.ProfileImage.CopyToAsync(ms);
                    var bytes = ms.ToArray();
                    model.ProfileImageBase64 = Convert.ToBase64String(bytes);
                    model.ProfileImageFileName = model.ProfileImage.FileName;
                    model.ProfileImageContentType = model.ProfileImage.ContentType;
                    model.ProfileImage = null;
                }

                // Process project images
                for (int i = 0; i < model.Projects.Count; i++)
                {
                    var project = model.Projects[i];
                    if (project.Image != null && project.Image.Length > 0)
                    {
                        using var ms = new MemoryStream();
                        await project.Image.CopyToAsync(ms);
                        var bytes = ms.ToArray();
                        project.ImageBase64 = Convert.ToBase64String(bytes);
                        project.ImageFileName = project.Image.FileName;
                        project.ImageContentType = project.Image.ContentType;
                        project.Image = null;
                    }
                }

                // Send data to AI service for processing
                var aiResult = await _service.ParsePortFolioAsync(model);

                if (aiResult == null)
                {
                    TempData["Error"] = "AI service failed to process your portfolio.";
                    return View(model);
                }

                // Convert result to PortFolio entity
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var entity = MapToPortfolioEntity(aiResult, userId);
                entity.createdDate = DateOnly.FromDateTime(DateTime.UtcNow).ToString();
                entity.modifiedDate = DateTime.UtcNow.ToString();

                await _portfolioRepository.AddAsync(entity);

                TempData["Success"] = "Portfolio created successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"An error occurred while creating the portfolio: {ex.Message}";
                return View(model);
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var portfolio = await _portfolioRepository.GetByIdAsync(id);

            if (portfolio == null)
            {
                return NotFound();
            }

            // Verify the portfolio belongs to the current user
            if (portfolio.EndUserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return Forbid();
            }

            var portfolioDto = MapToPortfolioDto(portfolio);

            return View(portfolioDto);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PortfolioJsonDto model)
        {
            if (!ModelState.IsValid)
            {
                // Re-render the view with validation errors and keep current step
                return View("EditPortfolio", model);
            }
            var portfolio1 = MapToPortfolioEntity(model, User.FindFirstValue(ClaimTypes.NameIdentifier));
            portfolio1.portFolioId = model.portFolioId;
            await _portfolioRepository.UpdateAsync(portfolio1);
            // After a successful edit, redirect to Index
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Preview(int id)
        {
            // Get the portFolio by ID
            var portFolio = await _portfolioRepository.GetByIdAsync(id);

            if (portFolio == null)
            {
                return NotFound();
            }

            // Verify the portFolio belongs to the current user
            if (portFolio.EndUserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return Forbid();
            }

            // Map the portFolio entity to DTO
            var portfolioJson = MapToPortfolioDto(portFolio);

            return View("PortFolioTemplate", portfolioJson);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var portFolio = await _portfolioRepository.GetByIdAsync(id);

                if (portFolio == null)
                {
                    TempData["ErrorMessage"] = "Portfolio not found";
                    return RedirectToAction("Index");
                }

                // Verify the portfolio belongs to the current user
                if (portFolio.EndUserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                {
                    TempData["ErrorMessage"] = "You are not authorized to delete this portfolio";
                    return Forbid();
                }

                await _portfolioRepository.DeleteAsync(portFolio.portFolioId);
                TempData["SuccessMessage"] = "Portfolio deleted successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // You can log the error here if you have a logging system
                TempData["ErrorMessage"] = "An error occurred while trying to delete the portfolio";
                return RedirectToAction("Index");
            }
        }

        public IActionResult PortFolioTemplate(PortfolioJsonDto json)
        {
            return View(json);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessStep(PortFolioDTO model)
        {
            try
            {
                // 1. Process image if exists
                if (model.ProfileImage != null && model.ProfileImage.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        await model.ProfileImage.CopyToAsync(ms);
                        var bytes = ms.ToArray();
                        model.ProfileImageBase64 = Convert.ToBase64String(bytes);
                        model.ProfileImageFileName = model.ProfileImage.FileName;
                        model.ProfileImageContentType = model.ProfileImage.ContentType;
                    }
                    model.ProfileImage = null;
                }
                for (int i = 0; i < model.Projects.Count; i++)
                {
                    var project = model.Projects[i];
                    if (project.Image != null && project.Image.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            await project.Image.CopyToAsync(ms);
                            var bytes = ms.ToArray();
                            project.ImageBase64 = Convert.ToBase64String(bytes);
                            project.ImageFileName = project.Image.FileName;
                            project.ImageContentType = project.Image.ContentType;
                        }
                        project.Image = null; // Clear the file to avoid model binding issues
                    }
                    // If no new image was uploaded but we have existing base64 data, retain it
                    else if (project.Image == null && !string.IsNullOrEmpty(project.ImageBase64))
                    {
                        // Keep the existing image data
                    }
                }
                // 2. Send data to AI service
                var aiResult = await _service.ParsePortFolioAsync(model);

                if (aiResult == null)
                {
                    TempData["Error"] = "AI service failed to process your portfolio";
                    return View("Create", model);
                }
                var p = MapToPortfolioEntity(aiResult, User.FindFirstValue(ClaimTypes.NameIdentifier));
                await _portfolioRepository.AddAsync(p);

                // 3. Display the result
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // 4. Error handling
                TempData["Error"] = $"An error occurred: {ex.Message}";
                return View("Create", model);
            }
        }

        private PortFolio MapToPortfolioEntity(PortfolioJsonDto dto, string userId)
        {
            return new PortFolio
            {
                // Top-level properties:
                portFolioId = dto.portFolioId,
                firstName = dto.FirstName,
                lastName = dto.LastName,
                email = dto.Email,
                phoneNumber = dto.PhoneNumber,
                title = dto.title,
                address = dto.Address,
                bio = dto.Summery,
                ImageBase64 = dto.ImageBase64,
                ImageContentType = dto.ImageContentType,
                ImageFileName = dto.ImageFileName,
                gitHub = dto.gitHub,
                linkedIn = dto.linkedIn,
                EndUserId = userId,

                // Timestamps
                createdDate = DateOnly.FromDateTime(DateTime.UtcNow).ToString(),
                modifiedDate = DateTime.UtcNow.ToString(),

                // Services
                services = dto.Services?.Select(s => new Service
                {
                    serviceName = s.ServiceName,
                    serviceDescription = s.ServiceDescription
                }).ToList() ?? new List<Service>(),

                // Projects
                projects = dto.Projects?.Select(p => new Projectt
                {
                    projectName = p.ProjectName,
                    projectDescription = p.ProjectDescription,
                    startDate = string.IsNullOrWhiteSpace(p.StartDate) ? null : p.StartDate,
                    endDate = string.IsNullOrWhiteSpace(p.EndDate) ? null : p.EndDate,
                    IsOngoing = p.IsOngoing,
                    ImageBase64 = p.ImageBase64,
                    ImageContentType = p.ImageContentType,
                    ImageFileName = p.ImageFileName,
                    projectLink = p.ProjectLink
                }).ToList() ?? new List<Projectt>(),

                skills = dto.Skills?.Select(s => new Skill
                {
                    skillName = s.SkillName,
                    skillType = s.SkillType
                }).ToList() ?? new List<Skill>(),
            };
        }

        public List<PortfolioJsonDto> MapToPortfolioJsonDtoList(List<PortFolio> portfolios)
        {
            if (portfolios == null || portfolios.Count == 0)
                return new List<PortfolioJsonDto>();

            return portfolios.Select(p => new PortfolioJsonDto
            {
                portFolioId = p.portFolioId,
                title = p.title,
                FirstName = p.firstName,
                LastName = p.lastName,
                Email = p.email,
                PhoneNumber = p.phoneNumber,
                Address = p.address,
                Summery = p.bio,
                gitHub = p.gitHub,
                linkedIn = p.linkedIn,
                Services = p.services?.Select(s => new ServiceItem
                {
                    ServiceName = s.serviceName,
                    ServiceDescription = s.serviceDescription
                }).ToList() ?? new List<ServiceItem>(),
                Projects = p.projects?.Select(proj => new ProjectItem1
                {
                    ProjectName = proj.projectName,
                    ProjectDescription = proj.projectDescription,
                    StartDate = proj.startDate,
                    EndDate = proj.endDate,
                    ProjectLink = proj.projectLink
                }).ToList() ?? new List<ProjectItem1>()
            }).ToList();
        }

        private PortfolioJsonDto MapToPortfolioDto(PortFolio portfolio)
        {
            if (portfolio == null)
                return null;

            return new PortfolioJsonDto
            {
                portFolioId = portfolio.portFolioId,
                title = portfolio.title,
                FirstName = portfolio.firstName,
                LastName = portfolio.lastName,
                Email = portfolio.email,
                PhoneNumber = portfolio.phoneNumber,
                Address = portfolio.address,
                Summery = portfolio.bio,
                gitHub = portfolio.gitHub,
                linkedIn = portfolio.linkedIn,
                ImageBase64 = portfolio.ImageBase64,
                ImageFileName = portfolio.ImageFileName,
                ImageContentType = portfolio.ImageContentType,
                CreatedDate = portfolio.createdDate,
                ModifiedDate = portfolio.modifiedDate,
                Services = portfolio.services?.Select(s => new ServiceItem
                {
                    ServiceName = s.serviceName,
                    ServiceDescription = s.serviceDescription
                }).ToList() ?? new List<ServiceItem>(),
                Projects = portfolio.projects?.Select(p => new ProjectItem1
                {
                    ProjectName = p.projectName,
                    ProjectDescription = p.projectDescription,
                    StartDate = p.startDate,
                    EndDate = p.endDate,
                    IsOngoing = p.IsOngoing,
                    ProjectLink = p.projectLink,
                    ImageBase64 = p.ImageBase64,
                    ImageFileName = p.ImageFileName,
                    ImageContentType = p.ImageContentType
                }).ToList() ?? new List<ProjectItem1>(),
                Skills = portfolio.skills?.Select(s => new SkillItem
                {
                    SkillName = s.skillName,
                    SkillType = s.skillType,
                }).ToList() ?? new List<SkillItem>(),
            };
        }
    }
}