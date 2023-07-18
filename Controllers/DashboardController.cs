using Microsoft.AspNetCore.Mvc;

namespace OnlineVotingSystem.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
