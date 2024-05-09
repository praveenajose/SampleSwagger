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
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IActionResult Index()
        {
            var client = new HttpClient();
            var swaggerApiClient = new SwaggerAPIClient("https://localhost:7293/", client);

            var loginViewModel = new LoginRequest { Username = "admin", Password = "admin" };
            var loggedInUser = swaggerApiClient.LoginAsync(loginViewModel).Result;
            var token = "Bearer " + loggedInUser.Token;

            _logger.LogInformation($"Token Received: {token}");
            _logger.LogDebug($"**Token Received: {token}");
            _logger.LogError($"***Token Received: {token}");
            _logger.LogWarning($"****Token Received: {token}");

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