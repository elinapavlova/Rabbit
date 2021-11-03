using System;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Configurations;
using Infrastructure.Repositories.Message;
using Microsoft.Extensions.Options;
using Models.Messages;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly RabbitMqOptions _options;
        
        public MessageService
        (
            IOptions<RabbitMqOptions> options, 
            IMessageRepository messageRepository
        )
        {
            _options = options.Value;
            _messageRepository = messageRepository;
        }

        public async Task ListenAsync()
        {
            var factory = new ConnectionFactory {HostName = _options.HostName};
            
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            
            channel.QueueDeclare(_options.QueueTo, false, false, false, null);
            channel.QueueDeclare(_options.QueueFrom, false, false, false, null);
            
            var consumer = new EventingBasicConsumer(channel);

            await SendMessage(channel);

            // Receiving messages
            consumer.Received += async (_, ea) =>
            {
               await ReceiveMessage(ea);
            };
            channel.BasicConsume(_options.QueueFrom, true, consumer);

            Console.ReadLine();
        }

        private async Task ReceiveMessage(BasicDeliverEventArgs ea)
        {
            var receivedBody = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(receivedBody);
            Console.WriteLine("received {0} : {1}", message, DateTime.Now);

            await TrySaveMessage(message);
        }

        private async Task SendMessage(IModel channel)
        {
            // PING to bytes array
            var sentBody = Encoding.UTF8.GetBytes(_options.DefaultMessage);            
            channel.BasicPublish("", _options.QueueTo, null, sentBody);
        }
        
        private async Task TrySaveMessage(string newMessage)
        {
            if (string.IsNullOrEmpty(newMessage))
            {
                Console.WriteLine("Bad message text!");
                return;
            }

            var message = new MessageDto
            {
                DateCreated = DateTime.Now,
                Text = newMessage
            };
            
            await _messageRepository.AddAsync(message);
        }
    }
}