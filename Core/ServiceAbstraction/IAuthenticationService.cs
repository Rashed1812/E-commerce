using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DataTransferObject.IdentityDtos;

namespace ServiceAbstraction
{
    public interface IAuthenticationService
    {
        //Login 
        //Take Email and Password The Return Token and DisplayName

        Task<UserDto> LoginAsync(LoginDto loginDto);

        //Register
        //Take Email, Password, UserName, DisplayName and PhoneNumber
        //Return Token, Email and DisplayName
        Task<UserDto> RegisterAsync(RegisterDto registerDto);

        //Check Email isvalid and return boolean
        Task<bool> CheckEmailAsync(string email);

        Task<AddressDto> GetCurrentUserAddressAsync(string email);
        //Update User Address
        //Take AddressDto and return boolean
        Task<AddressDto> UpdateCurrentUserAddressAsync(string Email,AddressDto addressDto);
        //Get current User Address
        //Take String Emai and return userDto
        Task<UserDto> GetCurrentUserAsync(string email);

    }
}
