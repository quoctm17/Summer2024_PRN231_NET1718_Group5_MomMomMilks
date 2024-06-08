using BusinessObject.Entities;
using Repository.Interface;

namespace Repository
{
    public class PaymentTypeRepository : IPaymentTypeRepository
    {

        public async Task<List<PaymentType>> GetAllPaymentTypesAsync()
        {
            return await PaymentTypeDAO.Instance.GetAllPaymentTypesAsync();
        }
    }
}
