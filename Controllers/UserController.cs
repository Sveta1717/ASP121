using ASP121.Date;
using ASP121.Models.User;
using ASP121.Services.Hash;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Text.RegularExpressions;

namespace ASP121.Controllers
{
    public class UserController : Controller
    {
        // Підключення БД - інжекція залежності від контексту (зареєстрованого у Program.cs)
        private readonly DataContext _dataContext;
        private readonly IHashService _hashService;

        public UserController(DataContext dataContext, IHashService hashService)
        {
            _dataContext = dataContext;
            _hashService = hashService;
        }

        public IActionResult SignUp(UserSignupModel? model)
        {
            if (HttpContext.Request.Method == "POST")   // є передані з форми дані
            {
                ViewData["form"] = _ValidateModel(model);
            }
            return View(model);
        }

        private bool _LoginUnique(string login)
        {
            return _dataContext.Users121.All(user => user.Login != login);
        }

        // Перевіряє валідність даних у моделі, прийнятої з форми
        // Повертає повідомлення про помилку валідації або String.Empty
        // якщо перевірка успішна
        private String _ValidateModel(UserSignupModel? model)
        {
            if (model == null) { return "Дані не передані"; }
            if (String.IsNullOrEmpty(model.Login)) { return "Логін не може бути порожним"; }
            if (String.IsNullOrEmpty(model.Password)) { return "Пароль не може бути порожним"; }
            if (String.IsNullOrEmpty(model.Email)) { return "Email не може бути порожним"; }
            if (!model.Agree) { return "Необхідно дотримуватись правил сайту"; }
            // завантажуємо файл-аватарку
            String? newName = null;
            if (model.AvatarFile != null)  // є файл
            {
                // визначаємо тип (розширення) файлу
                String ext = Path.GetExtension(model.AvatarFile.FileName);

                // Д.З. Перевірити тип файлу на допустимий перелік
                string[] allowedExtensions = { ".jpg", ".jpeg", ".png" }; // Допустимі розширення файлів
                if (!allowedExtensions.Contains(ext))
                {
                    return "Недопустимий тип файлу. Дозволені лише файли з розширеннями .jpg, .jpeg, .png";
                }

                // змінюємо ім'я файлу - це запобігає випадковому перезапису
                newName = Guid.NewGuid().ToString() + ext;

                // зберігаємо файл - в окрему папку wwwroot/uploads (створюємо вручну)
                model.AvatarFile.CopyTo(
                    new FileStream(
                        $"wwwroot/uploads/{newName}",
                        FileMode.Create));
            }

            // Перевірка на однаковість паролю та його повтору
            if (!String.Equals(model.Password, model.RepeatPassword)) { return "Паролі не однакові"; }

            // Перевірка паролю на складність
            if (model.Password.Length < 8 ||
                !model.Password.Any(char.IsDigit) ||
                !model.Password.Any(char.IsUpper) ||
                !model.Password.Any(char.IsLower)) 
            {
                return "Пароль повинен містити щонайменше 8 символів, цифри, великі та маленькі літери";
            }

            // Перевірка Email регулярним виразом
            string emailUnique = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            if (!Regex.IsMatch(model.Email, emailUnique))
            {
                return "Невірний формат Email";
            }

            // перевірка Логіна на те, що такий вже використовується
            if (!_LoginUnique(model.Login))
            {
                return "Цей логін вже використовується";
            }

            // додаємо користувача до БД
            _dataContext.Users121.Add(new Date.Entity.User
            {
                Id = Guid.NewGuid(),
                Login = model.Login,
                PasswordHash = _hashService.HashString(model.Password),
                Email = model.Email,
                Avatar = newName,
                RealName = model.RealName,
                RegisteredDt = DateTime.Now,
            });
            // зберігаємо внесені зміни
            _dataContext.SaveChanges();  // PlanetScale не підтримує асинхронні запити

            return String.Empty;
        }
    }
}
/* Д.З. Валідація: 
 *       - додати перевірку на однаковість паролю та його повтору
 *       - перевірити тип файлу на допустимий перелік
 *       - * перевірити Email регулярним виразом
 *       - * перевірити пароль на складність
 *       - перевірити Логін на те, що такий вже використовується
 */