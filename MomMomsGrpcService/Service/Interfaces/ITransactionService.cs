using BusinessObject.Entities;

namespace Service.Interfaces
{
    public interface ITransactionService
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
