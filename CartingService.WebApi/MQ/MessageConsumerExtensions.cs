using MessageQueue.Interfaces;

namespace CartingService.WebApi.MQ
{
    public static class MessageConsumerExtensions
    {
        public static IApplicationBuilder UseRabbitMQ(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var messageConsumer = scope.ServiceProvider.GetRequiredService<IMessageConsumer>();
                messageConsumer?.ProcessMessages();
            }

            return app;
        }
    }
}
