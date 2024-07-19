using MomMomMilks.EmailService;
using Service.Interfaces;

namespace MomMomMilks.Extensions
{
    public class BackgroundTaskAutoAssign : IHostedService, IDisposable
    {
        private Timer? _timer;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<BackgroundTaskAutoAssign> _logger;
        private readonly TimeSpan _morningStart = new TimeSpan(5, 59, 0);
        private readonly TimeSpan _morningEnd = new TimeSpan(8, 59, 0);
        private readonly TimeSpan _afternoonStart = new TimeSpan(10, 59, 0);
        private readonly TimeSpan _afternoonEnd = new TimeSpan(13, 59, 0);
        private readonly TimeSpan _eveningStart = new TimeSpan(15, 59, 0);
        private readonly TimeSpan _eveningEnd = new TimeSpan(18, 59, 0);
        private readonly TimeSpan _tolerance = TimeSpan.FromMinutes(1);
        public BackgroundTaskAutoAssign(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        private void ScheduleNextRun()
        {
            var now = DateTime.Now.TimeOfDay;
            TimeSpan initialDelay;

            if (now < _morningStart)
            {
                initialDelay = _morningStart - now;
            }
            else if (now < _morningEnd)
            {
                initialDelay = _morningEnd - now;
            }
            else if (now < _afternoonStart)
            {
                initialDelay = _afternoonStart - now;
            }
            else if (now < _afternoonEnd)
            {
                initialDelay = _afternoonEnd - now;
            }
            else if (now < _eveningStart)
            {
                initialDelay = _eveningStart - now;
            }
            else if (now < _eveningEnd)
            {
                initialDelay = _eveningEnd - now;
            }
            else
            {
                initialDelay = new TimeSpan(24, 0, 0) - now + _morningStart;
            }

            _timer = new Timer(RunTaskIfInTimeWindow, null, initialDelay, Timeout.InfiniteTimeSpan);
        }
        private async void RunTaskIfInTimeWindow(object? state)
        {
            var now = DateTime.Now.TimeOfDay;

            if (IsWithinTolerance(now, _morningStart) ||
                IsWithinTolerance(now, _afternoonStart) ||
                IsWithinTolerance(now, _eveningStart))
            {
                await AutoAssignShipper(state);
                await AutoUpdateOrderStatus(state);
            } else if (IsWithinTolerance(now, _morningEnd) ||
                IsWithinTolerance(now, _afternoonEnd) ||
                IsWithinTolerance(now, _eveningEnd))
            {
                await AutoUpdateOverDateShippingOrder(state);
            }

                ScheduleNextRun();
        }

        private bool IsWithinTimeWindow(TimeSpan currentTime, TimeSpan startTime, TimeSpan endTime)
        {
            return currentTime >= startTime && currentTime <= endTime;
        }
        private bool IsWithinTolerance(TimeSpan currentTime, TimeSpan targetTime)
        {
            return Math.Abs((currentTime - targetTime).TotalMinutes) <= _tolerance.TotalMinutes;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            ScheduleNextRun();
            return Task.CompletedTask;
        }
        

        private async Task AutoAssignShipper(object? state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();

                // Define the current date and time slot
                var currentDate = DateTime.Now.Date;
                string currentTimeSlot = GetCurrentTimeSlot();

                // Pass the current date and time slot to AutoAssignOrdersToShippers
                await orderService.AutoAssignOrdersToShippers(currentDate, currentTimeSlot);
            }
        }
        private async Task AutoUpdateOverDateShippingOrder(object? state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                string currentTimeSlot = GetCurrentTimeSlot();

                var orders = await orderService.IsLateForShippingToNotifyShipper(currentTimeSlot);
                foreach (var order in orders)
                {
                    await emailService.SendAsync("autoemail62@gmail.com",
                    order.ShipperEmail, order.Title,
                    order.Message);
                }
            }
        }

        private string GetCurrentTimeSlot()
        {
            var now = DateTime.Now.TimeOfDay;

            if (IsWithinTimeWindow(now, _morningStart, _morningEnd + TimeSpan.FromMinutes(1)))
            {
                return "Morning";
            }
            else if (IsWithinTimeWindow(now, _afternoonStart, _afternoonEnd + TimeSpan.FromMinutes(1)))
            {
                return "Afternoon";
            }
            else if (IsWithinTimeWindow(now, _eveningStart, _eveningEnd + TimeSpan.FromMinutes(1)))
            {
                return "Evening";
            }
            else
            {
                throw new Exception("Current time does not fall within any defined time slot.");
            }
        }

        private async Task AutoUpdateOrderStatus(object? state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();
                await orderService.UpdateOrderStatusWhenOverDateAsync();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
