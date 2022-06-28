using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using FluentScheduler;


namespace EmailReminderService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            /*FluentScheduler.Schedule(x =>
            {
                EventService.SendEmailReminders():
            }).Run().Every(1).Min();*/

            //Console.WriteLine("Proba " + DateTime.Now); ova raboti

           
            JobManager.Initialize(new JobRegistry());
        }
    }
}
