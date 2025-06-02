
namespace PasswordManager.WebAPI.Services
{
    public class JwtKeyRotationService : BackgroundService
    {

        private readonly TimeSpan interval = TimeSpan.FromDays(7);
        private readonly JwtKeyService _jwtKeyService;

        public JwtKeyRotationService(JwtKeyService jwtKeyService)
        {
            _jwtKeyService = jwtKeyService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(interval, stoppingToken);
                _jwtKeyService.GenerateJwtKey();
            }
        }
    }
}
