using BusinessObject.Entities;
using Repository.Interface;
using Service.Interfaces;

namespace Service.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<List<Transaction>> GetAllTransactionsAsync()
        {
            return await _transactionRepository.GetAllTransactionsAsync();
        }

        public async Task<Transaction> GetTransactionByIdAsync(int id)
        {
            return await _transactionRepository.GetTransactionByIdAsync(id);
        }

        public async Task<Transaction> GetTransactionByOrderIdAsync(int orderId)
        {
            return await _transactionRepository.GetTransactionByOrderIdAsync(orderId);
        }

        public async Task<Transaction> GetTransactionByOrderPaymentCodeAsync(long paymentOrderCode)
        {
            return await _transactionRepository.GetTransactionByOrderPaymentCodeAsync(paymentOrderCode);
        }

        public async Task AddTransactionAsync(Transaction transaction)
        {
            await _transactionRepository.AddTransactionAsync(transaction);
        }

        public async Task UpdateTransactionAsync(Transaction transaction)
        {
            await _transactionRepository.UpdateTransactionAsync(transaction);
        }

        public async Task DeleteTransactionAsync(int id)
        {
            await _transactionRepository.DeleteTransactionAsync(id);
        }


    }
}
