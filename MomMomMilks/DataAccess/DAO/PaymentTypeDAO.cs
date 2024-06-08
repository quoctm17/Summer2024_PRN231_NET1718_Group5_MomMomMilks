using BusinessObject.Entities;
using DataAccess;
using Microsoft.EntityFrameworkCore;

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
