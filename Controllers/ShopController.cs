using ASP121.Date;
using ASP121.Date.Entity;
using ASP121.Models.Rate;
using ASP121.Models.Shop;
using ASP121.Models.User;
using ASP121.Services.Hash;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ASP121.Controllers
{
    public class ShopController : Controller
    {
        private readonly DataContext _dataContext;

        public ShopController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IActionResult Index()
        {
            ShopIndexViewModel model = new()
            {
                ProductGroups = _dataContext.ProductGroups
                .Where(g => g.DeleteDt == null).ToList(),
                Products = _dataContext.Products
                .Include(p => p.Rates)   // заповнює навігаційну властивість (включає до запиту)
                .Where(g => g.DeleteDt == null).ToList(),                

            };
            if(HttpContext.Session.Keys.Contains("AddMessage"))
            {
                model.AddMessage = HttpContext.Session.GetString("AddMessage");
                HttpContext.Session.Remove("AddMessage");
            }
            return View(model);
        }
        [HttpPost]
        public RedirectToActionResult AddProduct(ShopIndexFormModel model)
        {
            // перевіряемо модель, зберігаємо файл, додаємо до БД, повертаємо повідомлення
            String errorMessage = _ValidateModel(model);
            if (errorMessage != String.Empty) 
            {
                // є помилка валідації
                HttpContext.Session.SetString("AddMessage", errorMessage);
            }
            else
            {
                // перевірка успішна
                HttpContext.Session.SetString("AddMessage", "Додано успішно!");
            }
            return RedirectToAction(nameof(Index));
        }
       
        private String _ValidateModel(ShopIndexFormModel? model)
        {
            if (model == null) { return "Дані не передані"; }
            if (String.IsNullOrEmpty(model.Title)) { return "Назва не може бути порожньою"; }
            if (string.IsNullOrEmpty(model.Description)) { return "Опис не може бути порожнім"; }

            if (model.Price == 0)
            {
                // можливо, помилка декодування через локалізацію (1.5/1,5)
                model.Price = Convert.ToSingle(
                    Request.Form["productPrice"].First()?.Replace(',', '.'),
                    CultureInfo.InvariantCulture);
                if (model.Price <= 0) { return "Ціна повинна бути більшою за 0"; }
            }
           
            // завантажуємо файл
            String? newName = null;
            if (model.Image != null)  // є файл
            {
                // визначаємо тип (розширення) файлу
                String ext = Path.GetExtension(model.Image.FileName);

                // Д.З. Перевірити тип файлу (картинка товару) на допустимий перелік
                string[] allowedExtensions = { ".jpg", ".jpeg", ".png" }; // Допустимі розширення файлів
                if (!allowedExtensions.Contains(ext))
                {
                    return "Недопустимий тип файлу. Дозволені лише файли з розширеннями .jpg, .jpeg, .png";
                }

                // змінюємо ім'я файлу - це запобігає випадковому перезапису
                newName = Guid.NewGuid().ToString() + ext;

                // зберігаємо файл - в окрему папку wwwroot/uploads (створюємо вручну)
                model.Image.CopyTo(
                    new FileStream(
                        $"wwwroot/uploads/{newName}",
                        FileMode.Create));
            }
            else { return "Файл-картинка необхідний"; }            

            _dataContext.Products.Add(new Date.Entity.Product
            {
                Id = Guid.NewGuid(),
                Title = model.Title,                
                Description = model.Description,
                ProductGroupId= model.ProductGroupId,
                CreateDt = DateTime.Now,
                Price= model.Price,
                ImageUri = newName               
            });
            // зберігаємо внесені зміни
            _dataContext.SaveChanges();  // PlanetScale не підтримує асинхронні запити

            return String.Empty;
        }
    }
}

// Д.З. Перевірити тип файлу (картинка товару) на допустимий перелік
// Доповнити валідацію (ціна більша за 0 і т.п.)
// Наповнити БД товарами (із розподілом по групам)

