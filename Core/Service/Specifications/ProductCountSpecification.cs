using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Shared.DataTransferObject;

namespace Service.Specifications
{
    public class ProductCountSpecification :BaseSpecifications<Product,int>
    {
        public ProductCountSpecification(ProductQueryParameters queryParameters) :base(ApplyCritria(queryParameters))
        {
            
        }
        private static Expression<Func<Product, bool>> ApplyCritria(ProductQueryParameters queryParameters)
        {
            return product =>
                        (!queryParameters.BrandId.HasValue || product.BrandId == queryParameters.BrandId.Value) &&
                        (!queryParameters.TypeId.HasValue || product.TypeId == queryParameters.TypeId.Value) &&
                        (string.IsNullOrEmpty(queryParameters.Search) ||
                        product.Name.ToLower().Contains(queryParameters.Search.ToLower()));
        }

    }
}
