using System;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Configurations;
using Infrastructure.UnitOfWork;
using Microsoft.Extensions.Options;
using Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Services
{
    public class ResponseService : IResponseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppOptions _options;
        
        public ResponseService
        (
            IOptions<AppOptions> options,
            IUnitOfWork unitOfWork
        )
        {
            _options = options.Value;
            _unitOfWork = unitOfWork;
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
               await GetMessages(ea, channel);
            };

            channel.BasicConsume(_options.QueueFrom, true, consumer);

            Console.ReadLine();
        }

        private async Task GetMessages(BasicDeliverEventArgs ea, IModel channel)
        {
            var receivedBody = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(receivedBody);
            Console.WriteLine("received {0} : {1}", message, DateTime.Now);

            await TryAddResponse(message);
            await SendMessage(channel);
        }

        private async Task SendMessage(IModel channel)
        {
            // PING to bytes
            for (var i = 0; i < 3; i++ )
            {
                var sentBody = Encoding.UTF8.GetBytes(_options.DefaultMessage);            
                channel.BasicPublish("", _options.QueueTo, null, sentBody);
            }
        }

        private async Task TryAddResponse(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                Console.WriteLine("Bad message text!");
                return;
            }

            var response = new Response
            {
                DateCreated = DateTime.Now,
                Message = message
            };
            
            // Add response to db
            await _unitOfWork.ResponseRepository.Add(response);
            _unitOfWork.Save();
        }
    }
}