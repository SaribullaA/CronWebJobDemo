using Cronos;
using CronWebJobDemo.Services;

public class CronJobService : BackgroundService
{
    private readonly CronExpression _cron = CronExpression.Parse("*/10 * * * * *", CronFormat.IncludeSeconds);
    private readonly TimeZoneInfo _timeZone = TimeZoneInfo.Local;
    private readonly MyService _service;

    public CronJobService(MyService service)
    {
        _service = service;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var next = _cron.GetNextOccurrence(DateTimeOffset.Now, _timeZone);

            if (next.HasValue)
            {
                var delay = next.Value - DateTimeOffset.Now;

                if (delay.TotalMilliseconds > 0)
                    await Task.Delay(delay, stoppingToken);

                Console.WriteLine($"Running job at: {DateTime.Now}");

                try
                {
                    _service.DoWork();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Job failed: {ex.Message}");
                }
            }
        }
    }
}