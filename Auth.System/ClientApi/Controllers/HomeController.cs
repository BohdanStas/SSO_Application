using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ClientApi.Models;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace ClientApi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IHttpClientFactory httpClientFactory;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            this.httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetSecret()
        {
            var authClient = httpClientFactory.CreateClient();

            var document = await authClient.GetDiscoveryDocumentAsync("https://localhost:10001");

            var responseToken = await authClient.RequestClientCredentialsTokenAsync(
                new ClientCredentialsTokenRequest
                {

                    Address = document.TokenEndpoint,

                    ClientId = "client_id",
                    ClientSecret = "client_secret",
                    Scope = "OrdersAPI"
                });

            var orderClient = httpClientFactory.CreateClient();

            orderClient.SetBearerToken(responseToken.AccessToken);
           
            var response = await orderClient.GetAsync("https://localhost:5001/Home/Secret");

            var message = await response.Content.ReadAsStringAsync();

            ViewData["Message"] = message;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
