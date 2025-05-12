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
    internal class ProductWithTypeAndBrandSpecifications : BaseSpecifications<Product,int>
    {
        public ProductWithTypeAndBrandSpecifications(int id) : base(product=> product.Id == id)
        {
            //Add Includes
            AddIncludes(p => p.ProductType);
            AddIncludes(p => p.ProductBrand);
        }
        public ProductWithTypeAndBrandSpecifications(ProductQueryParameters queryParameters)
            : base(ApplyCritria(queryParameters))
        {
            AddIncludes(p => p.ProductType);
            AddIncludes(p => p.ProductBrand);

            ApplySorting(queryParameters);
            ApplyPagination(queryParameters.PageSize, queryParameters.PageIndex);
        }

        private static Expression<Func<Product, bool>> ApplyCritria(ProductQueryParameters queryParameters)
        {
            return product =>
                        (!queryParameters.BrandId.HasValue || product.BrandId == queryParameters.BrandId.Value) &&
                        (!queryParameters.TypeId.HasValue || product.TypeId == queryParameters.TypeId.Value) &&
                        (string.IsNullOrEmpty(queryParameters.Search) ||
                        product.Name.ToLower().Contains(queryParameters.Search.ToLower()));
        }

        private void ApplySorting(ProductQueryParameters queryParameters)
        {
            switch (queryParameters.Options)
            {
                case ProductSortingOptions.NameAsc:
                    AddOrderBy(p => p.Name);
                    break;
                case ProductSortingOptions.NameDesc:
                    AddOrderByDescending(p => p.Name);
                    break;
                case ProductSortingOptions.PriceAsc:
                    AddOrderBy(p => p.Price);
                    break;
                case ProductSortingOptions.PriceDesc:
                    AddOrderByDescending(p => p.Price);
                    break;
                default:
                    AddOrderBy(p => p.Name);
                    break;
            }
        }
    }
   
}
