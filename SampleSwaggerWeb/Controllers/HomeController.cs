using Microsoft.AspNetCore.Mvc;
using SampleSwagger.Endpoints;
using SampleSwaggerWeb.Models;
using System.Diagnostics;

namespace SampleSwaggerWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var client = new HttpClient();
            var swaggerApiClient = new SwaggerAPIClient("https://localhost:7150/", client);

            var loginViewModel = new LoginRequest { Username = "admin", Password = "admin" };
            swaggerApiClient.LoginAsync(loginViewModel);
            //var loggedInUser = swaggerApiClient.LoginAsync(loginViewModel);
            //var token = "Bearer " + loggedInUser.Token;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}