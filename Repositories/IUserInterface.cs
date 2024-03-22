using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Messaging_Rabbit.Models;

namespace Messaging_Rabbit.Repositories
{
    public interface IUserInterface
    {
        bool Login(UserModel user);
    }
}