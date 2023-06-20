using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ASP121.Models;
using ASP121.Models.Product;
using ASP121.Services.Hash;

namespace ASP121.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHashService _hashService;

        public HomeController(ILogger<HomeController> logger, IHashService hashService)
        {
            _logger = logger;
            _hashService = hashService;
        }
         public ViewResult Azure()
         {
            return View();
         }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Intro()
        {
            return View();
        }

        public IActionResult Basics()
        {
            Models.Home.BasicModel model = new()
            {
                Moment = DateTime.Now
            };
            return View(model);
        }

        public IActionResult Razor()
        {
            Models.Home.RazorModel model = new()
            {
                IntValue = 10,
                BoolValue = true,
                ListValue = new() { "String 1", "String 2", "String 3", "String 4" },

            };
            return View(model);
        }

        public ViewResult API()
        {
            return View();
        }

        public ViewResult Services()
        {
            ViewData["hash"] = _hashService.HashString("123");
            ViewData["obj"] = _hashService.GetHashCode();
            ViewData["ctr"] = this.GetHashCode();
            return View();
        }

        public ViewResult Sessions(String? userstring)
        {
            if (userstring != null)      // є дані від форми
            {
                HttpContext.Session.SetString("StoredString", userstring);
            }
            if (HttpContext.Session.Keys.Contains("StoredString"))
            {
               
                ViewData["StoredString"] = HttpContext.Session.GetString("StoredString");
            }
            else
            {
                ViewData["StoredString"] = "У сесії немає збережених даних";
            }

            if (HttpContext.Session.Keys.Contains("Form2String"))
            {

                ViewData["Form2String"] = HttpContext.Session.GetString("Form2String");
            }
            else
            {
                ViewData["Form2String"] = "У сесії немає збережених даних";
            }
            return View();
        }

        public RedirectToActionResult SessionsForm(String? userstring)
        {
            //цей метод приймає дані від другої форми і надсилає Redirect
            //Але для того, щоб дані "userstring" були доступні після перезапиту,
            // їх необхідно зберегти у сесії
            HttpContext.Session.SetString("Form2String", userstring!);
            return RedirectToAction("Sessions");
            /* Sessions       userstring
            *  Form1 -----------------------> Sessions -> HTML (/sessions?userstring=123)
            *  
            *  
            *                userstring
            *  Form2 -----------------------> SessionsForm -> 302 (Redirect)
            *           redirect to Sessions
            *        <-----------------------      Сесія зберігає дані між запитами
            *  Browser     -(немає даних)-
            *        -----------------------> Sessions -> HTML (/sessions)
            */
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Products()
        {
            var products = new List<Product>
        {
            new Product
            {
                Name = "Ноутбук Lenovo",
                Price = 18999,
                Image = "/img/laptop.jpg",
                Description = "Ультралегкий ноутбук Lenovo YOGA Slim 7i 14ITL05 " +
                "Slate Grey (82A300KNRA) створений для креативу та розваг. " +
                "Приголомшливе відтворення графіки та аудіо, потужна \"начинка\" " +
                "і вага всього 1,36 кг, до 10 годин автономності - бери пристрій " +
                "із собою та насолоджуйся кожною деталлю контенту."
            },
            new Product
            {
                Name = "Смартфон Samsung",
                Price = 9999,
                Image = "/img/phone.jpg",
                Description = "Екран (6.6\", TFT, 2408x1080) / Samsung Exynos 1280 " +
                "(2.0 ГГц + 2.4 ГГц) / основна квадрокамера: 50 Мп + 5 Мп + 2 Мп + 2 Мп, " +
                "фронтальна камера: 8 Мп / RAM 6 ГБ вбудованої пам'яті + microSD (до 1 ТБ) / " +
                "3G / LTE / 5G / GPS / підтримка 2х SIM-карт (Nano-SIM) / Android 12 / 5000 мА * год."
            },
            new Product
            {
                Name = "Телевізор LG",
                Price = 30999,
                Image = "/img/tv.jpg",
                Description = "Удосконалені функції підвищення роздільної здатності до 4K і " +
                "технологією зіставлення відтінків AI Tone Mapping покращують контрастність і " +
                "роздільну здатність для оптимальної деталізації, а функції покращення ефекту " +
                "динамічної яскравості збільшують глибину різкості та виразність кольорів."
            }
        };
            var model = new ProductModel
            {
                Products = products
            };

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}