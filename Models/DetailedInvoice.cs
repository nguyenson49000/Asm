using System;
using System.Collections.Generic;

namespace TSonCoffee.Models;

public partial class DetailedInvoice
{
    public int Id { get; set; }

    public int InvoicesId { get; set; }

    public int ProductsId { get; set; }

    public int SizeId { get; set; }

    public int SweetnessId { get; set; }

    public int AmountOfIceId { get; set; }

    public int ToppingId { get; set; }

    public int Quantity { get; set; }

    public double Price { get; set; }

    public virtual AmountOfIce AmountOfIce { get; set; } = null!;

    public virtual Invoice Invoices { get; set; } = null!;

    public virtual Product Products { get; set; } = null!;

    public virtual Size Size { get; set; } = null!;

    public virtual Sweetness Sweetness { get; set; } = null!;

    public virtual Topping Topping { get; set; } = null!;
}
