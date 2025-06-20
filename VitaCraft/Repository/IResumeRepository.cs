using VitaCraft.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using VitaCraft.Models;

namespace VitaCraft.Repository
{
    public interface IResumeRepository
    {
        Task<Resume> AddAsync(Resume resume);
        Task<Resume> GetByIdAsync(int id);
        Task<List<Resume>> GetByUserId(string id);
         Task<List<Resume>> GetAllAsync();
        Task UpdateAsync(Resume resume);
        Task DeleteAsync(int id);
    }
}
