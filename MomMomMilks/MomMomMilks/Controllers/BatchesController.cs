using AutoMapper;
using BusinessObject.Entities;
using DataTransfer;
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
        private readonly IMapper _mapper;

        public BatchesController(IBatchService batchService, IMapper mapper)
        {
            _batchService = batchService;
            _mapper = mapper;
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
        public async Task<IActionResult> Post([FromBody] CreateBatchDTO batchDTO)
        {
            var batch = _mapper.Map<Batch>(batchDTO);
            var result = await _batchService.CreateBatch(batch);
            if(!result) return BadRequest();
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateBatchDTO batchDTO)
        {
            var batch = _mapper.Map<Batch>(batchDTO);
            var result = await _batchService.UpdateBatch(batch);
            if (!result) return BadRequest();
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _batchService.DeleteBatch(id);
            if (!result) return BadRequest();
            return Ok(result);
        }

        [HttpGet("TotalQuantityByMilk/{milkId}")]
        public async Task<IActionResult> GetTotalQuantityByMilkId(int milkId)
        {
            var totalQuantity = await _batchService.GetTotalQuantityByMilkId(milkId);
            return Ok(totalQuantity);
        }
        [HttpGet("GetBatchByMilkId/{milkId}")]
        public async Task<IActionResult> GetBatchByMilkId(int milkId)
        {
            var result = await _batchService.GetBatchByMilkId(milkId);
            return Ok(result);
        }
    }
}
