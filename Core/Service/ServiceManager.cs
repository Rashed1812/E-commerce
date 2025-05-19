using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Models.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using ServiceAbstraction;

namespace Service
{
    public class ServiceManager(IUnitOfWork _unitOfWork,IMapper _mapper,IBasketRepository basketRepository,UserManager<ApplicationUser> userManager,IConfiguration _configuration) : IServiceManager
    {
        private readonly Lazy<IProductService> _LazyProductService = new Lazy<IProductService>(() => new ProductService(_unitOfWork,_mapper));
        private readonly Lazy<IBasketService> _LazyBasketService = new Lazy<IBasketService>(() => new BasketService(basketRepository, _mapper));
        private readonly Lazy<IAuthenticationService> _LazyAuthenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(userManager, _configuration , _mapper));
        public IProductService ProductService => _LazyProductService.Value;

        public IBasketService BasketService => _LazyBasketService.Value;

        public IAuthenticationService AuthenticationService => _LazyAuthenticationService.Value;
    }
}
