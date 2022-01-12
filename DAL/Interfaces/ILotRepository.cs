using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Entities;

namespace DAL.Interfaces
{
    /// <summary>
    /// ILotRepository
    /// </summary>
    public interface ILotRepository : IRepository<Lot>
    {
        IQueryable<Lot> GetAllWithDetails();

        Task<Lot> GetByIdWithDetailsAsync(int id);

        Task<List<Lot>> GetSoldLotsAsync();

        Task<List<Lot>> GetLotsByUserIdAsync(string userId);
    }
}
