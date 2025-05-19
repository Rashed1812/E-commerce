using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Exceptions;
using Domain.Models.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ServiceAbstraction;
using Shared.DataTransferObject.IdentityDtos;

namespace Service
{
    public class AuthenticationService(UserManager<ApplicationUser> _userManager, IConfiguration _configuration, IMapper _mapper) : IAuthenticationService
    {
        public async Task<bool> CheckEmailAsync(string email)
        {
            var User = await _userManager.FindByEmailAsync(email);
            if (User == null)
                return false;
            return true;
        }
        public async Task<UserDto> GetCurrentUserAsync(string email)
        {
            var User = await _userManager.FindByEmailAsync(email);
            if (User == null)
                throw new UserNotFoundException(email);
            return new UserDto
            {
                Email = email,
                DisplayName = User.DisplayName,
                Token = await CreateTokenAsync(User)
            };
        }
        public async Task<AddressDto> GetCurrentUserAddressAsync(string email)
        {
            var User = await _userManager.Users.Include(u => u.Address)
                .FirstOrDefaultAsync(e => e.Email == email) ?? throw new UserNotFoundException(email);
            if (User.Address is not null)
                return _mapper.Map<AddressDto>(User.Address);
            throw new AddressNotFoundException(User.UserName);


        }

        public async Task<AddressDto> UpdateCurrentUserAddressAsync(string Email, AddressDto addressDto)
        {
            var User = await _userManager.Users.Include(u => u.Address)
                         .FirstOrDefaultAsync(e => e.Email == Email) ?? throw new UserNotFoundException(Email);
            if (User.Address is not null)
            {
                User.Address.FirstName = addressDto.FirstName;
                User.Address.LastName = addressDto.LastName;
                User.Address.City = addressDto.City;
                User.Address.Country = addressDto.Country;
                User.Address.Street = addressDto.Street;
            }
            else
            {
                var Address = _mapper.Map<Address>(addressDto);
                User.Address = Address;
            }
            await _userManager.UpdateAsync(User);
            return _mapper.Map<AddressDto>(User.Address);

        }

        public async Task<UserDto> LoginAsync(LoginDto loginDto)
        {
            //Check if Email Exists
            var User = await _userManager.FindByEmailAsync(loginDto.Email) ??
                throw new UserNotFoundException(loginDto.Email);
            //Check if Password is Correct
            var isPasswordValid = await _userManager.CheckPasswordAsync(User, loginDto.Password);
            if (!isPasswordValid)
                throw new UnAuthorizedException();
            return new UserDto
            {
                Email = User.Email,
                DisplayName = User.DisplayName,
                Token = await CreateTokenAsync(User)
            };

            //Return UserDto 
        }


        public async Task<UserDto> RegisterAsync(RegisterDto registerDto)
        {
            var user = new ApplicationUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                UserName = registerDto.UserName
            };
            //Create User
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (result.Succeeded)
            {
                //Return UserDto
                return new UserDto
                {
                    Email = user.Email,
                    DisplayName = user.DisplayName,
                    Token = await CreateTokenAsync(user)
                };
            }
            else
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                //Return Error
                throw new BadRequestException(errors);
            }
        }


        private async Task<string> CreateTokenAsync(ApplicationUser user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };
            var Roles = await _userManager.GetRolesAsync(user);
            foreach (var role in Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var secretKey = _configuration.GetSection("JWTOptions")["SecretKey"];
            //Symmetric Key Take Byte Array so we use Encoding.UTF8.GetBytes
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //Create Token 
            var token = new JwtSecurityToken(
                issuer: _configuration["JWTOptions:Issuer"],
                audience: _configuration["JWTOptions:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );
            //Return Token as string so we use JwtSecurityTokenHandler 
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
