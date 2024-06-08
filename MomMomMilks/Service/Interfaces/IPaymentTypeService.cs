using BusinessObject.Entities;

namespace Service.Interfaces
{
    public interface IPaymentTypeService
    {
        Task<List<PaymentType>> GetAllPaymentTypesAsync();
    }
}
