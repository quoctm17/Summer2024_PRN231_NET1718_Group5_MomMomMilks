using AutoMapper;
using AutoMapper;
using BusinessObject.Entities;
using DataTransfer;
using DataTransfer.Shipper;
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
                .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src => src.OrderStatus.Name))
                .ReverseMap();
            CreateMap<OrderDetail, OrderDetailHistoryDTO>()
                .ForMember(dest => dest.MilkName, opt => opt.MapFrom(src => src.Milk.Name))
                .ReverseMap();
            CreateMap<OrderDetail, ShipperOrderDetailItemDTO>()
                .ForMember(dest => dest.MilkName, opt => opt.MapFrom(src => src.Milk.Name))
                .ReverseMap();
            CreateMap<Order, ShipperOrderDTO>()
                .ForMember(dest => dest.BuyerName, opt => opt.MapFrom(src => src.Buyer.UserName))
                .ForMember(dest => dest.BuyerEmail, opt => opt.MapFrom(src => src.Buyer.Email))
                .ForMember(dest => dest.PaymentTypeName, opt => opt.MapFrom(src => src.PaymentType.Name))
                .ForMember(dest => dest.ScheduleTimeSlot, opt => opt.MapFrom(src => src.Schedule.TimeSlot.Name))
                .ForMember(dest => dest.OrderStatusName, opt => opt.MapFrom(src => src.OrderStatus.Name));
            CreateMap<Order, ShipperOrderDetailDTO>()
                .ForMember(dest => dest.BuyerName, opt => opt.MapFrom(src => src.Buyer.UserName))
                .ForMember(dest => dest.BuyerEmail, opt => opt.MapFrom(src => src.Buyer.Email))
                .ForMember(dest => dest.PaymentTypeName, opt => opt.MapFrom(src => src.PaymentType.Name))
                .ForMember(dest => dest.ScheduleTimeSlot, opt => opt.MapFrom(src => src.Schedule.TimeSlot))
                .ForMember(dest => dest.OrderStatusName, opt => opt.MapFrom(src => src.OrderStatus.Name));
        }
    }
}
