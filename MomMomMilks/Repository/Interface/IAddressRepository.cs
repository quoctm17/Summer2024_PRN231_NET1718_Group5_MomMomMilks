using BusinessObject.Entities;

namespace Repository.Interface
{
    public interface IAddressRepository
    {
        Task<List<Address>> GetAllAddressesAsync();
        Task<Address> GetAddressByIdAsync(int addressId);
        Task AddAddressAsync(Address address);
        Task UpdateAddressAsync(Address address);
        Task DeleteAddressAsync(int addressId);
    }
}
