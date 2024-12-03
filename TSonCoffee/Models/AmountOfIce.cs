using System;
using System.Collections.Generic;

namespace TSonCoffee.Models;

public partial class AmountOfIce
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual ICollection<DetailedInvoice> DetailedInvoices { get; set; } = new List<DetailedInvoice>();
}
