using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Messaging_Rabbit.Models;
using Messaging_Rabbit.Repositories;
using RabbitMQ.Client;

namespace Messaging_Rabbit.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMessageInterface _messageInterface;

    public HomeController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor, IMessageInterface messageInterface)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        _messageInterface = messageInterface;
    }

    public IActionResult Index()
    {
        var session = _httpContextAccessor.HttpContext.Session;
        if (session.GetString("username") != null)
        {
            return View();
        }
        else
        {
            return RedirectToAction("Login", "User");
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }


    [HttpPost]
    public IActionResult sendmsg(string message, string friend)
    {
        Console.WriteLine("*************** "+ friend);
        Console.WriteLine("*************** "+ message);
        IConnection con = _messageInterface.GetConnection();
        bool flag = _messageInterface.send(con, message, friend);
        return Json(null);
    }

    [HttpPost]
    public IActionResult receive()
    {
       
            var Session = _httpContextAccessor.HttpContext.Session;
            IConnection con = _messageInterface.GetConnection();
            string userqueue = Session.GetString("username");
           
            string message = _messageInterface.receive(con, userqueue);
            return Json(message);
       


    }

    [HttpPost]
    public IActionResult friendlist(){
        string[] friend = {"admin@gmail.com", "user@gmail.com", "tejpal@gmail.com"};
        return Json(friend);
    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
