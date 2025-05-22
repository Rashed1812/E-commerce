using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Order_Module
{
    public enum OrderStatus
    {
        Pending = 0,
        PaymentReceived = 1,
        PaymentRejected = 2
    }
}
