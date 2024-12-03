using Microsoft.EntityFrameworkCore.Storage;

namespace TSonCoffee.Models
{
    public class DetailedInvoiceViewModel
    {
        public int InvoiceId { get; set; }
        public string Img { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }
        public string Sweetness { get; set; }
        public string AmontofIce { get; set; }
        public string Topping { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
