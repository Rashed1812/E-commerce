using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models.Order_Module;

namespace Service.Specifications
{
    public class OrderSpecifications : BaseSpecifications<Order,Guid>
    {
        public OrderSpecifications(string email):base(o=>o.UserEmail == email)
        {
            AddIncludes(o => o.DeliveryMethod);
            AddIncludes(o => o.Items);
        }
        public OrderSpecifications(Guid id) : base(o => o.Id == id)
        {
            AddIncludes(o => o.DeliveryMethod);
            AddIncludes(o => o.Items);
        }
    }
}
