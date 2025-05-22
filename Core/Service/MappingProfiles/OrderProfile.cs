using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Models.IdentityModule;
using Domain.Models.Order_Module;
using Shared.DataTransferObject.IdentityDtos;
using Shared.DataTransferObject.OrderDtos;

namespace Service.MappingProfiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile() 
        {
            CreateMap<AddressDto, OrderAddress>().ReverseMap();
            CreateMap<Order,OrderToReturnDto>()
                .ForMember(d=>d.DeliveryMethod , o=>o.MapFrom(s=>s.DeliveryMethod.ShortName));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.ProductName))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<OrderItemPictureUrlResolver>());
            CreateMap<DeliveryMethod, DeliveryMethodDto>();
        }
    }
}
