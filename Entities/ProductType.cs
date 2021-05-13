using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Entities
{
    public class BaseEntity : IEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
    public interface IEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class ProductType:BaseEntity
    {
        //public Guid ProductTypeId { get; set; }
        public string Name { get; set; }
        public virtual IEnumerable<Product> Products { get; set; }
    }

    public class Product : BaseEntity
    {
        //public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string SKU { get; set; }
        public decimal Price { get; set; }
        public Guid ProductTypeId { get; set; }
        public virtual ProductType ProductType { get; set; }
        public virtual IEnumerable<Variant> Variants { get; set; }
        public virtual IEnumerable<CompositeProduct> CompositeProducts { get; set; }
    }

    public class Variant : BaseEntity
    {
        //public Guid VariantId { get; set; }
        public string Attribute { get; set; }
        public string Value { get; set; }
        public decimal Price { get; set; }
        public string SKU { get; set; }
        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; }
    }

    public class ProductVariantPrice : BaseEntity
    {
        //public Guid ProductVariantPriceId { get; set; }
        public decimal Price { get; set; }
        public string SKU { get; set; }
        public Guid ProductId { get; set; }
        public virtual Product Product { get; set; }
    }

    public class CompositeProduct : BaseEntity
    {
        public Guid ProductId { get; set; }
        //public virtual Product Product { get; set; }
        public Guid AccompanyingProductId { get; set; }
        public virtual Product AccompanyingProduct { get; set; }
        public bool IsExtraCompulsory { get; set; }
        public bool IsExtraCosted { get; set; }
        public string SKU { get; set; }
    }

    public class ApplicationDatabaseContext : DbContext
    {
        public ApplicationDatabaseContext(DbContextOptions<ApplicationDatabaseContext> options) : base(options)
        {

        }

        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Variant> Variants { get; set; }
        public DbSet<ProductVariantPrice> ProductVariantPrices { get; set; }
        public DbSet<CompositeProduct> CompositeProducts { get; set; }
    }
    //public class ProductType
    //{
    //    public Guid ProductTypeId { get; set; }
    //    public string Name { get; set; }
    //    public virtual IEnumerable<Product> Products { get; set; }
    //}

    //public class Product 
    //{
    //    public Guid ProductId { get; set; }
    //    public string Name { get; set; }
    //    public decimal Price { get; set; }
    //    public Guid ProductTypeId { get; set; }
    //    public virtual ProductType ProductType { get; set; }
    //    public virtual IEnumerable<Variant> Variants { get; set; }
    //}

    //public class Variant
    //{
    //    public Guid VariantId { get; set; }
    //    public string Attribute { get; set; }
    //    public Guid ProductId { get; set; }
    //    public virtual Product Product { get; set; }
    //}

    //public class ProductVariantPrice
    //{
    //    public Guid ProductVariantPriceId { get; set; }
    //    public decimal Price { get; set; }
    //    public Guid ProductId { get; set; }
    //    public virtual Product Product { get; set; }
    //}

    //public class CompositeProduct
    //{
    //    public Guid ProductId { get; set; }
    //    public virtual Product Product { get; set; }
    //    public Guid AccompanyingProductId { get; set; }
    //    public virtual Product AccompanyingProduct { get; set; }
    //    public bool IsExtraCompulsory { get; set; }
    //    public bool IsExtraCosted { get; set; }
    //}

    //public class ApplicationDatabaseContext : DbContext
    //{
    //    public ApplicationDatabaseContext(DbContextOptions<ApplicationDatabaseContext> options) : base(options)
    //    {

    //    }

    //    public DbSet<ProductType> ProductTypes { get; set; }
    //    public DbSet<Product> Products { get; set; }
    //    public DbSet<Variant> Variants { get; set; }
    //    public DbSet<ProductVariantPrice> ProductVariantPrices { get; set; }
    //    public DbSet<CompositeProduct> CompositeProducts { get; set; }
    //}

}
