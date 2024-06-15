using Service.Interfaces;

namespace MomMomMilks.Extensions
{
    public class BackgroundMomMom : IHostedService, IDisposable
    {
        private Timer? _timer;
        private readonly IServiceProvider _serviceProvider;
        public BackgroundMomMom(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(UpdateCouponExpiryDate, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        private async void UpdateCouponExpiryDate(object? state)
        {
            /*using (var scope = _serviceProvider.CreateScope())
            {
                var couponService = scope.ServiceProvider.GetRequiredService<ICouponService>();
                await couponService.UpdateCouponExpiryDate();
            }*/
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
