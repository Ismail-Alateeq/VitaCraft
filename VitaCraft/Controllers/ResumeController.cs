using Microsoft.AspNetCore.Mvc;
using VitaCraft.Models.DTOs;
using VitaCraft.Repository;
using System.Linq;
using VitaCraft.Models;
using VitaCraft.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Build.Evaluation;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace VitaCraft.Controllers
{
    public class ResumeController : Controller
    {
        private readonly IResumeRepository _resumeRepository;
        private readonly IResumeOpenAiSrevice _srevice;

        public ResumeController(IResumeRepository resumeRepository, IResumeOpenAiSrevice srevice)
        {
            _resumeRepository = resumeRepository;
            _srevice = srevice;
        }

        // GET: Resume
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var resume = await _resumeRepository.GetByUserId(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var r = ToResumeJsonDtoList(resume);
            return View(r);
        }
        public IActionResult Create()
        {

            return View(new ResumeDTO());
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var resume = await _resumeRepository.GetByIdAsync(Id);

            if (resume == null)
            {
                return NotFound();
            }

          
            var resumeJsonDto = MapToResumeJsonDto(resume);


            return View(resumeJsonDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ResumeJsonDto model)
        {
            if (!ModelState.IsValid)
            {
                
                return View("Edit", model);
            }
            var resume = MapToResumeEntity(model, User.FindFirstValue(ClaimTypes.NameIdentifier));
            resume.resumeId = model.resumeId;
            await _resumeRepository.UpdateAsync(resume);
            
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            try
            {
                var resume = await _resumeRepository.GetByIdAsync(Id);

                if (resume == null)
                {
                    TempData["ErrorMessage"] = " ";
                    return RedirectToAction("Index");
                }

                // Verify the resume belongs to the current user
                if (resume.EndUserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                {
                    TempData["ErrorMessage"] = "You are not allowed to delete this resume.";
                    return Forbid();
                }

                await _resumeRepository.DeleteAsync(resume.resumeId);
                TempData["SuccessMessage"] = "Resume deleted successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while trying to delete the resume.{ex}";
                return RedirectToAction("Index");
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Preview(int Id)
        {
            // Get the resume by ID
            var resume = await _resumeRepository.GetByIdAsync(Id);

            if (resume == null)
            {
                return NotFound();
            }

            // Verify the resume belongs to the current user
            if (resume.EndUserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return Forbid();
            }

            // Map the resume entity to DTO
            var resumeJsonDto = MapToResumeJsonDto(resume);

            return View("ResumeTemplate", resumeJsonDto);
        }


        public IActionResult ResumeTemplate(ResumeJsonDto json)
        {

            return View(json);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessStep(ResumeDTO model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _srevice.ParseResumeAsync(model);
                    if (result == null)
                    {
                        return View("Create", model);
                    }
                    else
                    {
                        var r = MapToResumeEntity(result, User.FindFirstValue(ClaimTypes.NameIdentifier));
                        await _resumeRepository.AddAsync(r);
                        return RedirectToAction("Index");
                    }

                }
                catch (Exception ex)
                {
                        return View("Create", model);

                }

            }
            else
            {

                return View("Create", model);
            }



        }

        private Resume MapToResumeEntity(ResumeJsonDto dto, string userId)
        {
            return new Resume
            {
                resumeId = dto.resumeId,
                address = dto.Address,
                firstName = dto.FirstName,
                lastName = dto.LastName,
                email = dto.Email,
                phoneNumber = dto.PhoneNumber,
                title = dto.Title,
                bio = dto.Summary ?? dto.Summary,
                gitHub = dto.GitHubLink,
                linkedIn = dto.LinkedinLink,
                EndUserId = userId, 
                

                // Date/time fields:
                createdDate = dto.CreatedDate,
                modifiedDate = DateTime.UtcNow.ToShortDateString(),

                // Collections, mapped (you may need to adjust for navigation properties):
                educations = dto.Educations?.Select(e => new Education
                {
                    collageName = e.CollegeName,
                    degreeType = e.DegreeType,
                    major = e.Major,
                    startDate = e.StartDate,
                    endDate = e.EndDate,
                    GPA = e.GPA
                }).ToList() ?? new List<Education>(),

                experiences = dto.Experiences?.Select(x => new Experience
                {
                    Title = x.Title,
                    company = x.Company,
                    startDate = x.StartDate,
                    endDate = x.EndDate,
                    isCurrent = x.IsCurrent ?? false,
                    duties = x.Duties
                }).ToList() ?? new List<Experience>(),

                skills = dto.Skills?.Select(s => new Skill
                {
                    skillName = s.SkillName,
                    skillType = s.SkillType
                }).ToList() ?? new List<Skill>(),

                certificates = dto.Certificates?.Select(c => new Certificate
                {
                    providerName = c.ProviderName,
                    topicName = c.Field,
                    startDate = c.StartDate,
                    endDate = c.EndDate,
                    GPA = c.GPA
                }).ToList() ?? new List<Certificate>(),
            };
        }

        private ResumeJsonDto MapToResumeJsonDto(Resume entity)
        {
            return new ResumeJsonDto
            {
                resumeId = entity.resumeId,
                GitHubLink = entity.gitHub,
                CreatedDate = entity.createdDate,
                ModifiedDate = entity.modifiedDate,
                EndUserId = entity.EndUserId,
                FirstName = entity.firstName,
                LastName = entity.lastName,
                Email = entity.email,
                PhoneNumber = entity.phoneNumber,
                Address = entity.address,
                Summary = entity.bio,
                Title = entity.title,
                LinkedinLink = entity.linkedIn,

                // Collections
                Educations = entity.educations?.Select(e => new EducationItem
                {
                    CollegeName = e.collageName,
                    DegreeType = e.degreeType,
                    Major = e.major,
                    StartDate = e.startDate,
                    EndDate = e.endDate,
                    GPA = e.GPA
                }).ToList(),

                Experiences = entity.experiences?.Select(e => new ExperienceItem
                {
                    Title = e.Title,
                    Company = e.company,
                    StartDate = e.startDate,
                    EndDate = e.endDate,
                    IsCurrent = e.isCurrent,
                    Duties = e.duties
                }).ToList(),

                Skills = entity.skills?.Select(s => new SkillItem1
                {
                    SkillName = s.skillName,
                    SkillType = s.skillType
                }).ToList(),

                Certificates = entity.certificates?.Select(c => new CertificateItem
                {
                    ProviderName = c.providerName,
                    Field = c.topicName,
                    StartDate = c.startDate,
                    EndDate = c.endDate,
                    GPA = c.GPA
                }).ToList(),

            };
        }

        private List<ResumeJsonDto> ToResumeJsonDtoList(List<Resume> entities)
        {
            if (entities == null)
                return new List<ResumeJsonDto>();

            return entities.Select(entity => new ResumeJsonDto
            {
                resumeId = entity.resumeId,
                GitHubLink = entity.gitHub,
                FirstName = entity.firstName,
                LastName = entity.lastName,
                Email = entity.email,
                PhoneNumber = entity.phoneNumber,
                Address = entity.address,
                Summary = entity.bio,
                Title = entity.title,
                LinkedinLink = entity.linkedIn,

                Educations = entity.educations?.Select(e => new EducationItem
                {
                    CollegeName = e.collageName,
                    DegreeType = e.degreeType,
                    Major = e.major,
                    StartDate = e.startDate,
                    EndDate = e.endDate,
                    GPA = e.GPA
                }).ToList(),

                Experiences = entity.experiences?.Select(e => new ExperienceItem
                {
                    Title = e.Title,
                    Company = e.company,
                    StartDate = e.startDate,
                    EndDate = e.endDate,
                    IsCurrent = e.isCurrent,
                    Duties = e.duties
                }).ToList(),

                Skills = entity.skills?.Select(s => new SkillItem1
                {
                    SkillName = s.skillName,
                    SkillType = s.skillType
                }).ToList(),

                Certificates = entity.certificates?.Select(c => new CertificateItem
                {
                    ProviderName = c.providerName,
                    Field = c.topicName,
                    StartDate = c.startDate,
                    EndDate = c.endDate,
                    GPA = c.GPA
                }).ToList(),

            }).ToList();
        }
    }

}

