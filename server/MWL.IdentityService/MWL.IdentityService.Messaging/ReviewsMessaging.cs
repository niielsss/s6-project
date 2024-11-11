using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MWL.IdentityService.Messaging
{
    public class ReviewsMessaging : IDisposable
    {
        private const string QUEUE_NAME = "reviewservice_messaging";

        private readonly IConnection connection;
        private readonly IModel channel;
        private readonly string replyQueueName;
        private readonly ConcurrentDictionary<string, TaskCompletionSource<string>> callbackMapper = new();

        public ReviewsMessaging()
        {
            var host = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "rabbitmq";
            var port = Environment.GetEnvironmentVariable("RABBITMQ_PORT") ?? "5672";
            var username = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME") ?? "username";
            var password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD") ?? "password";

            Console.WriteLine($" [.] Host: {host}, Port: {port}, Username: {username}, Password: {password}");

            var factory = new ConnectionFactory { HostName = host, Password = password, UserName = username, Port = Convert.ToInt32(port) };

            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            // declare a server-named queue
            replyQueueName = channel.QueueDeclare().QueueName;
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                if (!callbackMapper.TryRemove(ea.BasicProperties.CorrelationId, out var tcs))
                    return;
                var body = ea.Body.ToArray();
                var response = Encoding.UTF8.GetString(body);
                tcs.TrySetResult(response);
            };

            channel.BasicConsume(consumer: consumer,
                                 queue: replyQueueName,
                                 autoAck: true);
        }

        public async Task<string> CallAsync(BrokerMessage message, CancellationToken cancellationToken = default)
        {
            IBasicProperties props = channel.CreateBasicProperties();
            var correlationId = Guid.NewGuid().ToString();
            props.CorrelationId = correlationId;
            props.ReplyTo = replyQueueName;
            var messageBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
            var tcs = new TaskCompletionSource<string>();
            callbackMapper.TryAdd(correlationId, tcs);

            channel.BasicPublish(exchange: string.Empty,
                                 routingKey: QUEUE_NAME,
                                 basicProperties: props,
                                 body: messageBytes);

            cancellationToken.Register(() => callbackMapper.TryRemove(correlationId, out _));

            var completedTask = await Task.WhenAny(tcs.Task, Task.Delay(TimeSpan.FromSeconds(15), cancellationToken));

            if (completedTask == tcs.Task)
            {
                return await tcs.Task;
            }
            else
            {
                callbackMapper.TryRemove(correlationId, out _);
                throw new TimeoutException("Timeout occurred while waiting for response.");
            }

            //return tcs.Task;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                connection?.Dispose();
                channel?.Dispose();
            }
        }

    }
}
