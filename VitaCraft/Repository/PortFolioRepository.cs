using VitaCraft.Data;
using VitaCraft.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace VitaCraft.Repository
{
    public class PortFolioRepository : IPortFolioRepository
    {
        private readonly ApplicationDbContext _context;
        public PortFolioRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PortFolio> AddAsync(PortFolio portFolio)
        {
            try
            {
                _context.PortFolios.Add(portFolio);
                await _context.SaveChangesAsync();
                return portFolio;



            }
            catch (Exception ex)
            {
                throw new Exception($"there is an error{ex}");
            }


        }

        public async Task DeleteAsync(int id)
        {
            var portFolio = await _context.PortFolios
                .Include(p => p.projects)
                .Include(p => p.skills)
                .Include(p => p.services)
                .FirstOrDefaultAsync(p => p.portFolioId == id);

            if (portFolio == null)
                throw new KeyNotFoundException($"Portfolio with ID {id} not found.");

            if (portFolio.projects != null)
                _context.Projectts.RemoveRange(portFolio.projects);
            if (portFolio.skills != null)
                _context.Skills.RemoveRange(portFolio.skills);
            if (portFolio.services != null)
                _context.Services.RemoveRange(portFolio.services);

            _context.PortFolios.Remove(portFolio);
            await _context.SaveChangesAsync();

        }
        public async Task<List<PortFolio>> GetAllAsync()
        {
            return await _context.PortFolios.ToListAsync();
        }

        public async Task<PortFolio> GetByIdAsync(int id)
        {
            return await _context.PortFolios
                .Include(p => p.services)
                .Include(p => p.projects)
                .Include(p => p.skills)
                .Include(p => p.EndUser)
                .FirstOrDefaultAsync(p => p.portFolioId == id);
        }

        public async Task<List<PortFolio>> GetByUserId(string id)
        {
            return await _context.PortFolios
                .Include(p => p.services)
                .Include(p => p.projects)
                .Include(p => p.skills)
                .Include(p => p.EndUser)
                .Where(p => p.EndUserId == id).ToListAsync();
        }

        public async Task UpdateAsync(PortFolio portFolio)
        {
            if (portFolio == null)
                throw new ArgumentNullException(nameof(portFolio), "Portfolio cannot be null");

            var existingPortfolio = await _context.PortFolios
                .Include(p => p.services)
                .Include(p => p.projects)
                .Include(p => p.skills)
                .FirstOrDefaultAsync(p => p.portFolioId == portFolio.portFolioId);

            if (existingPortfolio == null)
                throw new KeyNotFoundException($"Portfolio with ID {portFolio.portFolioId} not found.");

            // Update main properties
            _context.Entry(existingPortfolio).CurrentValues.SetValues(portFolio);
            existingPortfolio.modifiedDate = DateOnly.FromDateTime(DateTime.Now).ToString();

            // Remove all existing children
            _context.Services.RemoveRange(existingPortfolio.services ?? new List<Service>());
            _context.Projectts.RemoveRange(existingPortfolio.projects ?? new List<Projectt>());
            _context.Skills.RemoveRange(existingPortfolio.skills ?? new List<Skill>());

            // Add the new children (if any)
            existingPortfolio.services = portFolio.services ?? new List<Service>();
            existingPortfolio.projects = portFolio.projects ?? new List<Projectt>();
            existingPortfolio.skills = portFolio.skills ?? new List<Skill>();

            await _context.SaveChangesAsync();
        }
    }
}