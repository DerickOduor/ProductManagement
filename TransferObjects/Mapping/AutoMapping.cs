using AutoMapper;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace TransferObjects.Mapping
{
    public class AutoMapping:Profile
    {
        public AutoMapping()
        {
            CreateMap<ProductTypeDto, ProductType>();
            CreateMap<ProductType, ProductTypeDto>();

            CreateMap<DisplayProductTypeDto, ProductType>();
            CreateMap<ProductType, DisplayProductTypeDto>();

            CreateMap<ProductDto, Product>();
            CreateMap<Product, ProductDto>();

            CreateMap<VariantDto, Variant>();
            CreateMap<Variant, VariantDto>();

            CreateMap<ProductVariantPriceDto, ProductVariantPrice>();
            CreateMap<ProductVariantPrice, ProductVariantPriceDto>();

            CreateMap<CompositeProductDto, CompositeProduct>();
            CreateMap<CompositeProduct, CompositeProductDto>();

            CreateMap<DisplayCompositeProductDto, CompositeProduct>();
            CreateMap<CompositeProduct, DisplayCompositeProductDto>();

            CreateMap<VariantDto, Variant>();
            CreateMap<Variant, VariantDto>();

            CreateMap<DisplayVariantDto, Variant>();
            CreateMap<Variant, DisplayVariantDto>();

            CreateMap<CreateVariantDto, Variant>();
            CreateMap<Variant, CreateVariantDto>();

            CreateMap<CreateCompositeProductDto, CompositeProduct>();
            CreateMap<CompositeProduct, CreateCompositeProductDto>();
        }
    }
}
