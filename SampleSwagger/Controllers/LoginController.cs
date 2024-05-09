using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SampleSwaggerApi.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SampleSwagger.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(LoggedInUserViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
        {
            if (request == null)
            {
                // Return 400 bad request.
                return this.Problem(statusCode: (int)HttpStatusCode.BadRequest, title: "Login details not provided");
            }

            // Perform authentication logic here
            var validUser = await IsValidUserAsync(request.Username, request.Password);

            if(!validUser)
            {
                // Return 400 bad request.
                return this.Problem(statusCode: 400, title: "Invalid email address or password");
            }

            // Simulate generating a token (replace with your actual authentication logic)
            var token = GenerateJwtToken(request.Username);

            return Ok(new LoggedInUserViewModel { Token = token });
        }

        // Assuming IsValidUser method is made asynchronous as well
        private async Task<bool> IsValidUserAsync(string username, string password)
        {
            // Your asynchronous validation logic goes here
            // For example, if you're making an asynchronous database call to validate the user
            // You would await that call here and return true/false based on the result
            // For demonstration purposes, let's assume it's an asynchronous method returning a boolean
            return await Task.FromResult(IsValidUser(username, password));
        }

        private bool IsValidUser(string username, string password)
        {
            // Implement your authentication logic here
            // For demonstration purposes, assume username and password are valid
            return true;
        }

        private string GenerateJwtToken(string username)
        {
            IConfiguration config = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .Build();

            JwtOptions jwtOptions = new JwtOptions();
            config.GetSection("JwtOptions").Bind(jwtOptions);

            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(jwtOptions.SigningKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, username)
                }),
                Expires = DateTime.UtcNow.AddDays(7), // Token expiration time
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

}
