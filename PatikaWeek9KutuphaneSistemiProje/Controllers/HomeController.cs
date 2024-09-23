using Microsoft.AspNetCore.Mvc;
using PatikaWeek9KutuphaneSistemiProje.Models;

namespace PatikaWeek9KutuphaneSistemiProje.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        { return View(); }
    }
}
