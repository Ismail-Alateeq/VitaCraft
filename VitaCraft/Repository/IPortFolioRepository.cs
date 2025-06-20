using VitaCraft.Models.DTOs;
using VitaCraft.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VitaCraft.Repository
{
    public interface IPortFolioRepository
    {
    

        Task<PortFolio> AddAsync(PortFolio portFolio);
        Task<PortFolio> GetByIdAsync(int id);
        Task<List<PortFolio>> GetByUserId(string id);
        Task<List<PortFolio>> GetAllAsync();

        Task UpdateAsync(PortFolio portFolio);
        Task DeleteAsync(int id);
        
    }
}
