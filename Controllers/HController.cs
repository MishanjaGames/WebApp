using Microsoft.AspNetCore.Mvc;

namespace WebApp
{
    public class HController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
