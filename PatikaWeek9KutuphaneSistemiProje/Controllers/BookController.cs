using Microsoft.AspNetCore.Mvc;
using PatikaWeek9KutuphaneSistemiProje.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Cryptography.X509Certificates;
namespace PatikaWeek9KutuphaneSistemiProje.Controllers
{
    public class BookController : Controller
    {
        public static List<Book> books = new List<Book>()
        {
            new Book{Id=1,Title="İki Şehrin Hikayesi",AuthorId= 1, CopiesAvailable = 10000,PublishDate = new DateTime(1994,12,10),Isbn= "112233 ",GenreId=1,Summary="Manette'in tesadüfen Londra'ya dönüşü sırasında tanıştıkları bir Fransız olan Charles Darnay ile kızının yapacakları evlilik ve bunun ardından meydana gelen Fransız İhtilali'nin hayatlarına etkileri anlatılır. Bu insanların ruhsal değişimlerini ele alan kitapta birçok tarihi izlere de rastlayabiliriz."},
            new Book{Id=2,Title="Hamamböcekleri",AuthorId= 1, CopiesAvailable = 100000,PublishDate = new DateTime(2005,12,10),Isbn= "115566 ", GenreId=1,Summary="Tayland'daki Norveç Büyükelçisi Molnes ucuz bir motel odasında ölü bulunmuştur. Harry Hole cinayeti araştırmak ve gerekirse gerçeklerin üstünü örtmek için Oslo'dan Bangkok'a gönderilir. Molnes'in arabasında bulunan fotoğraflar cinayetin hiç de kolay çözülmeyeceğini gösterir gibidir."},
            new Book{Id=3,Title="Şu Çılgın Türkler",AuthorId= 2, CopiesAvailable = 50000,PublishDate = new DateTime(2015,08,05),Isbn= "2266778 ", GenreId=2,Summary="Şu Çılgın Türkler, 1921-1922 yıllarındaki Kurtuluş Mücadelesi'ni konu ediniyor. Kitap, savaş öncesinde, 1914 ve 1920 yılları arasındaki süreçte yaşanan tarihi olayları özetleyen bir giriş ile başlıyor."},
            new Book{Id=4,Title="Sefiller",AuthorId= 2, CopiesAvailable = 6000000,PublishDate = new DateTime(1945,06,25),Isbn= "96857452 ", GenreId=1,Summary="Ailesine ekmek götürebilmek için hırsızlık yapan ve bu yüzden kürek mahkûmiyetine çarptırılan bir adamın hikâyesi. Aldığı ağır cezanın bedelini ömrü boyunca ödeyen Jean Valjean'ı merkezine alan roman, yoksulluğu, toplumsal adaleti ve dayanışmayı anlatıyor."},
            new Book{Id=5,Title="1984",AuthorId= 2, CopiesAvailable = 650000,PublishDate = new DateTime(1999,05,28),Isbn= "3658741 ", GenreId=1,Summary="Okyanusya adlı hayali bir ülkede geçen romanda, baskıcı bir devlet ile bu devleti yıkmak isteyen özgürlükçü insanların mücadelesi anlatılıyor. Her şeyin yasak olduğu ve herkesin gözetim altında tutulduğu bir dünyada, isyan bayrağını dalgalandıran bir karakterdir."},
            new Book{Id=6,Title="Adana",AuthorId= 2, CopiesAvailable = 650000,PublishDate = new DateTime(1999,05,28),Isbn= "3658741 ",GenreId=3,Summary="Adana İşte :)"}
        };


        public IActionResult Index()
        {
            // AuthorController'daki static Authors listesine erişiyoruz
            ViewBag.Authors = AuthorController.Authors;
            ViewBag.Genres = GenreController.genres; // Genre listesine erişiyoruz

            var list = books.Where(x => x.IsDeleted == false)
                .Join(
                    AuthorController.Authors, // Yazar listesi ile join
                    book => book.AuthorId,
                    author => author.Id,
                    (book, author) => new { book, author }
                )
                .Join(
                    GenreController.genres, // Tür listesi ile join
                    bookAuthor => bookAuthor.book.GenreId, // Kitapların GenreId ile türlerin GenreId'sini eşliyoruz
                    genre => genre.GenreId,
                    (bookAuthor, genre) => new { bookAuthor.book, bookAuthor.author, genre } // Kitap, yazar ve türü birleştiriyoruz
                )
                .Select(x => new BookListViewModel
                {
                    Id = x.book.Id,
                    Title = x.book.Title,
                    AuthorId = x.book.AuthorId,
                    GenreName = x.genre.GenreName, 
                    Isbn = x.book.Isbn,
                    PublishDate = x.book.PublishDate,
                    AuthorName = x.author.FirstName + " " + x.author.LastName,
                    Summary = x.book.Summary,
                    


                })
                .ToList();


            return View(list);  // Listeyi view'e gönderiyoruz
        }

        [HttpGet]
        public IActionResult Add()
        {
            // Yazar listesini ve Tür listesini ViewBag ile view'e gönderiyoruz
            ViewBag.Genres = GenreController.genres;
            ViewBag.Authors = AuthorController.Authors;
            return View();
        }


        [HttpPost]
        public IActionResult Add(BookAddViewModel formData)
        {


            if (!ModelState.IsValid)
            {
                ViewBag.Genres = GenreController.genres;
                ViewBag.Authors = AuthorController.Authors;
                return View(formData);
            }

            int maxId = books.Max(x => x.Id);

            var newBook = new Book()
            {
                Id = maxId + 1,
                Title = formData.Title,
                CopiesAvailable = formData.CopiesAvailable,
                PublishDate = DateTime.Now,
                GenreId = formData.GenreId,
                Isbn = formData.Isbn,
                AuthorId = formData.AuthorId,
                IsDeleted = false,
                Summary = formData.Summary,
                




            };

            books.Add(newBook);


            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            // Bu id ile eşleşen liste elemanını yakala.
            var edit = books.Find(x => x.Id == id);

            // Gerekli özelliklerini view'e gönder.
            var viewModel = new BookEditViewModel()
            {
                Id = edit.Id,
                Title = edit.Title,
                AuthorId = edit.AuthorId,
                GenreId = edit.GenreId,
                Isbn = edit.Isbn,
                CopiesAvailable = edit.CopiesAvailable,
                PublishDate = edit.PublishDate,
                Summary = edit.Summary,
            };
            ViewBag.Genres = GenreController.genres;
            ViewBag.Owners = AuthorController.Authors;

            // View'i aç.
            return View(viewModel);


        }


        [HttpPost]
        public IActionResult Edit(BookEditViewModel formData)
        {
            ViewBag.Genres = GenreController.genres;
            ViewBag.Owners = AuthorController.Authors;
            if (!ModelState.IsValid)
            {
                return View(formData);
            }

            var task = books.Find(x => x.Id == formData.Id);

            task.Title = formData.Title;
            task.AuthorId = formData.AuthorId;
            task.GenreId = formData.GenreId;
            task.Isbn = formData.Isbn;
            task.CopiesAvailable = formData.CopiesAvailable;
            task.PublishDate = formData.PublishDate;
            task.IsDeleted = false;
            task.Summary = formData.Summary;




            return RedirectToAction("Index");
        }

        public IActionResult DeletedBook(int id)
        {
            var book = books.Find(x => x.Id == id);

            

            book.IsDeleted = true; 


            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Detail(int id)
        { // Bu id ile eşleşen liste elemanını yakala.
            var edit = books.Find(x => x.Id == id);

            // Gerekli özelliklerini view'e gönder.
            var viewModel = new BookDetailViewModel()
            {
                Id = edit.Id,
                Title = edit.Title,
                AuthorId = edit.AuthorId,
                GenreId = edit.GenreId,
                Isbn = edit.Isbn,
                CopiesAvailable = edit.CopiesAvailable,
                PublishDate = edit.PublishDate,
                Summary = edit.Summary,
            };
            ViewBag.Genres = GenreController.genres;
            ViewBag.Owners = AuthorController.Authors;

            // View'i aç.
            return View(viewModel);
        }

        public IActionResult NameFilter()
        {


            ViewBag.Authors = AuthorController.Authors;// Yazar listesine erişiyoruz
            ViewBag.Genres = GenreController.genres; // Genre listesine erişiyoruz

            var list = books.Where(x => x.IsDeleted == false)
                .Join(
                    AuthorController.Authors, // Yazar listesi ile join
                    book => book.AuthorId,
                    author => author.Id,
                    (book, author) => new { book, author }
                )
                .Join(
                    GenreController.genres, // Tür listesi ile join
                    bookAuthor => bookAuthor.book.GenreId, // Kitapların GenreId ile türlerin GenreId'sini eşliyoruz
                    genre => genre.GenreId,
                    (bookAuthor, genre) => new { bookAuthor.book, bookAuthor.author, genre } // Kitap, yazar ve türü birleştiriyoruz
                )
                .Select(x => new BookListViewModel
                {
                    Id = x.book.Id,
                    Title = x.book.Title,
                    AuthorId = x.book.AuthorId,
                    GenreName = x.genre.GenreName, // Tür ismini alıyoruz
                    Isbn = x.book.Isbn,
                    PublishDate = x.book.PublishDate,
                    AuthorName = x.author.FirstName + " " + x.author.LastName,
                    Summary = x.book.Summary,
                }).OrderBy(x => x.Title)
                .ToList();
            // Kitap başlığına (Title) göre alfabetik sıralama (A-Z)



            return View("Index", list); // Sıralanmış ve dönüştürülmüş modeli gönderiyoruz
        }


        public IActionResult AuthorFilter()
        {

            
            ViewBag.Authors = AuthorController.Authors;//Yazar listesine
            ViewBag.Genres = GenreController.genres; // Genre listesine erişiyoruz

            var list = books.Where(x => x.IsDeleted == false)
                .Join(
                    AuthorController.Authors, // Yazar listesi ile join
                    book => book.AuthorId,
                    author => author.Id,
                    (book, author) => new { book, author }
                )
                .Join(
                    GenreController.genres, // Tür listesi ile join
                    bookAuthor => bookAuthor.book.GenreId, // Kitapların GenreId ile türlerin GenreId'sini eşliyoruz
                    genre => genre.GenreId,
                    (bookAuthor, genre) => new { bookAuthor.book, bookAuthor.author, genre } // Kitap, yazar ve türü birleştiriyoruz
                )
                .Select(x => new BookListViewModel
                {
                    Id = x.book.Id,
                    Title = x.book.Title,
                    AuthorId = x.book.AuthorId,
                    GenreId = x.genre.GenreId, // Tür ID'sini alıyoruz
                    GenreName = x.genre.GenreName, // Tür ismini alıyoruz
                    Isbn = x.book.Isbn,
                    PublishDate = x.book.PublishDate,
                    AuthorName = x.author.FirstName + " " + x.author.LastName,
                    Summary = x.book.Summary,
                })
                .ToList();
            return View(list);
        }

        public IActionResult GenreFilter()
        {



            // AuthorController'daki static Authors listesine erişiyoruz
            ViewBag.Authors = AuthorController.Authors;
            ViewBag.Genres = GenreController.genres; // Genre listesine erişiyoruz

            var list = books.Where(x => x.IsDeleted == false)
                .Join(
                    AuthorController.Authors, // Yazar listesi ile join
                    book => book.AuthorId,
                    author => author.Id,
                    (book, author) => new { book, author }
                )
                .Join(
                    GenreController.genres, // Tür listesi ile join
                    bookAuthor => bookAuthor.book.GenreId, // Kitapların GenreId ile türlerin GenreId'sini eşliyoruz
                    genre => genre.GenreId,
                    (bookAuthor, genre) => new { bookAuthor.book, bookAuthor.author, genre } // Kitap, yazar ve türü birleştiriyoruz
                )
                .Select(x => new BookListViewModel
                {
                    Id = x.book.Id,
                    Title = x.book.Title,
                    AuthorId = x.book.AuthorId,
                    GenreId = x.genre.GenreId, // Tür ID'sini alıyoruz
                    GenreName = x.genre.GenreName, // Tür ismini alıyoruz
                    Isbn = x.book.Isbn,
                    PublishDate = x.book.PublishDate,
                    AuthorName = x.author.FirstName + " " + x.author.LastName,
                    Summary = x.book.Summary,
                })
                .ToList();




            return View(list);


        }

        public IActionResult Library(int id)
        {
            var book = books.Find(x => x.Id == id);

            
            book.IsMyLibrary = true; 


            return RedirectToAction("Index");
        }

        public IActionResult MyLibrary()
        {

            // AuthorController'daki static Authors listesine erişiyoruz
            ViewBag.Authors = AuthorController.Authors;
            ViewBag.Genres = GenreController.genres; // Genre listesine erişiyoruz

            var list = books.Where(x => x.IsMyLibrary == true)
                .Join(
                    AuthorController.Authors, // Yazar listesi ile join
                    book => book.AuthorId,
                    author => author.Id,
                    (book, author) => new { book, author }
                )
                .Join(
                    GenreController.genres, // Tür listesi ile join
                    bookAuthor => bookAuthor.book.GenreId, // Kitapların GenreId ile türlerin GenreId'sini eşliyoruz
                    genre => genre.GenreId,
                    (bookAuthor, genre) => new { bookAuthor.book, bookAuthor.author, genre } // Kitap, yazar ve türü birleştiriyoruz
                )
                .Select(x => new BookListViewModel
                {
                    Id = x.book.Id,
                    Title = x.book.Title,
                    AuthorId = x.book.AuthorId,
                    GenreName = x.genre.GenreName, // Tür ismini alıyoruz
                    Isbn = x.book.Isbn,
                    PublishDate = x.book.PublishDate,
                    AuthorName = x.author.FirstName + " " + x.author.LastName,
                    Summary = x.book.Summary,
                })
                .ToList();


            return View(list);  // Listeyi view'e gönderiyoruz
        }



    }
}



