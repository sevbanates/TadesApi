using MassTransit;
using System;
using System.Threading.Tasks;

namespace VideoPortalApi.RabbitMqService.TaskManager
{
    public abstract class BaseConsumer<T> : IConsumer<T> where T : class
    {
        public BaseConsumer()
        {
        }

        public abstract Task Consume(ConsumeContext<T> context);

        protected async void LogInfo(string message)
        {

            //if (Convert.ToBoolean(Convert.ToBoolean(ConfigurationManager.AppSettings["RabbitMQLogInfo"])))
            //{
            await Console.Out.WriteLineAsync(Environment.NewLine + message);
            //}
        }
    }
}
