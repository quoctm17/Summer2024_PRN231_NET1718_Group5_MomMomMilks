using AutoMapper;
using AutoMapper;
using BusinessObject.Entities;
using DataTransfer;
using DataTransfer.AddressDTOs;
using DataTransfer.Manager;
using DataTransfer.MilkCRUD;
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
                .ForMember(dest => dest.MilkAge, opt => opt.MapFrom(src => $"From {src.MilkAge.Min} to {src.MilkAge.Max}"))
                .ReverseMap();

            CreateMap<RegisterDTO, AppUser>();
            CreateMap<Order, OrderDTO>()
                .ReverseMap();
            CreateMap<Shipper, ShipperDTO>()
                .ReverseMap();
            CreateMap<Batch, CreateBatchDTO>()
                .ReverseMap();
            CreateMap<Coupon, CouponDTO>()
                .ReverseMap();
            CreateMap<CouponUsageHistory, CouponUsageDTO>()
                .ReverseMap();
            CreateMap<Batch, UpdateBatchDTO>()
                .ReverseMap();
            CreateMap<Order, OrderHistoryDTO>()
                .ForMember(dest => dest.Shipper, opt => opt.MapFrom(src => src.Shipper.UserName))
                .ForMember(dest => dest.PaymentType, opt => opt.MapFrom(src => src.PaymentType.Name))
                .ForMember(dest => dest.OrderStatusId, opt => opt.MapFrom(src => src.OrderStatusId))
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
                .ForMember(dest => dest.ScheduleTimeSlot, opt => opt.MapFrom(src => src.TimeSlot.Name))
                .ForMember(dest => dest.OrderStatusName, opt => opt.MapFrom(src => src.OrderStatus.Name));
            CreateMap<Order, ManagerOrderDTO>()
                .ForMember(dest => dest.BuyerName, opt => opt.MapFrom(src => src.Buyer.UserName))
                .ForMember(dest => dest.PaymentTypeName, opt => opt.MapFrom(src => src.PaymentType.Name))
                .ForMember(dest => dest.ScheduleTimeSlot, opt => opt.MapFrom(src => src.TimeSlot.Name))
                .ForMember(dest => dest.OrderStatusName, opt => opt.MapFrom(src => src.OrderStatus.Name));
            CreateMap<Order, ShipperOrderDetailDTO>()
                .ForMember(dest => dest.BuyerName, opt => opt.MapFrom(src => src.Buyer.UserName))
                .ForMember(dest => dest.BuyerEmail, opt => opt.MapFrom(src => src.Buyer.Email))
                .ForMember(dest => dest.PaymentTypeName, opt => opt.MapFrom(src => src.PaymentType.Name))
                .ForMember(dest => dest.ScheduleTimeSlot, opt => opt.MapFrom(src => src.Schedule.TimeSlot))
                .ForMember(dest => dest.OrderStatusName, opt => opt.MapFrom(src => src.OrderStatus.Name));
            CreateMap<Address, AddressDTO>()
                .ForMember(dest => dest.WardName, opt => opt.MapFrom(src => src.Ward.Name))
                .ForMember(dest => dest.DistrictName, opt => opt.MapFrom(src => src.District.Name))
                .ReverseMap();
            CreateMap<Address, AddressCRUD>().ReverseMap();
            CreateMap<Shipper, ManagerShipperDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.AppUser.UserName))
                .ForMember(dest => dest.DistrictName, opt => opt.MapFrom(src => src.District.Name));
            CreateMap<Supplier, SupplierDTO>().ReverseMap();
            CreateMap<Brand, BrandDTO>().ReverseMap();
            CreateMap<MilkAge, MilkAgeDTO>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Milk, ReadMilkDTO>().ReverseMap();
            CreateMap<Milk, CreateMilkDTO>().ReverseMap();
            CreateMap<Milk, UpdateMilkDTO>().ReverseMap();
        }
    }
}
