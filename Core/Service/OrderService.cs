using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.Models.Order_Module;
using Domain.Models.ProductModule;
using Service.Specifications;
using ServiceAbstraction;
using Shared.DataTransferObject.IdentityDtos;
using Shared.DataTransferObject.OrderDtos;

namespace Service
{
    public class OrderService(IMapper _mapper, IBasketRepository _basketRepository, IUnitOfWork _unitOfWork) : IOrderService
    {
        public async Task<OrderToReturnDto> CreateOrder(OrderDto orderDto, string Email)
        {
            //Mapping address to OrderAddress
            var OrderAddress = _mapper.Map<AddressDto, OrderAddress>(orderDto.Address);

            //Get Basket
            var Basket = await _basketRepository.GetBasketAsync(orderDto.BasketId) ?? throw new BasketNotFoundException(orderDto.BasketId);

            //create order Items List
            List<OrderItem> OrderItems = [];
            var ProductRepo = _unitOfWork.GetRepository<Product, int>();
            foreach (var item in Basket.Items)
            {
                var Product = await ProductRepo.GetByIdAsync(item.Id)
                    ?? throw new ProductNotFoundException(item.Id);
                var OrderItem = new OrderItem
                {
                    Product = new ProductItemOrded()
                    {
                        ProductId = Product.Id,
                        ProductName = Product.Name,
                        PictureUrl = Product.PictureUrl
                    },
                    Price = Product.Price,
                    Quantity = item.Quantity
                };
                OrderItems.Add(OrderItem);
            }
            //Get DelivryMethod
            var DeliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(orderDto.DeliveryMethodId)
                ?? throw new DeliveryMethodNotFoundException(orderDto.DeliveryMethodId);
            //Calculate SubTotal
            var SubTotal = OrderItems.Sum(item => item.Price * item.Quantity);

            //Create Order
            var Order = new Order(Email, OrderAddress, DeliveryMethod, OrderItems, SubTotal);
            await _unitOfWork.GetRepository<Order, Guid>().AddAsync(Order);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<Order, OrderToReturnDto>(Order);
        }

        public async Task<IEnumerable<DeliveryMethodDto>> GetDeliveryMethodsAsync()
        {
            var DelivryMethods = await _unitOfWork.GetRepository<DeliveryMethod,int>().GetAllAsync();
            return  _mapper.Map<IEnumerable<DeliveryMethod>,IEnumerable<DeliveryMethodDto>>(DelivryMethods);
        }
        public async Task<IEnumerable<OrderToReturnDto>> GetAllOrdersAsync(string Email)
        {
            var Specification = new OrderSpecifications(Email);
            var Orders = await _unitOfWork.GetRepository<Order, Guid>().GetAllAsync(Specification);
            return _mapper.Map<IEnumerable<Order>, IEnumerable<OrderToReturnDto>>(Orders);
        }

        public async Task<OrderToReturnDto> GetOrderByIdAsync(Guid id)
        {
            var Specification = new OrderSpecifications(id);
            var Order = await _unitOfWork.GetRepository<Order, Guid>().GetByIdAsync(Specification);
            return _mapper.Map<Order, OrderToReturnDto>(Order);
        }
    }
}
