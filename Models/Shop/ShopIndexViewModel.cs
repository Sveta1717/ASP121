using ASP121.Date.Entity;

namespace ASP121.Models.Shop
{
    public class ShopIndexViewModel
    {
        public List<Date.Entity.ProductGroup> ProductGroups{ get; set;}
        public List<Date.Entity.Product> Products { get; set; }
        public String? AddMessage { get; set;}
    }
}
