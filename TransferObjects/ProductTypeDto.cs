using System;
using System.Collections.Generic;

namespace TransferObjects
{
    public class BaseEntityDto : IEntityDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
    public interface IEntityDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class ProductTypeDto: BaseEntityDto
    {
        //public Guid ProductTypeId { get; set; }
        public string Name { get; set; }
        public virtual IEnumerable<ProductDto> Products { get; set; }
    }
    public class DisplayProductTypeDto: BaseEntityDto
    {
        public string Name { get; set; }
    }

    public class ProductDto: BaseEntityDto
    {
        //public Guid ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string SKU { get; set; }
        public Guid ProductTypeId { get; set; }
        public ProductTypeDto ProductType { get; set; }
        public IEnumerable<VariantDto> Variants { get; set; }
        public IEnumerable<CompositeProductDto> CompositeProducts { get; set; }
    }

    public class DisplayProductDto: BaseEntityDto
    {
        //public Guid ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string SKU { get; set; }
        public Guid ProductTypeId { get; set; }
        public DisplayProductTypeDto ProductType { get; set; }
        public IEnumerable<DisplayVariantDto> Variants { get; set; }
        public IEnumerable<DisplayCompositeProductDto> CompositeProducts { get; set; }
    }

    public class CreateProductDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string SKU { get; set; }
        public Guid ProductTypeId { get; set; }
        public IEnumerable<CreateVariantDto> Variants { get; set; }
        //public CreateProductVariantPriceDto CreateProductVariantPriceDto { get; set; }
        public IEnumerable<CreateCompositeProductDto> CompositeProducts { get; set; }
    }

    public class UpdateProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string SKU { get; set; }
        public Guid ProductTypeId { get; set; }
        public IEnumerable<CreateVariantDto> Variants { get; set; }
        public IEnumerable<CreateCompositeProductDto> CompositeProducts { get; set; }
    }

    public class VariantDto: BaseEntityDto
    {
        //public Guid VariantId { get; set; }
        public string Attribute { get; set; }
        public string Value { get; set; }
        public decimal Price { get; set; }
        public string SKU { get; set; }
        public Guid ProductId { get; set; }
        public ProductDto Product { get; set; }
    }

    public class DisplayVariantDto: BaseEntityDto
    {
        //public Guid VariantId { get; set; }
        public string Attribute { get; set; }
        public string Value { get; set; }
        public decimal Price { get; set; }
        public string SKU { get; set; }
        public Guid ProductId { get; set; }
    }

    public class CreateVariantDto
    {
        //public Guid VariantId { get; set; }
        public string Attribute { get; set; }
        public string Value { get; set; }
        public decimal Price { get; set; }
        public string SKU { get; set; }
        public Guid ProductId { get; set; }
    }

    public class ProductVariantPriceDto: BaseEntityDto
    {
        public decimal Price { get; set; }
        public string SKU { get; set; }
        public Guid ProductId { get; set; }
        public ProductDto Product { get; set; }
    }

    public class CreateProductVariantPriceDto : BaseEntityDto
    {
        public decimal Price { get; set; }
        public string SKU { get; set; }
        public Guid ProductId { get; set; }
    }

    public class CompositeProductDto: BaseEntityDto
    {
        public virtual ProductDto Product { get; set; }
        public Guid AccompanyingProductId { get; set; }
        public ProductDto AccompanyingProduct { get; set; }
        public bool IsExtraCompulsory { get; set; }
        public bool IsExtraCosted { get; set; }
        public string SKU { get; set; }
    }

    public class DisplayCompositeProductDto: BaseEntityDto
    {
        public Guid AccompanyingProductId { get; set; }
        public bool IsExtraCompulsory { get; set; }
        public bool IsExtraCosted { get; set; }
        public string SKU { get; set; }
    }

    public class CreateCompositeProductDto
    {
        public Guid AccompanyingProductId { get; set; }
        public bool IsExtraCompulsory { get; set; }
        public bool IsExtraCosted { get; set; }
        public string SKU { get; set; }
    }
}
