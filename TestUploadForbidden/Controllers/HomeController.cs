using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TestUploadForbidden.Controllers
{
    [ApiController]
    public class HomeController : Controller
    {
        [HttpGet("/")]
        public ActionResult Index()
        {
            return this.Content("<html><body><form method='post' action='/' enctype='multipart/form-data'><input type='file' name='file' /><button type='submit'>send</button></form></body></html>", "text/html");
        }

        [RequestSizeLimit(2_000_000_000)]// 2GB
        [HttpPost("/")]
        public ActionResult Index(IFormFile file)
        {
            return this.Forbid();
        }
    }
}