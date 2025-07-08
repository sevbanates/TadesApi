using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace VideoPortalApi.RabbitMqService
{
    public class App
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfigurationRoot _config;
        private readonly ILogger<App> _logger;

        public App(IServiceProvider serviceProvider, IConfigurationRoot config, ILoggerFactory loggerFactory)
        {
            _serviceProvider = serviceProvider;
            _logger = loggerFactory.CreateLogger<App>();
            _config = config;
        }

        public async Task Run(string[] args)
        {
            var busControl = _serviceProvider.GetRequiredService<IBusControl>();

            await busControl.StartAsync(new CancellationTokenSource(TimeSpan.FromSeconds(10)).Token);
            try
            {
                _logger.LogInformation("Press enter to exit");

                await Task.Run(() => Console.ReadLine());
            }
            finally
            {
                await busControl.StopAsync();
            }
        }
    }
}
