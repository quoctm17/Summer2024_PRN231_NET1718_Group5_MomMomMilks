using BusinessObject.Entities;
using DataAccess.DAO;
using Repository.Interface;

namespace Repository
{
    public class AddressRepository : IAddressRepository
    {
        private readonly AddressDAO _addressDAO;

        public AddressRepository()
        {
            _addressDAO = AddressDAO.Instance;
        }

        public async Task<List<Address>> GetAllAddressesAsync()
        {
            return await _addressDAO.GetAllAddressesAsync();
        }

        public async Task<Address> GetAddressByIdAsync(int addressId)
        {
            return await _addressDAO.GetAddressAsync(addressId);
        }

        public async Task<List<Address>> GetAddressesByUserIdAsync(int userId)
        {
            return await AddressDAO.Instance.GetAddressesByUserIdAsync(userId);
        }

        public async Task AddAddressAsync(Address address)
        {
            await _addressDAO.AddAddressAsync(address);
        }

        public async Task UpdateAddressAsync(Address address)
        {
            await _addressDAO.UpdateAddressAsync(address);
        }

        public async Task DeleteAddressAsync(int addressId)
        {
            await _addressDAO.DeleteAddressAsync(addressId);
        }
    }
}
