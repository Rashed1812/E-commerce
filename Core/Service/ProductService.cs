using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.Models.ProductModule;
using Service.Specifications;
using ServiceAbstraction;
using Shared;
using Shared.DataTransferObject.ProductModuleDtos;

namespace Service
{
    public class ProductService(IUnitOfWork _unitOfWork , IMapper _mapper) : IProductService
    {
        public async Task<IEnumerable<BrandDto>> GetAllBrandsAsync()
        {
            var repo = _unitOfWork.GetRepository<ProductBrand,int>();
            var brands =await repo.GetAllAsync();
            var BrandsDto = _mapper.Map<IEnumerable<ProductBrand>, IEnumerable<BrandDto>>(brands);
            return BrandsDto;
        }

        public async Task<PaginationResponse<ProductDto>> GetAllProductsAsync(ProductQueryParameters queryParameters)
        {
            var specification = new ProductWithTypeAndBrandSpecifications(queryParameters);
            var repo = _unitOfWork.GetRepository<Product, int>();
            var products = await repo.GetAllAsync(specification);
            var Data = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products);
            var ProductCount = products.Count();
            var countSpec = new ProductCountSpecification(queryParameters);
            var totalCount = await repo.CountAsync(countSpec);
            return new PaginationResponse<ProductDto>(queryParameters.PageIndex, ProductCount,totalCount,Data);

        }

        public async Task<IEnumerable<TypeDto>> GetAllTypesAsync()
        {
            var types = await _unitOfWork.GetRepository<ProductType, int>().GetAllAsync();
            return _mapper.Map<IEnumerable<ProductType>, IEnumerable<TypeDto>>(types);
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            var specification = new ProductWithTypeAndBrandSpecifications(id);
            var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(specification);
            if (product is null)
            {
                throw new ProductNotFoundException(id);
            }
            return _mapper.Map<Product, ProductDto>(product);
        }
    }
}
