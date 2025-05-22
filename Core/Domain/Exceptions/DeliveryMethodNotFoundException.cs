using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public sealed class DeliveryMethodNotFoundException(int deliveryMethodId) : NotFoundException($"Delivery method with id: {deliveryMethodId} doesn't exist in the database.")
    {
    }
}
