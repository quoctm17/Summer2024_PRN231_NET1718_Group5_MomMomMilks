using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Service.Interfaces;

namespace MomMomMilks.Controllers
{
    [Route("odata/[controller]")]
    [ApiController]
    public class PaymentTypeController : ODataController
    {
        private readonly IPaymentTypeService _paymentTypeService;

        public PaymentTypeController(IPaymentTypeService paymentTypeService)
        {
            _paymentTypeService = paymentTypeService;
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            var paymentTypes = await _paymentTypeService.GetAllPaymentTypesAsync();
            return Ok(paymentTypes);
        }
    }
}
