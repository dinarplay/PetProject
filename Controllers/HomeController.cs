using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PetProject.Models;
using PetProject.Services;

namespace PetProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PassGeneratorService _passGenerator;
        private readonly ApplicationContext db;

        public HomeController(ILogger<HomeController> logger, PassGeneratorService passGenerator, ApplicationContext db)
        {
            _logger = logger;
            _passGenerator = passGenerator;
            this.db = db;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Faq()
        {
            if (HttpContext.Request.Path.ToString().Contains("Home/Faq"))
            {
                return PartialView("_Faq");
            }
            return Redirect("/");
        }
        public IActionResult Contacts()
        {
            if (HttpContext.Request.Path.ToString().Contains("Home/Contacts"))
            {
                Thread.Sleep(2000);
                return PartialView("_Contacts");
            }
            return Redirect("/");
        }
        public IActionResult GetPass()
        {
            if (HttpContext.Request.Path.ToString().Contains("Home/GetPass"))
            {
                ViewBag.Items = new SelectList("С заглавными", "С цифрами", "С символами");
                return PartialView("_PassGenerator");
            }
            return Redirect("/");
        }
        [HttpPost]
        public IActionResult GetPass(int longPass = 16, bool isCaps = false, bool isNumbers = false, bool isSymbols = false)
        {
            return Content(_passGenerator.GeneratePassword(longPass, isCaps, isNumbers, isSymbols));
        }
    }
}
