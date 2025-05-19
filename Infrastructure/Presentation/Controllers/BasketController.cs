using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServiceAbstraction;
using Shared.DataTransferObject.BasketModuleDtos;

namespace Presentation.Controllers
{
    public class BasketController(IServiceManager _serviceManager) : ApiController
    {
        //Get
        [HttpGet]
        public async Task<ActionResult<BasketDto>> GetBasket(string key)
        {
            var Basket = await _serviceManager.BasketService.GetBasketAsync(key);
            return Ok(Basket);
        }
        //createorupdate
        [HttpPost]
        public async Task<ActionResult<BasketDto>> CreateOrUpdateBasket(BasketDto basket)
        {
            var Basket = await _serviceManager.BasketService.CreateOrUpdateBasketAsync(basket);
            return Ok(Basket);
        }
        //delete
        [HttpDelete("{Key}")]
        public async Task<ActionResult<bool>> DeleteBasket(string key)
        {
            var Result = await _serviceManager.BasketService.DeleteBasketAsync(key);
            return Ok(Result);
        }
    }
}
