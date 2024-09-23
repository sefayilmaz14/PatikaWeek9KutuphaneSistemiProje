using Microsoft.AspNetCore.Mvc;
using PatikaWeek9KutuphaneSistemiProje.Models;

namespace PatikaWeek9KutuphaneSistemiProje.Controllers
{
    public class GenreController : Controller
    {
        public static List<Genre> genres = new List<Genre>()
        {
            new Genre{GenreId=1, GenreName="Dram"},
            new Genre{GenreId=2, GenreName="Epik"},
            new Genre{GenreId=3, GenreName="Komedi"},
        };
        [HttpGet]
        public IActionResult Add()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Add(GenreAddViewModel formData)
        {
            if (!ModelState.IsValid)
            {

                return View(formData);
            }

            int maxId = genres.Max(x => x.GenreId);

            var newBook = new Genre()
            {
                GenreId = maxId + 1,
                GenreName = formData.GenreName,




            };

            genres.Add(newBook);
            return RedirectToAction("Add","Book");
        }
    }
}
