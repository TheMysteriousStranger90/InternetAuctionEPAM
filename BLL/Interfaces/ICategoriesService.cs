using System;
using System.Collections.Generic;
using BLL.Models;

namespace BLL.Interfaces
{
    public interface ICategoriesService : ICrud<CategoryModel>, IDisposable
    {
        ICollection<CategoryModel> GetAllWithDetails();

        ICollection<LotModel> GetLotByCategoryId(int id);
    }
}
