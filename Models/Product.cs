using System;
using System.Collections.Generic;

namespace TSonCoffee.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public double Price { get; set; }

    public int ImgId { get; set; }

    public DateOnly? CreateAt { get; set; }

    public int? CreateBy { get; set; }

    public int? UpdateBy { get; set; }

    public bool Status { get; set; }

    public int ProductTypeId { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual ICollection<DetailedInvoice> DetailedInvoices { get; set; } = new List<DetailedInvoice>();

    public virtual Image Img { get; set; } = null!;

    public virtual ProductType ProductType { get; set; } = null!;
}
