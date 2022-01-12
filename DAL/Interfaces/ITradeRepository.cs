using System.Linq;
using System.Threading.Tasks;
using DAL.Entities;

namespace DAL.Interfaces
{
    /// <summary>
    /// ITradeRepository
    /// </summary>
    public interface ITradeRepository : IRepository<Trade>
    {
        IQueryable<Trade> FindAllWithDetails();

        Task<Trade> GetByIdWithDetailsAsync(int id);
    }
}
