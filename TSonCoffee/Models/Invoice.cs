using System;
using System.Collections.Generic;

namespace TSonCoffee.Models;

public partial class Invoice
{
    public int Id { get; set; }

    public string? NameShop { get; set; }

    public double TemporaryTotalAmount { get; set; }

    public int AddressId { get; set; }

    public double ShippingFee { get; set; }

    public double Vat { get; set; }

    public int PaymentMethodId { get; set; }

    public DateOnly CreateAt { get; set; }

    public int CreateBy { get; set; }

    public bool Status { get; set; }

    public double TotalAmount { get; set; }

    public virtual Address Address { get; set; } = null!;

    public virtual ICollection<DetailedInvoice> DetailedInvoices { get; set; } = new List<DetailedInvoice>();

    public virtual PaymentMethod PaymentMethod { get; set; } = null!;
}
