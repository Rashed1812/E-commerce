using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;
using Shared.DataTransferObject;

namespace ServiceAbstraction
{
    public interface IProductService
    {
        //Get All Products
        Task<PaginationResponse<ProductDto>> GetAllProductsAsync(ProductQueryParameters queryParameters);
        //Get Product By Id
        Task<ProductDto> GetProductByIdAsync(int id);
        //Get All Brands
        Task<IEnumerable<BrandDto>> GetAllBrandsAsync();
        //Get All Types
        Task<IEnumerable<TypeDto>> GetAllTypesAsync();

    }
}
