using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Web.Controllers
{
    public class SecretsTestController : Controller
    {
        private readonly IConfiguration _config;

        public SecretsTestController(IConfiguration config)
        {
            _config = config;
        }

        public IActionResult Index()
        {
            var stripe = _config["Stripe:PublishableKey"];
            var mail = _config["Mail:Smtp:Host"];
            var cloud = _config["Cloudinary:CloudName"];

            return Content($"Stripe={stripe}\nMail={mail}\nCloudinary={cloud}");
        }
    }
}