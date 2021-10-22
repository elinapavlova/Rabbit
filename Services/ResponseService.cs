using System;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Configurations;
using Infrastructure.Repositories.Response;
using Microsoft.Extensions.Options;
using Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Services
{
    public class ResponseService : IResponseService
    {
        private readonly AppOptions _options;
        private readonly IResponseRepository _responseRepository;

        public ResponseService
        (
            IOptions<AppOptions> options,
            IResponseRepository responseRepository
        )
        {
            _options = options.Value;
            _responseRepository = responseRepository;
        }

        public async Task ListenAsync()
        {
            var factory = new ConnectionFactory {HostName = _options.HostName};
            
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            
            channel.QueueDeclare(_options.QueueTo, false, false, false, null);
            channel.QueueDeclare(_options.QueueFrom, false, false, false, null);
            
            var consumer = new EventingBasicConsumer(channel);

            // Receiving messages
            consumer.Received += async (_, ea) =>
            {
               await ReceiveMessage(ea);
               await SendMessage(channel);
            };
            channel.BasicConsume(_options.QueueFrom, true, consumer);

            Console.ReadLine();
        }

        private async Task ReceiveMessage(BasicDeliverEventArgs ea)
        {
            var receivedBody = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(receivedBody);
            Console.WriteLine("received {0} : {1}", message, DateTime.Now);

            await TryAddMessage(message);
        }

        private async Task SendMessage(IModel channel)
        {
            // PING to bytes array
            var sentBody = Encoding.UTF8.GetBytes(_options.DefaultMessage);            
            channel.BasicPublish("", _options.QueueTo, null, sentBody);
        }

        private async Task TryAddMessage(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                Console.WriteLine("Bad message text!");
                return;
            }

            var response = new ResponseDto
            {
                DateCreated = DateTime.Now,
                Message = message
            };
            
            await _responseRepository.AddAsync(response);
        }
    }
}