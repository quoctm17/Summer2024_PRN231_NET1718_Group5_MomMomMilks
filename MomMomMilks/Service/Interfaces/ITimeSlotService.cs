using BusinessObject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface ITimeSlotService
    {
        Task<IEnumerable<TimeSlot>> GetAllTimeSlotsAsync();
        Task<TimeSlot> GetTimeSlotByIdAsync(int id);
    }
}
