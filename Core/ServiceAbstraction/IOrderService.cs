using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DataTransferObject.OrderDtos;

namespace ServiceAbstraction
{
    public interface IOrderService
    {
        //Create Order
        public Task<OrderToReturnDto> CreateOrder(OrderDto orderDto,string Email);

        //Get Delivery Methods
        public Task<IEnumerable<DeliveryMethodDto>> GetDeliveryMethodsAsync();
        //Get All Orders
        public Task<IEnumerable<OrderToReturnDto>> GetAllOrdersAsync(string Email);
        //Get Order By Id
        public Task<OrderToReturnDto> GetOrderByIdAsync(Guid id);
    }
}
