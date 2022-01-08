using System.Linq;
using DAL.Entities;

namespace DAL.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        IQueryable<Category> GetAllWithDetails();

        IQueryable<Lot> GetLotByCategoryId(int id);
    }
}
