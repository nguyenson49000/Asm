using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TSonCoffee.Models;

public partial class TsonCoffeeContext : DbContext
{
    public TsonCoffeeContext()
    {
    }

    public TsonCoffeeContext(DbContextOptions<TsonCoffeeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<AmountOfIce> AmountOfIces { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<DetailedInvoice> DetailedInvoices { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductType> ProductTypes { get; set; }

    public virtual DbSet<Size> Sizes { get; set; }

    public virtual DbSet<Sweetness> Sweetnesses { get; set; }

    public virtual DbSet<Topping> Toppings { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=TSƠN\\NGUYENSONDEV;Database=TSonCoffee;Trusted_Connection=True; TrustServerCertificate =True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.ToTable("Address");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AddressLine)
                .HasMaxLength(50)
                .HasColumnName("Address_line");
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("Create_At");
            entity.Property(e => e.State).HasMaxLength(50);
            entity.Property(e => e.UserId).HasColumnName("User_ID");

            entity.HasOne(d => d.User).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Address_Users");
        });

        modelBuilder.Entity<AmountOfIce>(entity =>
        {
            entity.ToTable("Amount_Of_Ice");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.ToTable("Cart");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.UserId).HasColumnName("User_ID");

            entity.HasOne(d => d.User).WithMany(p => p.Carts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cart_Users");
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.ToTable("Cart_Items");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AmountOfIceId).HasColumnName("AmountOfIce_ID");
            entity.Property(e => e.CartId).HasColumnName("Cart_ID");
            entity.Property(e => e.ProductsId).HasColumnName("Products_ID");
            entity.Property(e => e.SizeId).HasColumnName("SIze_ID");
            entity.Property(e => e.SweetnessId).HasColumnName("Sweetness_ID");
            entity.Property(e => e.ToppingId).HasColumnName("Topping_ID");

            entity.HasOne(d => d.AmountOfIce).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.AmountOfIceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cart_Items_Amount_Of_Ice");

            entity.HasOne(d => d.Cart).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.CartId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cart_Items_Cart");

            entity.HasOne(d => d.Products).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.ProductsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cart_Items_Products");

            entity.HasOne(d => d.Size).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.SizeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cart_Items_Size");

            entity.HasOne(d => d.Sweetness).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.SweetnessId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cart_Items_Sweetness");

            entity.HasOne(d => d.Topping).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.ToppingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cart_Items_Topping");
        });

        modelBuilder.Entity<DetailedInvoice>(entity =>
        {
            entity.ToTable("Detailed_Invoice");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AmountOfIceId).HasColumnName("AmountOfIce_ID");
            entity.Property(e => e.InvoicesId).HasColumnName("Invoices_ID");
            entity.Property(e => e.ProductsId).HasColumnName("Products_ID");
            entity.Property(e => e.SizeId).HasColumnName("Size_ID");
            entity.Property(e => e.SweetnessId).HasColumnName("Sweetness_ID");
            entity.Property(e => e.ToppingId).HasColumnName("Topping_ID");

            entity.HasOne(d => d.AmountOfIce).WithMany(p => p.DetailedInvoices)
                .HasForeignKey(d => d.AmountOfIceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Detailed_Invoice_Amount_Of_Ice");

            entity.HasOne(d => d.Invoices).WithMany(p => p.DetailedInvoices)
                .HasForeignKey(d => d.InvoicesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Detailed_Invoice_Invoices");

            entity.HasOne(d => d.Products).WithMany(p => p.DetailedInvoices)
                .HasForeignKey(d => d.ProductsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Detailed_Invoice_Products");

            entity.HasOne(d => d.Size).WithMany(p => p.DetailedInvoices)
                .HasForeignKey(d => d.SizeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Detailed_Invoice_Size");

            entity.HasOne(d => d.Sweetness).WithMany(p => p.DetailedInvoices)
                .HasForeignKey(d => d.SweetnessId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Detailed_Invoice_Sweetness");

            entity.HasOne(d => d.Topping).WithMany(p => p.DetailedInvoices)
                .HasForeignKey(d => d.ToppingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Detailed_Invoice_Topping");
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.ToTable("Image");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.UrlImg)
                .HasMaxLength(50)
                .HasColumnName("URL_Img");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AddressId).HasColumnName("Address_ID");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("Create_at");
            entity.Property(e => e.CreateBy).HasColumnName("Create_by");
            entity.Property(e => e.NameShop).HasMaxLength(50);
            entity.Property(e => e.PaymentMethodId).HasColumnName("Payment_Method_ID");
            entity.Property(e => e.Vat).HasColumnName("VAT");

            entity.HasOne(d => d.Address).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.AddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Invoices_Address");

            entity.HasOne(d => d.PaymentMethod).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.PaymentMethodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Invoices_Payment_Method");
        });

        modelBuilder.Entity<PaymentMethod>(entity =>
        {
            entity.ToTable("Payment_Method");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("Create_At");
            entity.Property(e => e.CreateBy).HasColumnName("Create_By");
            entity.Property(e => e.ImgId).HasColumnName("Img_ID");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.ProductTypeId).HasColumnName("ProductTypeID");
            entity.Property(e => e.UpdateBy).HasColumnName("Update_By");

            entity.HasOne(d => d.Img).WithMany(p => p.Products)
                .HasForeignKey(d => d.ImgId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Products_Image");

            entity.HasOne(d => d.ProductType).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Products_ProductType");
        });

        modelBuilder.Entity<ProductType>(entity =>
        {
            entity.ToTable("ProductType");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Size>(entity =>
        {
            entity.ToTable("Size");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AdditionalAmount).HasColumnName("Additional_Amount");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Sweetness>(entity =>
        {
            entity.ToTable("Sweetness");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Topping>(entity =>
        {
            entity.ToTable("Topping");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AdditionalAmount).HasColumnName("Additional_Amount");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("Create_At");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(50)
                .HasColumnName("Create_By");
            entity.Property(e => e.DateOfBirth).HasColumnName("Date_Of_Birth");
            entity.Property(e => e.FullName).HasMaxLength(50);
            entity.Property(e => e.PassWord).HasMaxLength(50);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(10)
                .HasColumnName("Phone_Number");
            entity.Property(e => e.PurchaseCount).HasColumnName("Purchase_Count");
            entity.Property(e => e.Role).HasMaxLength(50);
            entity.Property(e => e.UserName).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
