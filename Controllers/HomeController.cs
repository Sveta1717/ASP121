using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ASP121.Models;
using ASP121.Models.Product;

namespace ASP121.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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