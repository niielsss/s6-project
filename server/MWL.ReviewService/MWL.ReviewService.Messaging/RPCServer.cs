using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MWL.ReviewService.Domain.Entities;
using Newtonsoft.Json;
using MWL.ReviewService.Domain.Services;
using MWL.ReviewService.Domain.Models;
using Microsoft.Extensions.DependencyInjection;

namespace MWL.ReviewService.Messaging
{
    public class RPCServer
    {
        private readonly ConnectionFactory _factory;
        private IConnection _connection;
        private readonly IModel _channel;
        private readonly string _queueName;
        private IReviewService _reviewService;
        private readonly IServiceProvider _serviceProvider;

        public RPCServer(string queueName, IServiceProvider serviceProvider)
        {
            var host = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "rabbitmq";
            var port = Environment.GetEnvironmentVariable("RABBITMQ_PORT") ?? "5672";
            var username = Environment.GetEnvironmentVariable("RABBITMQ_USERNAME") ?? "username";
            var password = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD") ?? "password";

            Console.WriteLine($" [.] Host: {host}, Port: {port}, Username: {username}, Password: {password}");

            _factory = new ConnectionFactory { HostName = host, Password = password, UserName = username, Port = Convert.ToInt32(port) };
            CreateConnection();
            _channel = _connection.CreateModel();
            _queueName = queueName;
            _serviceProvider = serviceProvider;
        }

        private void CreateConnection()
        {
            int retries = 0;
            int maxRetries = 5;
            TimeSpan delay = TimeSpan.FromSeconds(10);
            while (retries < maxRetries)
            {
                try
                {
                    _connection = _factory.CreateConnection();
                    break;
                }
                catch (Exception)
                {
                    retries++;
                    Task.Delay(delay).Wait();
                    Console.WriteLine($" [.] Retry {retries} of {maxRetries}");
                }
            }

            if (retries == maxRetries)
            {
                throw new Exception("Retry count exceeded");
            }
        }

        public async Task StartListening()
        {
            _channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            var consumer = new EventingBasicConsumer(_channel);
            _channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);

            Console.WriteLine($" [x] Awaiting RPC requests on {_queueName}");

            consumer.Received += async (model, ea) =>
            {
                using var scope = _serviceProvider.CreateScope();
                _reviewService = scope.ServiceProvider.GetRequiredService<IReviewService>();
                string response = string.Empty;

                var body = ea.Body.ToArray();
                var props = ea.BasicProperties;
                var replyProps = _channel.CreateBasicProperties();

                replyProps.CorrelationId = props.CorrelationId;

                try
                {
                    var message = Encoding.UTF8.GetString(body);

                    BrokerMessage brokerMessage = JsonConvert.DeserializeObject<BrokerMessage>(message);
                    Console.WriteLine($" [.] {brokerMessage.Action} has been requested");

                    switch (brokerMessage.Action)
                    {
                        case "GetReviews":
                            Console.WriteLine($" [.] {brokerMessage.Data} has been requested");
                            var reviews = await _reviewService.GetListByFilterAsync(new ReviewFilter { MovieId = int.Parse(brokerMessage.Data)});
                            response = JsonConvert.SerializeObject(reviews);
                            break;
                        case "DeleteReviews":
                            Console.WriteLine($" [.] {brokerMessage.Data} has been requested");
                            await _reviewService.DeleteByUserIdAsync(brokerMessage.Data, CancellationToken.None);
                            response = "Deleted";
                            break;
                        default:
                            response = string.Empty;
                            break;
                    }

                    //int n = int.Parse(message);
                    //Console.WriteLine($" [.] {message} has been requested");

                    //var reviews = await _reviewService.GetListByFilterAsync(new ReviewFilter { MovieId = n });

                    //response = JsonConvert.SerializeObject(reviews);
                }
                catch (Exception e)
                {
                    Console.WriteLine($" [.] {e.Message}");
                    response = string.Empty;
                }
                finally
                {
                    var responseBytes = Encoding.UTF8.GetBytes(response);
                    _channel.BasicPublish(exchange: string.Empty,
                                         routingKey: props.ReplyTo,
                                         basicProperties: replyProps,
                                         body: responseBytes);
                    _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                }
            };
        }

        public void Close()
        {
            _connection.Close();
        }
    }
}
