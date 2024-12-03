using System;
using System.Collections.Generic;

namespace TSonCoffee.Models;

public partial class User
{
    public int Id { get; set; }

    public string UserName { get; set; } = null!;

    public string PassWord { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public int PurchaseCount { get; set; }

    public string? Role { get; set; }

    public DateOnly? CreateAt { get; set; }

    public string? CreateBy { get; set; }

    public bool Status { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();
}
