using BusinessObject.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class PaymentTypeDAO
    {
        private readonly AppDbContext _context;

        private static PaymentTypeDAO instance;

        public PaymentTypeDAO()
        {
            _context = new AppDbContext();
        }

        public static PaymentTypeDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PaymentTypeDAO();
                }
                return instance;
            }
        }

        public async Task<List<PaymentType>> GetAllPaymentTypesAsync()
        {
            return await _context.PaymentTypes.AsNoTracking().ToListAsync();
        }
    }
}
