using BusinessObject.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Service.Interfaces;

namespace MomMomMilks.Controllers
{
    [Route("odata/[controller]")]
    [ApiController]
    public class BatchesController : ODataController
    {
        private readonly IBatchService _batchService;

        public BatchesController(IBatchService batchService)
        {
            _batchService = batchService;
        }

        [EnableQuery]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _batchService.GetAllBatches();
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingleBatch(int id)
        {
            return Ok(await _batchService.GetSingleBatch(id));
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Batch batch)
        {
            var result = await _batchService.CreateBatch(batch);
            if(!result) return BadRequest();
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Batch batch)
        {
            var result = await _batchService.UpdateBatch(batch);
            if (!result) return BadRequest();
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _batchService.DeleteBatch(id);
            if (!result) return BadRequest();
            return Ok();
        }
    }
}
