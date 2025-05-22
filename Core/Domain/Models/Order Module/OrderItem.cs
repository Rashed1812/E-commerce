﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Order_Module
{
    public class OrderItem : BaseEntity<int>
    {
        public ProductItemOrded Product { get; set; } = default!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }

    }
}
