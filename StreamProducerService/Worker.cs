using StreamProducerService.Service;

namespace StreamProducerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ITwitterStreamService twitterStreamService;

        public Worker(ILogger<Worker> logger, ITwitterStreamService twitterStreamService)
        {
            _logger = logger;
            this.twitterStreamService = twitterStreamService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    twitterStreamService.processTweetSamples(stoppingToken);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Tweet sample processing failed. Will retry after a second.");
                }
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}