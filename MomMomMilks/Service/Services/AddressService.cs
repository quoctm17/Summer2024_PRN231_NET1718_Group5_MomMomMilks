using BusinessObject.Entities;
using Repository.Interface;
using Service.Interfaces;

namespace Service.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;

        public AddressService(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public async Task<List<Address>> GetAllAddressesAsync()
        {
            return await _addressRepository.GetAllAddressesAsync();
        }

        public async Task<Address> GetAddressByIdAsync(int addressId)
        {
            return await _addressRepository.GetAddressByIdAsync(addressId);
        }

        public async Task<List<Address>> GetAddressesByUserIdAsync(int userId)
        {
            return await _addressRepository.GetAddressesByUserIdAsync(userId);
        }

        public async Task AddAddressAsync(Address address)
        {
            await _addressRepository.AddAddressAsync(address);
        }

        public async Task UpdateAddressAsync(Address address)
        {
            await _addressRepository.UpdateAddressAsync(address);
        }

        public async Task DeleteAddressAsync(int addressId)
        {
            await _addressRepository.DeleteAddressAsync(addressId);
        }
    }
}
