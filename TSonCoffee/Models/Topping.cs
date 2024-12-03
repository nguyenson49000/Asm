using System;
using System.Collections.Generic;

namespace TSonCoffee.Models;

public partial class Topping
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public double AdditionalAmount { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual ICollection<DetailedInvoice> DetailedInvoices { get; set; } = new List<DetailedInvoice>();
}
