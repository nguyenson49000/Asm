using System;
using System.Collections.Generic;

namespace TSonCoffee.Models;

public partial class Image
{
    public int Id { get; set; }

    public string UrlImg { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
