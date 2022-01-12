using System;
using System.Collections.Generic;
using BLL.Models;

namespace BLL.Interfaces
{
    /// <summary>
    /// ICategoriesService
    /// </summary>
    public interface ICategoriesService : ICrud<CategoryModel>, IDisposable
    {
        ICollection<CategoryModel> GetAllWithDetails();

        ICollection<LotModel> GetLotByCategoryId(int id);
    }
}
