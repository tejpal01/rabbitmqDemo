using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Messaging_Rabbit.Models;
using Npgsql;

namespace Messaging_Rabbit.Repositories
{
    public class UserRepo : IUserInterface
    {
        private readonly string? _ConnectionString;
        private NpgsqlConnection _conn;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string Username, Password, HostName, VirtualHost;
        private int Port;
        public UserRepo(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
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
        public bool Login(UserModel user)
        {
            Console.WriteLine(" "+Username);
            Console.WriteLine(" "+Password);
            Console.WriteLine(" "+HostName);
            Console.WriteLine(" "+Port);
            Console.WriteLine(" "+VirtualHost);

            bool isUserAuthenticated = false;
            try
            {
                _conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT c_userid, c_email, c_password FROM t_register WHERE c_email=@uemail AND c_password=@password;", _conn))
                {
                    cmd.Parameters.AddWithValue("@uemail", user.c_email);
                    cmd.Parameters.AddWithValue("@password", user.c_password);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isUserAuthenticated = true;
                            var session = _httpContextAccessor.HttpContext.Session;
                            session.SetString("username",(string)reader["c_email"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error In Login" + ex);
            }
            finally
            {
                _conn.Close();
            }

            return isUserAuthenticated;
        }
    }

}