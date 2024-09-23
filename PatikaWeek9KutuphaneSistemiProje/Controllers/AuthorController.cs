using Microsoft.AspNetCore.Mvc;
using PatikaWeek9KutuphaneSistemiProje.Models;
using static System.Reflection.Metadata.BlobBuilder;

namespace PatikaWeek9KutuphaneSistemiProje.Controllers
{
    public class AuthorController : Controller
    {
        public static List<Author> Authors = new List<Author>()
        {
            new Author{Id=1,FirstName="Jo",LastName="Nesbo",DateOfBirth=new DateTime(2024,06,12),About="1960’ta Oslo’da doğdu. Norveç Ekonomi Üniversitesi’nde ekonomi ve işletme okudu. Dedektif Harry Hole polisiyeleriyle dünya çapında ün kazanan ve kitapları 51 dile çevrilen Nesbo,çocuk kitapları da yazdı. Aynı zamanda Di Derre rock grubunun solisti ve şarkı yazarıdır."},
            new Author{Id=2,FirstName="Turgut", LastName="Uyar",DateOfBirth=new DateTime(2024,07,15),About="Ahmet Turgut Uyar, Türk şair. Babasının görevinden ötürü ilköğrenimi farklı şehirlerde okurken ortaöğrenimine yatılı askerî okulda okuyarak devam eden Uyar, 1948'de Kaynak dergisinin başlatmış olduğu bir şiir yarışmasında \"Arz-ı Hal\" adlı şiiriyle katılmış ve yarışmada ikinci olmuştur."},

        };

        public IActionResult Index()

        {

            var list = Authors.Where(x => x.IsDeleted == false).Select(x => new AuthorListViewModel
            {

                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                DateOfBirth = x.DateOfBirth,
                About = x.About,

            }).ToList();
            return View(list);
        }

        [HttpGet]
        public IActionResult Add()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Add(AuthorAddViewModel formData)
        {


            if (!ModelState.IsValid)
            {

                return View(formData);
            }

            int maxId = Authors.Max(x => x.Id);

            var newAuthor = new Author()
            {
                Id = maxId + 1,
                FirstName = formData.FirstName,
                LastName = formData.LastName,
                DateOfBirth = formData.DateOfBirth,
                About = formData.About,
            };

            Authors.Add(newAuthor);


            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            // Bu id ile eşleşen liste elemanını yakala.
            var edit = Authors.Find(x => x.Id == id);

            // Gerekli özelliklerini view'e gönder.
            var viewModel = new AuthorEditViewModel()
            {
                Id = edit.Id,
                FirstName = edit.FirstName,
                LastName = edit.LastName,
                DateOfBirth = edit.DateOfBirth,
                About= edit.About,
            };



            // View'i aç.
            return View(viewModel);


        }


        [HttpPost]
        public IActionResult Edit(AuthorEditViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                return View(formData);
            }

            var edit = Authors.Find(x => x.Id == formData.Id);


            edit.FirstName = formData.FirstName;
            edit.LastName = formData.LastName;
            edit.DateOfBirth = formData.DateOfBirth;
            edit.About = formData.About;
            




            return RedirectToAction("Index");
        }

        public IActionResult DeletedBook(int id)
        {
            var book = Authors.Find(x => x.Id == id);

            //  _tasks.Remove(task); // HARD DELETE -> Veri gitti, bir daha ulaşılamaz.

            book.IsDeleted = true; // SOFT DELETE -> Veri hala duruyor fakat ben listelerken artık isDeleted'ı false olanları listelicem.


            return RedirectToAction("Index");
        }

        public IActionResult Detail(int id)
        {
            var authorDetail = Authors
                     .Where(x => x.Id == id)
                     .Select(x => new AuthorDetailViewModel
                     {
                         Id = x.Id,
                         FirstName = x.FirstName,
                         LastName = x.LastName,
                         DateOfBirth = x.DateOfBirth,
                         About = x.About,
                         // Diğer gerekli alanlar
                     })
                     .FirstOrDefault(); // Tek bir öğe döndürüyor
            ViewBag.Books = BookController.books.Where(x => x.AuthorId == id).ToList(); // Kitap listesini yazarın ID'sine göre alıyoruz
            ViewBag.Genres = GenreController.genres.ToList(); // Türleri tüm liste olarak alıyoruz

            return View(authorDetail);
        }
    }
}
