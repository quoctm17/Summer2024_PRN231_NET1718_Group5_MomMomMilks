using BusinessObject.Entities;

namespace Repository.Interface
{
    public interface IPaymentTypeRepository
    {
        Task<List<PaymentType>> GetAllPaymentTypesAsync();
    }
}
