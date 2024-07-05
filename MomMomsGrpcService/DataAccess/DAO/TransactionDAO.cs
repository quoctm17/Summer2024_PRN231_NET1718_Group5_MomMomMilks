using BusinessObject.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DAO
{
    public class TransactionDAO
    {
        private readonly AppDbContext _context;
        private static TransactionDAO instance;

        private TransactionDAO()
        {
            _context = new AppDbContext();
        }

        public static TransactionDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TransactionDAO();
                }
                return instance;
            }
        }

        public async Task<List<Transaction>> GetAllTransactions()
        {
            return await _context.Transactions.Include(t => t.Order).ToListAsync();
        }

        public async Task<Transaction> GetTransactionById(int id)
        {
            return await _context.Transactions.Include(t => t.Order).FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Transaction> GetTransactionByOrderIdAsync(int orderId)
        {
            return await _context.Transactions.FirstOrDefaultAsync(t => t.OrderId == orderId);
        }

        public async Task<Transaction> GetTransactionByOrderPaymentCodeAsync(long paymentOrderCode)
        {
            return await _context.Transactions.FirstOrDefaultAsync(t => t.PaymentOrderCode == paymentOrderCode.ToString());
        }

        public async Task AddTransaction(Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTransaction(Transaction transaction)
        {
            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTransaction(int id)
        {
            var transaction = await GetTransactionById(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
                await _context.SaveChangesAsync();
            }
        }
    }
}
