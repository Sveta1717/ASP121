namespace ASP121.Date.Entity
{
    public class Product
    {
        public Guid     Id             { get; set; }
        public Guid     ProductGroupId { get; set; }
        public String   Title          { get; set; } = null!;
        public String?   Description   { get; set; }
        public DateTime? DeleteDt      { get; set; }
        public DateTime? CreateDt      { get; set;}
        public String    ImageUri      { get; set; } = null!;
        public float      Price       { get; set; }
        
        // Navigation properties
        public List<Rate> Rates { get; set; } = null!;
    }
}
