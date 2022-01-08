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
    public class CategoriesService : ICategoriesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoriesService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<CategoryModel> GetAll()
        {
            var categories = _unitOfWork.CategoryRepository.FindAll();
            if (categories == null) throw new InternetAuctionException("Categories not found!");
            var result = (from b in categories
                select _mapper.Map<Category, CategoryModel>(b)).ToList();
            return result;
        }

        public async Task<CategoryModel> GetByIdAsync(int id)
        {
            if (_unitOfWork.CategoryRepository.FindAll().FirstOrDefault(x => x.Id == id) == null) throw new InternetAuctionException("Category not found!");
            var result = _mapper.Map<Category, CategoryModel>(_unitOfWork.CategoryRepository.GetByIdAsync(id).Result);
            return await Task.FromResult<CategoryModel>(result);
        }

        public async Task AddAsync(CategoryModel model)
        {
            if (_unitOfWork.CategoryRepository.FindAll().FirstOrDefault(x => x.Id == model.Id) != null) throw new InternetAuctionException("Category already exist!");
            try
            {
                Category _model = _mapper.Map<Category>(model);
                await _unitOfWork.CategoryRepository.AddAsync(_model);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex)
            {
                throw new InternetAuctionException(ex.Message);
            }
        }

        public async Task UpdateAsync(CategoryModel model)
        {
            if (_unitOfWork.CategoryRepository.FindAll().FirstOrDefault(x => x.Id == model.Id) == null) throw new InternetAuctionException("Category not found!");

            _unitOfWork.CategoryRepository.Update(_mapper.Map<CategoryModel, Category>(model));
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteByIdAsync(int modelId)
        {
            if (_unitOfWork.CategoryRepository.FindAll().FirstOrDefault(x => x.Id == modelId) == null) throw new InternetAuctionException("Category not found!");
            await _unitOfWork.CategoryRepository.DeleteByIdAsync(modelId);
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

        public ICollection<CategoryModel> GetAllWithDetails()
        {
            var categories = _unitOfWork.CategoryRepository.FindAll();
            if (categories == null) throw new InternetAuctionException("Categories not found!");
            var result = _unitOfWork.CategoryRepository.GetAllWithDetails()
                .Select(x => _mapper.Map<Category, CategoryModel>(x)).ToList();
            return result;
        }

        public ICollection<LotModel> GetLotByCategoryId(int id)
        {
            if (_unitOfWork.CategoryRepository.FindAll().FirstOrDefault(x => x.Id == id) == null) throw new InternetAuctionException("Category not found!");

            var result =_unitOfWork.CategoryRepository.GetLotByCategoryId(id)
                .Select(x=> _mapper.Map<Lot, LotModel>(x)).ToList();
            return result;
        }
    }
}
