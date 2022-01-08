using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL.Exceptions;
using BLL.Interfaces;
using BLL.Models;
using DAL.Entities;
using DAL.Interfaces;

namespace BLL.Services
{
    public class LotService : ILotService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LotService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<LotModel> GetAll()
        {
            var lots = _unitOfWork.LotRepository.FindAll();
            if (lots == null) throw new InternetAuctionException("Lots not found!");
            var result = (from b in lots
                select _mapper.Map<Lot, LotModel>(b)).ToList();
            return result;
        }

        public async Task<LotModel> GetByIdAsync(int id)
        {
            if (_unitOfWork.LotRepository.FindAll().FirstOrDefault(x => x.Id == id) == null) throw new InternetAuctionException("Lot not found!");
            var result = _mapper.Map<Lot, LotModel>(_unitOfWork.LotRepository.GetByIdAsync(id).Result);
            return await Task.FromResult<LotModel>(result);
        }

        public async Task AddAsync(LotModel model)
        {
            if (_unitOfWork.LotRepository.FindAll().FirstOrDefault(x => x.Id == model.Id) != null) throw new InternetAuctionException("Lot already exist!");
            try
            {
                Lot _model = _mapper.Map<Lot>(model);
                _model.Category = _unitOfWork.CategoryRepository.GetByIdAsync(model.CategoryId).Result;
                await _unitOfWork.LotRepository.AddAsync(_model);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new InternetAuctionException(ex.Message);
            }
        }

        public async Task UpdateAsync(LotModel model)
        {
            if (_unitOfWork.LotRepository.FindAll().FirstOrDefault(x => x.Id == model.Id) == null) throw new InternetAuctionException("Lot not found!");
            _unitOfWork.LotRepository.Update(_mapper.Map<Lot>(model));
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteByIdAsync(int modelId)
        {
            if (_unitOfWork.LotRepository.FindAll().FirstOrDefault(x => x.Id == modelId) == null) throw new InternetAuctionException("Lot not found!");
            await _unitOfWork.LotRepository.DeleteByIdAsync(modelId);
            await _unitOfWork.SaveAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _unitOfWork.Dispose();
                }
            }

            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public LotModel GetWithDetailsById(int id)
        {
            if (_unitOfWork.LotRepository.FindAll().FirstOrDefault(x => x.Id == id) == null) throw new InternetAuctionException("Lot not found!");
            return _mapper.Map<LotModel>(_unitOfWork.LotRepository.GetByIdWithDetailsAsync(id));
        }

        public async Task<List<LotModel>> GetLotsByUserIdAsync(string userId)
        {
            var listLot = await _unitOfWork.LotRepository.GetLotsByUserIdAsync(userId);
            if (listLot == null) throw new InternetAuctionException("Lots not found!");
            return _mapper.Map<List<LotModel>>(listLot);
        }

        public async Task<List<LotModel>> GetSoldLotsAsync()
        {
            var lots = await _unitOfWork.LotRepository.GetSoldLotsAsync();
            if (lots == null) throw new InternetAuctionException("Lots not found!");
            return _mapper.Map<List<LotModel>>(lots);
        }
    }
}
