namespace ASP121.Date.Entity
{
    public class ProductGroup
    {
        public Guid      Id          { get; set; }
        public String    Title       { get; set; } = null!;
        public String?   Description { get; set; }
        public DateTime? DeleteDt    { get; set; }
    }
}
