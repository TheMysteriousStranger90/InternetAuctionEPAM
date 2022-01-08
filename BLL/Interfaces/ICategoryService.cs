using System;
using System.Collections.Generic;
using BLL.Models;

namespace BLL.Interfaces
{
    public interface ICategoryService : ICrud<CategoryModel>, IDisposable
    {
        ICollection<CategoryModel> GetAllWithDetails();

        ICollection<LotModel> GetLotByCategoryId(int id);
    }
}
