using VitaCraft.Data;
using VitaCraft.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace VitaCraft.Repository
{
    public class ResumeRepository : IResumeRepository
    {
        private readonly ApplicationDbContext _context;

        public ResumeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Resume> AddAsync(Resume resume)
        {
            _context.Resumes.Add(resume);
            await _context.SaveChangesAsync();
            return resume;
        }

        public async Task DeleteAsync(int id)
        {

            var resume = await _context.Resumes
                .Include(r => r.educations)
                .Include(r => r.experiences)
                .Include(r => r.skills)
                .Include(r => r.languages)
                .Include(r => r.certificates)
                .FirstOrDefaultAsync(r => r.resumeId == id);

            if (resume == null)
                throw new KeyNotFoundException($"Resume with ID {id} not found.");




            if (resume.educations != null)
                _context.Educations.RemoveRange(resume.educations);

            if (resume.experiences != null)
                _context.Experiences.RemoveRange(resume.experiences);
            if (resume.skills != null)
                _context.Skills.RemoveRange(resume.skills);

            if (resume.languages != null)
                _context.Languages.RemoveRange(resume.languages);

            if (resume.certificates != null)
                _context.Certificates.RemoveRange(resume.certificates);

            _context.Resumes.Remove(resume);    
            await _context.SaveChangesAsync();
        }
        public async Task<List<Resume>> GetAllAsync()
        {
            return await _context.Resumes.ToListAsync();
        }

        public async Task<Resume> GetByIdAsync(int id)
        {
            return await _context.Resumes
                .Include(r => r.educations)
                .Include(r => r.experiences)
                .Include(r => r.skills)
                .Include(r => r.languages)
                .Include(r => r.certificates)
                .Include(r => r.EndUser)
                .FirstOrDefaultAsync(r => r.resumeId == id);
        }

        public async Task<List<Resume>> GetByUserId(string id)
        {
            return await _context.Resumes
                .Include(r => r.educations)
                .Include(r => r.experiences)
                .Include(r => r.skills)
                .Include(r => r.languages)
                .Include(r => r.certificates)
                .Where(r => r.EndUserId == id)
                .ToListAsync();
        }

        public async Task UpdateAsync(Resume resume)
        {

            if (resume == null) throw new ArgumentNullException(nameof(resume));

            var existingResume = await _context.Resumes
                .Include(r => r.educations)
                .Include(r => r.experiences)
                .Include(r => r.skills)
                .Include(r => r.languages)
                .Include(r => r.certificates)
                .FirstOrDefaultAsync(r => r.resumeId == resume.resumeId);

            if (existingResume == null)
                throw new KeyNotFoundException($"Resume with ID {resume.resumeId} not found.");

            _context.Entry(existingResume).CurrentValues.SetValues(resume);
            existingResume.modifiedDate = DateOnly.FromDateTime(DateTime.Now).ToString();


            _context.Educations.RemoveRange(existingResume.educations ?? new List<Education>());
            existingResume.educations = resume.educations ?? new List<Education>();

            _context.Experiences.RemoveRange(existingResume.experiences ?? new List<Experience>());
            existingResume.experiences = resume.experiences ?? new List<Experience>();

            _context.Skills.RemoveRange(existingResume.skills ?? new List<Skill>());
            existingResume.skills = resume.skills ?? new List<Skill>();

            _context.Languages.RemoveRange(existingResume.languages ?? new List<Language>());
            existingResume.languages = resume.languages ?? new List<Language>();

            _context.Certificates.RemoveRange(existingResume.certificates ?? new List<Certificate>());
            existingResume.certificates = resume.certificates ?? new List<Certificate>();

            await _context.SaveChangesAsync();
        }
    }

}