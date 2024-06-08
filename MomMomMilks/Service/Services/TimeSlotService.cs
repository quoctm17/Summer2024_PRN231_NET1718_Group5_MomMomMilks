using BusinessObject.Entities;
using Repository.Interface;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class TimeSlotService : ITimeSlotService
    {
        private readonly ITimeSlotRepository _timeSlotRepository;

        public TimeSlotService(ITimeSlotRepository timeSlotRepository)
        {
            _timeSlotRepository = timeSlotRepository;
        }

        public async Task<IEnumerable<TimeSlot>> GetAllTimeSlotsAsync()
        {
            return await _timeSlotRepository.GetAllTimeSlotsAsync();
        }

        public async Task<TimeSlot> GetTimeSlotByIdAsync(int id)
        {
            return await _timeSlotRepository.GetTimeSlotByIdAsync(id);
        }
    }
}
