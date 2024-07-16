using Service.Interfaces;

namespace MomMomMilks.Extensions
{
    public class BackgroundTaskAutoAssign : IHostedService, IDisposable
    {
        private Timer? _timer;
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _morningStart = new TimeSpan(5, 59, 0);
        private readonly TimeSpan _morningEnd = new TimeSpan(8, 59, 0);
        private readonly TimeSpan _afternoonStart = new TimeSpan(10, 59, 0);
        private readonly TimeSpan _afternoonEnd = new TimeSpan(13, 59, 0);
        private readonly TimeSpan _eveningStart = new TimeSpan(15, 59, 0);
        private readonly TimeSpan _eveningEnd = new TimeSpan(18, 59, 0);
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

            if (IsWithinTimeWindow(now, _morningStart, _morningEnd) ||
                IsWithinTimeWindow(now, _afternoonStart, _afternoonEnd) ||
                IsWithinTimeWindow(now, _eveningStart, _eveningEnd))
            {
                await AutoAssignShipper(state);
                await AutoUpdateOrderStatus(state);
            }

            ScheduleNextRun();
        }
        private bool IsWithinTimeWindow(TimeSpan currentTime, TimeSpan startTime, TimeSpan endTime)
        {
            return currentTime >= startTime && currentTime <= endTime;
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

        private string GetCurrentTimeSlot()
        {
            var now = DateTime.Now.TimeOfDay;

            if (IsWithinTimeWindow(now, _morningStart, _morningEnd))
            {
                return "Morning";
            }
            else if (IsWithinTimeWindow(now, _afternoonStart, _afternoonEnd))
            {
                return "Afternoon";
            }
            else if (IsWithinTimeWindow(now, _eveningStart, _eveningEnd))
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
