using System;
using System.Collections.Generic;

namespace TSonCoffee.Models;

public partial class Address
{
    public int Id { get; set; }

    public string City { get; set; } = null!;

    public string State { get; set; } = null!;

    public string AddressLine { get; set; } = null!;

    public int UserId { get; set; }

    public DateOnly? CreateAt { get; set; }

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual User User { get; set; } = null!;
}
