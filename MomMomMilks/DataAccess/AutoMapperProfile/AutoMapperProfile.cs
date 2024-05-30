using AutoMapper;
using BusinessObject.Entities;
using DataTransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.AutoMapperProfile
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Milk, MilkDTO>()
                .ForMember(dest => dest.Brand, opt => opt.MapFrom(src => src.Brand.Name))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.Supplier, opt => opt.MapFrom(src => src.Supplier.Name))
                .ReverseMap();

            CreateMap<RegisterDTO, AppUser>();
            CreateMap<Order, OrderDTO>()
                .ReverseMap();
            CreateMap<Shipper, ShipperDTO>()
                .ReverseMap();
            CreateMap<Order, OrderHistoryDTO>()
                .ForMember(dest => dest.Shipper, opt => opt.MapFrom(src => src.Shipper.UserName))
                .ForMember(dest => dest.PaymentType, opt => opt.MapFrom(src => src.PaymentType.Name))
                .ReverseMap();
        }
    }
}
