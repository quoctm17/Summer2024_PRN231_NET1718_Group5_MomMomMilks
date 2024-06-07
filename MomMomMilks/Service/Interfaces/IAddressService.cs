using BusinessObject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IAddressService
    {
        Task<List<Address>> GetAllAddressesAsync();
        Task<Address> GetAddressByIdAsync(int addressId);
        Task<List<Address>> GetAddressesByUserIdAsync(int userId);
        Task AddAddressAsync(Address address);
        Task UpdateAddressAsync(Address address);
        Task DeleteAddressAsync(int addressId);
        
    }
}
