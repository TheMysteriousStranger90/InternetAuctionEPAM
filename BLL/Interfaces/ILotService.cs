using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Models;

namespace BLL.Interfaces
{
    /// <summary>
    /// ILotService
    /// </summary>
    public interface ILotService : ICrud<LotModel>, IDisposable
    {
        LotModel GetWithDetailsById(int id);

        Task<List<LotModel>> GetLotsByUserIdAsync(string userId);

        Task<List<LotModel>> GetSoldLotsAsync();
    }
}
