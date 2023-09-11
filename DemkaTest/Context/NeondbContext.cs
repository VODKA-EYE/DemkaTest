using System;
using System.Collections.Generic;
using DemkaTest.Models;
using Microsoft.EntityFrameworkCore;

namespace DemkaTest.Context;

public static class Healper
{
  public static readonly NeondbContext Database = new NeondbContext();
}

public partial class NeondbContext : DbContext
{
    public NeondbContext()
    {
    }

    public NeondbContext(DbContextOptions<NeondbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderPickupPoint> OrderPickupPoints { get; set; }

    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

    public virtual DbSet<OrderedProduct> OrderedProducts { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseLazyLoadingProxies().UseNpgsql("Host=ep-crimson-dust-85081271.eu-central-1.aws.neon.tech;Database=neondb;Username=VODKA-EYE;password=MXJohvpWK51n");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.Companyid).HasName("companies_pkey");

            entity.ToTable("companies");

            entity.Property(e => e.Companyid)
                .HasDefaultValueSql("nextval('company_seq'::regclass)")
                .HasColumnName("companyid");
            entity.Property(e => e.Companyname)
                .HasMaxLength(100)
                .HasColumnName("companyname");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Orderid).HasName("orders_pkey");

            entity.ToTable("orders");

            entity.Property(e => e.Orderid)
                .HasDefaultValueSql("nextval('order_seq'::regclass)")
                .HasColumnName("orderid");
            entity.Property(e => e.Orderclient).HasColumnName("orderclient");
            entity.Property(e => e.Orderdate)
                .HasColumnType("timestamp(0) without time zone")
                .HasColumnName("orderdate");
            entity.Property(e => e.Orderdeliverydate)
                .HasColumnType("timestamp(0) without time zone")
                .HasColumnName("orderdeliverydate");
            entity.Property(e => e.Orderpickupcode).HasColumnName("orderpickupcode");
            entity.Property(e => e.Orderpickuppoint).HasColumnName("orderpickuppoint");
            entity.Property(e => e.Orderstatus).HasColumnName("orderstatus");

            entity.HasOne(d => d.OrderclientNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Orderclient)
                .HasConstraintName("orders_orderclient_fkey");

            entity.HasOne(d => d.OrderpickuppointNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Orderpickuppoint)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orders_orderpickuppoint_fkey");

            entity.HasOne(d => d.OrderstatusNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Orderstatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orders_orderstatus_fkey");
        });

        modelBuilder.Entity<OrderPickupPoint>(entity =>
        {
            entity.HasKey(e => e.Orderpickuppointid).HasName("order_pickup_point_pkey");

            entity.ToTable("order_pickup_point");

            entity.Property(e => e.Orderpickuppointid)
                .HasDefaultValueSql("nextval('order_pickup_point_seq'::regclass)")
                .HasColumnName("orderpickuppointid");
            entity.Property(e => e.Orderpickuppointname)
                .HasMaxLength(200)
                .HasColumnName("orderpickuppointname");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.HasKey(e => e.Statusid).HasName("order_statuses_pkey");

            entity.ToTable("order_statuses");

            entity.Property(e => e.Statusid)
                .HasDefaultValueSql("nextval('order_status_seq'::regclass)")
                .HasColumnName("statusid");
            entity.Property(e => e.Statusname)
                .HasMaxLength(100)
                .HasColumnName("statusname");
        });

        modelBuilder.Entity<OrderedProduct>(entity =>
        {
            entity.HasKey(e => new { e.Orderid, e.Productarticlenumber }).HasName("ordered_products_pkey");

            entity.ToTable("ordered_products");

            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Productarticlenumber)
                .HasMaxLength(100)
                .HasColumnName("productarticlenumber");
            entity.Property(e => e.Productamount).HasColumnName("productamount");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderedProducts)
                .HasForeignKey(d => d.Orderid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ordered_products_orderid_fkey");

            entity.HasOne(d => d.ProductarticlenumberNavigation).WithMany(p => p.OrderedProducts)
                .HasForeignKey(d => d.Productarticlenumber)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ordered_products_productarticlenumber_fkey");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Productarticlenumber).HasName("products_pkey");

            entity.ToTable("products");

            entity.Property(e => e.Productarticlenumber)
                .HasMaxLength(100)
                .HasColumnName("productarticlenumber");
            entity.Property(e => e.Productcategory).HasColumnName("productcategory");
            entity.Property(e => e.Productcost)
                .HasPrecision(19, 4)
                .HasColumnName("productcost");
            entity.Property(e => e.Productdeliveler).HasColumnName("productdeliveler");
            entity.Property(e => e.Productdescription).HasColumnName("productdescription");
            entity.Property(e => e.Productdiscountamount).HasColumnName("productdiscountamount");
            entity.Property(e => e.Productmanufacturer).HasColumnName("productmanufacturer");
            entity.Property(e => e.Productname).HasColumnName("productname");
            entity.Property(e => e.Productphoto).HasColumnName("productphoto");
            entity.Property(e => e.Productquantityinstock).HasColumnName("productquantityinstock");

            entity.HasOne(d => d.ProductcategoryNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.Productcategory)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("products_productcategory_fkey");

            entity.HasOne(d => d.ProductdelivelerNavigation).WithMany(p => p.ProductProductdelivelerNavigations)
                .HasForeignKey(d => d.Productdeliveler)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("products_productdeliveler_fkey");

            entity.HasOne(d => d.ProductmanufacturerNavigation).WithMany(p => p.ProductProductmanufacturerNavigations)
                .HasForeignKey(d => d.Productmanufacturer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("products_productmanufacturer_fkey");
        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.HasKey(e => e.Productcategoryid).HasName("product_categories_pkey");

            entity.ToTable("product_categories");

            entity.Property(e => e.Productcategoryid)
                .HasDefaultValueSql("nextval('product_category_seq'::regclass)")
                .HasColumnName("productcategoryid");
            entity.Property(e => e.Productcategoryname)
                .HasMaxLength(100)
                .HasColumnName("productcategoryname");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Roleid).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.Property(e => e.Roleid)
                .HasDefaultValueSql("nextval('role_seq'::regclass)")
                .HasColumnName("roleid");
            entity.Property(e => e.Rolename)
                .HasMaxLength(100)
                .HasColumnName("rolename");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("users_pkey");

            entity.ToTable("users");

            entity.Property(e => e.Userid)
                .HasDefaultValueSql("nextval('user_seq'::regclass)")
                .HasColumnName("userid");
            entity.Property(e => e.Userlogin).HasColumnName("userlogin");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");
            entity.Property(e => e.Userpassword).HasColumnName("userpassword");
            entity.Property(e => e.Userpatronymic)
                .HasMaxLength(100)
                .HasColumnName("userpatronymic");
            entity.Property(e => e.Userrole).HasColumnName("userrole");
            entity.Property(e => e.Usersurname)
                .HasMaxLength(100)
                .HasColumnName("usersurname");

            entity.HasOne(d => d.UserroleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.Userrole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("users_userrole_fkey");
        });
        modelBuilder.HasSequence("company_seq");
        modelBuilder.HasSequence("order_pickup_point_seq");
        modelBuilder.HasSequence("order_seq");
        modelBuilder.HasSequence("order_status_seq");
        modelBuilder.HasSequence("product_category_seq");
        modelBuilder.HasSequence("role_seq");
        modelBuilder.HasSequence("user_seq");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
