using ASP121.Date;
using ASP121.Models.Rate;
using Intercom.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace ASP121.Controllers
{
    [Route("api/rate")]
    [ApiController]
    public class RateController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public RateController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpPost]
        public object DoPost([FromBody] RateModel model)
        {
            var rate = _dataContext.Rates
                .Where(r => r.ProductId == model.ProductId
                && r.UserId == model.UserId)
                .FirstOrDefault();
            if(rate != null) // була оцінка
            {
                return new { Status = false, Message = "Оцінка вже була дана раніше!" };           
                
            }
            else// нова оцінка
            {
                _dataContext.Rates.Add(new()
                {
                    ProductId = model.ProductId,
                    UserId = model.UserId,
                    Rating = model.Rating,
                    Moment = DateTime.Now,
                });

              
                _dataContext.SaveChanges();
                // оновлюємо дані про рейтинг даного товару 
                var product = 
                _dataContext.Products.Include(p => p.Rates)
                    .Where(p => p.Id == model.ProductId)
                    .First();
                // включаємо оновлені дані у відповідь ( для відображення)
                return new {
                    Status = true,
                    Message = "ОК",
                    Positive = product.Rates.Count(r => r.Rating >0),
                    Negative = product.Rates.Count(r => r.Rating <0),
                };
               
            }
           // return model;
           

        }
    }
}
