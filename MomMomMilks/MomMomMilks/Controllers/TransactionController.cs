using BusinessObject.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Service.Interfaces;

namespace MomMomMilks.Controllers
{
    [Route("odata/[controller]")]
    [ApiController]
    public class TransactionController : ODataController
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [EnableQuery]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var transactions = await _transactionService.GetAllTransactionsAsync();
            return Ok(transactions);
        }

        [EnableQuery]
        [HttpGet("{key}")]
        public async Task<IActionResult> Get([FromODataUri] int key)
        {
            var transaction = await _transactionService.GetTransactionByIdAsync(key);
            if (transaction == null)
            {
                return NotFound();
            }
            return Ok(transaction);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Transaction transaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _transactionService.AddTransactionAsync(transaction);
            return Created(transaction);
        }

        [HttpPut("{key}")]
        public async Task<IActionResult> Put([FromODataUri] int key, [FromBody] Transaction transaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingTransaction = await _transactionService.GetTransactionByIdAsync(key);
            if (existingTransaction == null)
            {
                return NotFound();
            }

            await _transactionService.UpdateTransactionAsync(transaction);
            return Updated(transaction);
        }

        [HttpDelete("{key}")]
        public async Task<IActionResult> Delete([FromODataUri] int key)
        {
            var existingTransaction = await _transactionService.GetTransactionByIdAsync(key);
            if (existingTransaction == null)
            {
                return NotFound();
            }

            await _transactionService.DeleteTransactionAsync(key);
            return NoContent();
        }
    }
}
