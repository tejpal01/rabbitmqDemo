using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace Messaging_Rabbit.Repositories
{
    public interface IMessageInterface
    {
        IConnection GetConnection();
        bool send(IConnection con, string message, string friendqueue);
        string receive(IConnection con,string myqueue);
    }
}