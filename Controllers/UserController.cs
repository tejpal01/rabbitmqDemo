using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Messaging_Rabbit.Models;
using Messaging_Rabbit.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Messaging_Rabbit.Controllers
{
    // [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserInterface _userInterface;

        public UserController(ILogger<UserController> logger, IUserInterface userInterface)
        {
            _logger = logger;
            _userInterface = userInterface;
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(UserModel user)
        {
            var status = _userInterface.Login(user);
            if(status){
                return RedirectToAction("Index","Home");
            }else{
                return RedirectToAction("Login");
            }
            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}