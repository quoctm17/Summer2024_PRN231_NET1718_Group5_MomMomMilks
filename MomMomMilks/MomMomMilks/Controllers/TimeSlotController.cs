using BusinessObject.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Service.Interfaces;

namespace MomMomMilks.Controllers
{
    [Route("odata/[controller]")]
    [ApiController]
    public class TimeSlotController : ODataController
    {
        private readonly ITimeSlotService _timeSlotService;

        public TimeSlotController(ITimeSlotService timeSlotService)
        {
            _timeSlotService = timeSlotService;
        }

        [EnableQuery]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TimeSlot>>> Get()
        {
            var timeSlots = await _timeSlotService.GetAllTimeSlotsAsync();
            return Ok(timeSlots);
        }

        [EnableQuery]
        [HttpGet("{id}")]
        public async Task<ActionResult<TimeSlot>> Get(int id)
        {
            var timeSlot = await _timeSlotService.GetTimeSlotByIdAsync(id);

            if (timeSlot == null)
            {
                return NotFound();
            }

            return Ok(timeSlot);
        }
    }
}
