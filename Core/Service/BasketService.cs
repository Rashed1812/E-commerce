using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Exceptions;
using Domain.Models.BasketModule;
using ServiceAbstraction;
using Shared.DataTransferObject.BasketModuleDtos;

namespace Service
{
    public class BasketService(IBasketRepository _basketRepository, IMapper mapper) : IBasketService
    {
        public async Task<BasketDto> CreateOrUpdateBasketAsync(BasketDto basket)
        {
            var CustomerBasket = mapper.Map<BasketDto, CustomerBasket>(basket);
           var CreateOrUpdateBasket = await _basketRepository.CreateOrUpdateBasketAsync(CustomerBasket);
            if (CreateOrUpdateBasket != null)
                return await GetBasketAsync(basket.Id);
            else
                throw new Exception("Can't Create Or Update Basket Now!");
        }

        public async Task<bool> DeleteBasketAsync(string key)
        {
            return await _basketRepository.DeleteBasketAsync(key);
        }

        public async Task<BasketDto> GetBasketAsync(string key)
        {
            var Basket = await _basketRepository.GetBasketAsync(key);
            if (Basket != null)
                return mapper.Map<CustomerBasket, BasketDto>(Basket);
            else
                throw new BasketNotFoundException(key);
        }
    }
}
