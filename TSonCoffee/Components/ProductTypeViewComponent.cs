using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TSonCoffee.Models;

namespace TSonCoffee.Components
{
    public class ProductTypeViewComponent : ViewComponent
    {
        private readonly TsonCoffeeContext db;

        public ProductTypeViewComponent(TsonCoffeeContext db)
        {
            this.db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync() => View(await db.ProductTypes.ToListAsync());
    }
}
