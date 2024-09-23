using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using PatikaWeek9KutuphaneSistemiProje.Models;
using System.Security.Claims;

namespace PatikaWeek9KutuphaneSistemiProje.Controllers
{
    public class AuthController : Controller
    {

        public static List<User> users = new List<User>()
        {
            new User{Id = 1 , Email=",", FullName =",", JoinDate=DateTime.Now, Password=".", PhoneNumber="."}
        };

        private readonly IDataProtector _dataProtector;

        public AuthController (IDataProtectionProvider dataProtectionProvider)
        {
            _dataProtector = dataProtectionProvider.CreateProtector("security");
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
      
        
        [HttpPost]
        public IActionResult SignUp(SignUpViewModel formData)
        {

            if (!ModelState.IsValid)
            {
                return View(formData);
            }

            var user = users.FirstOrDefault(x => x.Email.ToLower() == formData.Email.ToLower());
            if (user is not null)
            {
                ViewBag.Error = "Kullanıcı mevcut";
                return View(formData);
            }

            var newUser = new User()
            {
                Id =users.Max(x => x.Id) + 1,
                Email = formData.Email.ToLower(),
                Password = _dataProtector.Protect(formData.Password),
                FullName =formData.FullName,
                PhoneNumber = formData.PhoneNumber
            };

            users.Add(newUser);

            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult SignIn()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel formData)
        {
            var user = users.FirstOrDefault(x => x.Email.ToLower() == formData.Email.ToLower());

            if (user is null)
            {
                ViewBag.Error = "Kullanıcı adı veya şifre hatalı";
                return View(formData);
            }
            
            var rawPassword = _dataProtector.Unprotect(user.Password);

            if (rawPassword == formData.Password)
            {
                // Oturum Açma işlemleri.

                var claims = new List<Claim>();

                claims.Add(new Claim("email", user.Email));
                claims.Add(new Claim("id", user.Id.ToString()));

                var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                // Claims içerisindeki verilerle bir oturum açılacağı için yukarıdaki identity nesnesi tanımlandı.

                var autProperties = new AuthenticationProperties
                {
                    AllowRefresh = true, // yenilenebilir oturum
                    ExpiresUtc = new DateTimeOffset(DateTime.Now.AddHours(48)) // oturum açıldıktan sonra 48 saat geçerli.
                };


                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIdentity), autProperties);

                // await asenkronize (eşzamansız) yapılan işlemlerin birbirini beklemesi için kullanılır.
                // Burada oturum açma işlemi hem projmize hem de internete/browsera etc. bağlı
                // Asenkron metotlar geriye promise döner.

            }
            else
            {
                ViewBag.Error = "Kullanıcı adı veya şifre hatalı";
                return View(formData);
            }






            return RedirectToAction("Index", "Book");
        }

        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("Index", "Book");
        }
    }
}
    

