using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Models.IdentityModule;
using Domain.Models.Order_Module;
using Domain.Models.ProductModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.Identity;

namespace Persistence
{
    public class DataSeeding(StoreDbContext _DbContext
        ,UserManager<ApplicationUser> _userManager
        ,RoleManager<IdentityRole> _roleManager
        ,StoreIdentityDbContext _identityDbContext) : IDataSeeding
    {
        public async Task DataSeedAsync()
        {
            var PendingMigrations = await _DbContext.Database.GetPendingMigrationsAsync();
            if (PendingMigrations.Any())
               await _DbContext.Database.MigrateAsync();
            try
            {
                #region Add Data Seeding Locally In DB
                if (!_DbContext.Set<ProductBrand>().Any())
                {
                    //var ProductsBrandData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\DataSeed\brands.json");
                    var ProductsBrandData = File.OpenRead(@"..\Infrastructure\Persistence\Data\DataSeed\brands.json");
                    //Convert data from json to C# object
                    var ProductBrands = await JsonSerializer.DeserializeAsync<List<ProductBrand>>(ProductsBrandData);
                    if (ProductBrands is not null && ProductBrands.Any())
                       await _DbContext.ProductBrands.AddRangeAsync(ProductBrands);
                }
                if (!_DbContext.Set<ProductType>().Any())
                {
                    var ProductsTypeData = File.OpenRead(@"..\Infrastructure\Persistence\Data\DataSeed\types.json");
                    //Convert data from json to C# object
                    var ProductTypes = await JsonSerializer.DeserializeAsync<List<ProductType>>(ProductsTypeData);
                    if (ProductTypes is not null && ProductTypes.Any())
                        await _DbContext.ProductTypes.AddRangeAsync(ProductTypes);
                }
                if (!_DbContext.Set<Product>().Any())
                {
                    var ProductsData = File.OpenRead(@"..\Infrastructure\Persistence\Data\DataSeed\products.json");
                    //Convert data from json to C# object
                    var Products = await JsonSerializer.DeserializeAsync<List<Product>>(ProductsData);
                    if (Products is not null && Products.Any())
                       await _DbContext.Products.AddRangeAsync(Products);
                }
                if (!_DbContext.Set<DeliveryMethod>().Any())
                {
                    var DeliveryMethodData = File.OpenRead(@"..\Infrastructure\Persistence\Data\DataSeed\delivery.json");
                    //Convert data from json to C# object
                    var DeliveryMethod = await JsonSerializer.DeserializeAsync<List<DeliveryMethod>>(DeliveryMethodData);
                    if (DeliveryMethod is not null && DeliveryMethod.Any())
                        await _DbContext.Set<DeliveryMethod>().AddRangeAsync(DeliveryMethod);
                }
                #endregion
                await _DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task IdentityDataSeedAsync()
        {
            try
            {
                if (!_roleManager.Roles.Any())
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                }
                if (!_userManager.Users.Any())
                {
                    var user01 = new ApplicationUser()
                    {
                        Email = "Abdelrahma@gmail.com",
                        DisplayName = "Abdelrahman Rashed",
                        PhoneNumber = "01011845887",
                        UserName = "AbdelrahmaRashed"
                    };
                    var user02 = new ApplicationUser()
                    {
                        Email = "Salma@gmail.com",
                        DisplayName = "Salma Mohamed",
                        PhoneNumber = "01011845887",
                        UserName = "SalmaMohamed"
                    };
                    await _userManager.CreateAsync(user01, "P@ssw0rd");
                    await _userManager.CreateAsync(user02, "P@ssw0rd");

                    _userManager.AddToRoleAsync(user01, "SuperAdmin");
                    _userManager.AddToRoleAsync(user02, "Admin");
                }

                await _identityDbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {

            }
        }
    }
}
