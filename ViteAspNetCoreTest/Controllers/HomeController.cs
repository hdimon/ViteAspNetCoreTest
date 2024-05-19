using Microsoft.AspNetCore.Mvc;

namespace ViteAspNetCoreTest.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Contacts()
        {
            return View();
        }
    }
}
