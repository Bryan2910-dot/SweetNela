using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SweetNela.Controllers
{
    [Route("[controller]")]
    public class ChatController : Controller
    {
        private readonly ILogger<ChatController> _logger;

        public ChatController(ILogger<ChatController> logger)
        {
            _logger = logger;
        }

        [HttpGet("")] // Esto aclara que esta es la ruta GET base: /Chat
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Error")] // Esto aclara que esta es la ruta GET: /Chat/Error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error");
        }
    }
}
