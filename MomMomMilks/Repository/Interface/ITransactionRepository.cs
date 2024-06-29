using BusinessObject.Entities;

namespace Repository.Interface
{
    public interface ITransactionRepository
    {
        Task<List<Transaction>> GetAllTransactionsAsync();
        Task<Transaction> GetTransactionByIdAsync(int id);
        Task<Transaction> GetTransactionByOrderIdAsync(int orderId);
        Task<Transaction> GetTransactionByOrderPaymentCodeAsync(long paymentOrderCode);
        Task AddTransactionAsync(Transaction transaction);
        Task UpdateTransactionAsync(Transaction transaction);
        Task DeleteTransactionAsync(int id);
    }
}
