using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace Messaging_Rabbit.Models
{
    public class UserModel
    {
        public int c_userid {get; set;} = 0;
        public string c_email {get; set;} = string.Empty;
        public string c_password {get; set;} = string.Empty;
    }
}