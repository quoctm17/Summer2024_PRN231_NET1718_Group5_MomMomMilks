using Service.Interfaces;
using Quartz;
using System.Threading.Tasks;

namespace Service.Services
{
    public class AssignOrdersJob : IJob
    {
        private readonly IOrderService _orderService;

        public AssignOrdersJob(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _orderService.AutoAssignOrdersToShippers();
        }
    }
}
