using System.Linq;
using AutoMapper;
using BLL.Models;
using DAL.Entities;

namespace BLL.Mapping
{
    /// <summary>
    /// Automapper profile class in BLL level
    /// </summary>
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Lot, LotModel>()
                .ForMember(p => p.Id, c => c.MapFrom(x => x.Id))
                .ForMember(p => p.CategoryId, c => c.MapFrom(lot => lot.Category.Id))
                .ReverseMap();

            CreateMap<Category, CategoryModel>()
                .ForMember(p => p.Id, c => c.MapFrom(x => x.Id))
                .ForMember(p => p.LotsId, c => c.MapFrom(lot => lot.Lots.Select(x => x.Id)))
                .ReverseMap();

            CreateMap<Trade, TradeModel>()
                .ForMember(p => p.Id, c => c.MapFrom(lot => lot.Id))
                .ReverseMap();

            CreateMap<User, UserModel>()
                .ForMember(p => p.Id, c => c.MapFrom(user => user.Id))
                //.ForMember(u => u.PurchasedLots, opt => opt.MapFrom(user => user.PurchasedLots.Select(b => b.Id)))
                .ReverseMap();

        }
    }
}
