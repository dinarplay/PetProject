using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetProject.Models;
using System.Security.Claims;

namespace PetProject.Controllers
{
    public class AuthController : Controller
    {
        ApplicationContext db;
        public AuthController(ApplicationContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string? returnUrl, UserLogin user)
        {
            if (ModelState.IsValid)
            {
                // находим пользователя 
                var currentUser = db.Users.Join(db.Roles,
                    u => u.RoleId,
                    r => r.Id,
                    (u,r) => new
                    {
                        Id = u.Id,
                        Name = u.Name,
                        Email = u.Email,
                        Password = u.Password,
                        Age = u.Age,
                        CreatedDate = u.CreatedDate,
                        UserRole = r.Name
                    })
                    .FirstOrDefault(p => p.Email == user.Email && p.Password == user.Password);
                // если пользователь не найден, отправляем статусный код 401
                if (currentUser is null)
                {
                    return Unauthorized();
                }

                var claims = new List<Claim> { 
                    new Claim("Name", currentUser.Name),
                    new Claim("Email", currentUser.Email),
                    new Claim("Age", currentUser.Age.ToString()),
                    new Claim("Created date", currentUser.CreatedDate.ToString()),
                    new Claim("Role", currentUser.UserRole)
                };
                // создаем объект ClaimsIdentity
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                // установка аутентификационных куки
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                return Redirect(returnUrl ?? "/");
            }
            return BadRequest("Email и/или пароль не введены");
        }

        [HttpGet]
        public IActionResult Registration()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            return View();
        }
        [HttpPost]
        public IActionResult Registration(User user)
        {
            if (ModelState.IsValid)
            {
                User? someUser = db.Users.FirstOrDefault(u => u.Email == user.Email);
                if (someUser is not null)
                {
                    return Content("Данный email уже занят");
                }
                user.CreatedDate = DateTime.Now;
                user.RoleId = 2;
                db.Users.Add(user);
                db.SaveChanges();
                return Redirect("/");
            }
            return Content("Error");

            //string errorMessages = "";
            //// проходим по всем элементам в ModelState
            //foreach (var item in ModelState)
            //{
            //    // если для определенного элемента имеются ошибки
            //    if (item.Value.ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
            //    {
            //        errorMessages = $"{errorMessages}\nОшибки для свойства {item.Key}:\n";
            //        // пробегаемся по всем ошибкам
            //        foreach (var error in item.Value.Errors)
            //        {
            //            errorMessages = $"{errorMessages}{error.ErrorMessage}\n";
            //        }
            //    }
            //}
            //return Content(errorMessages);
        }

        /// <summary>
        /// 
        /// </summary>

        [Authorize]
        public IActionResult About()
        {
            return View();
        }
        [Authorize(Roles="Admin")]
        public IActionResult AdminPanel()
        {
            return View();
        }
    }
}
