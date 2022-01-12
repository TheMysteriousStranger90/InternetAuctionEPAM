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
    /// <summary>
    /// Trade Service class
    /// </summary>
    public class TradeService : ITradeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TradeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<TradeModel> GetAll()
        {
            var trades = _unitOfWork.TradeRepository.FindAll();
            if (trades == null) throw new InternetAuctionException("Trades not found!");
            var result = (from b in trades
                select _mapper.Map<Trade, TradeModel>(b)).ToList();
            return result;
        }

        public async Task<TradeModel> GetByIdAsync(int id)
        {
            if (_unitOfWork.TradeRepository.FindAll().FirstOrDefault(x => x.Id == id) == null) throw new InternetAuctionException("Trades not found!");
            var result = _mapper.Map<Trade, TradeModel>(_unitOfWork.TradeRepository.GetByIdAsync(id).Result);
            return await Task.FromResult<TradeModel>(result);
        }

        public async Task AddAsync(TradeModel model)
        {
            if (_unitOfWork.TradeRepository.FindAll().FirstOrDefault(x => x.Id == model.Id) != null) throw new InternetAuctionException("Trades already exist!");
            var lot = await _unitOfWork.LotRepository.GetByIdAsync(model.LotId);
            if (lot is null)
                throw new InternetAuctionException($"Lot with Id = {model.LotId} does not exist");
            var trade = _mapper.Map<Trade>(model);
            _unitOfWork.TradeRepository.AddAsync(trade);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateAsync(TradeModel model)
        {
            if (_unitOfWork.TradeRepository.FindAll().FirstOrDefault(x => x.Id == model.Id) == null) throw new InternetAuctionException("Trades not found!");
            _unitOfWork.TradeRepository.Update(_mapper.Map<Trade>(model));
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteByIdAsync(int modelId)
        {
            if (_unitOfWork.TradeRepository.FindAll().FirstOrDefault(x => x.Id == modelId) == null) throw new InternetAuctionException("Trades not found!");
            await _unitOfWork.TradeRepository.DeleteByIdAsync(modelId);
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
    }
}
