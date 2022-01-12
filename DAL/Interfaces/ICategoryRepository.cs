using System.Linq;
using DAL.Entities;

namespace DAL.Interfaces
{
    /// <summary>
    /// ICategoryRepository
    /// </summary>
    public interface ICategoryRepository : IRepository<Category>
    {
        IQueryable<Category> GetAllWithDetails();

        IQueryable<Lot> GetLotByCategoryId(int id);
    }
}
