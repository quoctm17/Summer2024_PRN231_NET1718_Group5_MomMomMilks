using BusinessObject.Entities;
using DataAccess.DAO;
using Repository.Interface;

namespace Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        public async Task<List<Transaction>> GetAllTransactionsAsync()
        {
            return await TransactionDAO.Instance.GetAllTransactions();
        }

        public async Task<Transaction> GetTransactionByIdAsync(int id)
        {
            return await TransactionDAO.Instance.GetTransactionById(id);
        }

        public async Task<Transaction> GetTransactionByOrderIdAsync(int orderId)
        {
            return await TransactionDAO.Instance.GetTransactionByOrderIdAsync(orderId);
        }

        public async Task<Transaction> GetTransactionByOrderPaymentCodeAsync(long paymentOrderCode)
        {
            return await TransactionDAO.Instance.GetTransactionByOrderPaymentCodeAsync(paymentOrderCode);
        }


        public async Task AddTransactionAsync(Transaction transaction)
        {
            await TransactionDAO.Instance.AddTransaction(transaction);
        }

        public async Task UpdateTransactionAsync(Transaction transaction)
        {
            await TransactionDAO.Instance.UpdateTransaction(transaction);
        }

        public async Task DeleteTransactionAsync(int id)
        {
            await TransactionDAO.Instance.DeleteTransaction(id);
        }
    }
}
