using Microsoft.AspNetCore.Mvc;

namespace TestUploadForbidden.Controllers
{
    [Route("Account")]
    public class AccountController:Controller
    {
        [HttpGet("AccessDenied")]
        public ActionResult AccessDenied()
        {
            this.Response.StatusCode = 403;
            return this.Content("Access denied");
        }
    }
}