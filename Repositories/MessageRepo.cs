using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Messaging_Rabbit.Repositories
{
    public class MessageRepo : IMessageInterface
    {
        private readonly string? _ConnectionString;
        private NpgsqlConnection _conn;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string Username, Password, HostName, VirtualHost;
        private int Port;
        public MessageRepo(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _ConnectionString = configuration.GetConnectionString("DefaultConnection");
            _conn = new NpgsqlConnection(_ConnectionString);
            _httpContextAccessor = httpContextAccessor;
            Username = configuration["RabbitMQ:Username"];
            Password = configuration["RabbitMQ:Password"];
            HostName = configuration["RabbitMQ:HostName"];
            Port = configuration.GetValue<int>("RabbitMQ:Port");
            VirtualHost = configuration["RabbitMQ:VirtualHost"];

        }

        public IConnection GetConnection()
        {
            
            ConnectionFactory factory = new ConnectionFactory
            {
                HostName = HostName

            };

            return factory.CreateConnection();
        }

        public bool send(IConnection con, string message, string friendqueue)
        {
            try
            {
                IModel channel = con.CreateModel();
                channel.ExchangeDeclare("messageexchange", ExchangeType.Direct);
                channel.QueueDeclare(friendqueue, true, false, false, null); //durable,exclusive,autodelete
                channel.QueueBind(friendqueue, "messageexchange", friendqueue, null);
                var msg = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish("messageexchange", friendqueue, null, msg);

            }
            catch (Exception)
            {

            }
            return true;

        }

        public string receive(IConnection con, string myqueue)
        {
            try
            {
                string queue = myqueue;
                IModel channel = con.CreateModel();
                channel.QueueDeclare(queue: queue, durable: true, exclusive: false, autoDelete: false, arguments: null);
                var consumer = new EventingBasicConsumer(channel);
                BasicGetResult result = channel.BasicGet(queue: queue, autoAck: true);
                if (result != null)
                {
                    byte[] bodyBytes = result.Body.ToArray();
                    string message = Encoding.UTF8.GetString(bodyBytes);
                    return message;
                   
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;

            }

        }

    }
}