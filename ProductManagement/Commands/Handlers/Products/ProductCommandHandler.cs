using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Entities;
using System.Threading;
using System.Threading.Tasks;
using static Repository.MyRepo;
using TransferObjects;
using System.Linq;

namespace ProductManagement.Commands.Handlers.Products
{
    public class ProductCommandHandler : IRequestHandler<ProductCommand, CommandResponse>
    {
        public readonly IRepository<Product> repository;
        public readonly IRepository<ProductType> productTypeRepository;
        public readonly IRepository<Variant> variantTypeRepository;
        public readonly IRepository<ProductVariantPrice> productVariantPriceRepository;
        public readonly IRepository<CompositeProduct> compositeProductRepository;
        private readonly IMapper mapper;
        public ProductCommandHandler(IRepository<Product> _repository, IRepository<ProductType> _productTypeRepository,
            IRepository<Variant> _variantTypeRepository, IRepository<ProductVariantPrice> _productVariantPriceRepository,
            IRepository<CompositeProduct> _compositeProductRepository,IMapper _mapper)
        {
            repository = _repository;
            mapper = _mapper;
            productTypeRepository = _productTypeRepository;
            variantTypeRepository = _variantTypeRepository;
            productVariantPriceRepository = _productVariantPriceRepository;
            compositeProductRepository = _compositeProductRepository;
        }
        public async Task<CommandResponse> Handle(ProductCommand request, CancellationToken cancellationToken)
        {
            List<ProductDto> ProductsDTO = (List<ProductDto>)mapper.Map<IEnumerable<ProductDto>>(repository.GetAll());
            if (ProductsDTO.Where(p => p.Name.Equals(request.ProductDto.Name)).SingleOrDefault() != null)
                return new CommandResponse
                {
                    Status = false,
                    Code = 101,
                    Description = request.ProductDto.Name + " Exists"
                };
            if (ProductsDTO.Where(p => p.SKU.Equals(request.ProductDto.SKU)).SingleOrDefault() != null)
                return new CommandResponse
                {
                    Status = false,
                    Code = 101,
                    Description ="Product with SKU "+ request.ProductDto.SKU + " Exists"
                };

            ProductType productType = productTypeRepository.GetById(request.ProductDto.ProductTypeId);
            if(productType==null)
                return new CommandResponse
                {
                    Status = false,
                    Code = 101,
                    Description = "Product type with the Id ("+request.ProductDto.ProductTypeId+") does not exist"
                };

            var ProductType = productTypeRepository.GetById(request.ProductDto.ProductTypeId);

            switch (ProductType.Name.Trim().ToUpper())
            {
                case "STANDARD PRODUCT":
                    Product product = new Product
                    {
                        Name=request.ProductDto.Name,
                        Price=request.ProductDto.Price,
                        SKU=request.ProductDto.SKU,
                        ProductTypeId=ProductType.Id
                    };

                    if (request.ProductDto.Price == 0M)
                    {
                        return new CommandResponse
                        {
                            Code = 100,
                            Status = false,
                            Description = "Price cannot be zero(0)"
                        };
                    }

                    var ProductResponse=repository.Insert(product);

                    return new CommandResponse
                    {
                        Code = ProductResponse!=null?000:100,
                        Status = ProductResponse != null ? true:false,
                        Description = ProductResponse != null ? "Success - "+ ProductResponse.Name : "Failed"
                    };

                    break;
                case "PRODUCTS WITH VARIANTS":
                    Product productVariant = new Product 
                    { 
                        Name=request.ProductDto.Name.Trim(),
                        ProductTypeId=request.ProductDto.ProductTypeId,
                        SKU="N/A"
                    };

                    if(request.ProductDto.Variants==null)
                        return new CommandResponse
                        {
                            Code = 100,
                            Status = false,
                            Description = "Variants are compulsory"
                        };

                    var Variants = (List<Variant>)mapper.Map<IEnumerable<Variant>>(request.ProductDto.Variants);
                    var variantsInDB = (List<Variant>)mapper.Map<IEnumerable<Variant>>(variantTypeRepository.GetAll());
                    var DupliVariants= Variants.GroupBy(x => x.SKU).Any(g => g.Count() > 1);
                    if (DupliVariants)
                        return new CommandResponse
                        {
                            Code = 100,
                            Status = false,
                            Description = "Duplicate SKUs detected from the variants' list"
                        };
                    foreach (var variant in Variants)
                    {
                        if (variant.SKU == null)
                            return new CommandResponse
                            {
                                Code = 100,
                                Status = false,
                                Description = "SKU is required"
                            };
                        if (variant.SKU=="")
                            return new CommandResponse
                            {
                                Code = 100,
                                Status = false,
                                Description = "SKU is required"
                            };
                        if (variantsInDB.Where(v=>v.SKU.ToUpper().Trim().Equals(variant.SKU)).SingleOrDefault()!=null)
                            return new CommandResponse
                            {
                                Code = 100,
                                Status = false,
                                Description = "SKU ("+ variant.SKU + ") is already in use"
                            };
                        if (variant.Price==0)
                            return new CommandResponse
                            {
                                Code = 100,
                                Status = false,
                                Description = "Price is required"
                            };
                    }

                    var VariantProductResponse = repository.Insert(productVariant);

                    if (VariantProductResponse == null)
                        return new CommandResponse
                        {
                            Code = 100,
                            Status = false,
                            Description = "Failed"
                        };
                    foreach (var variant in Variants)
                    {
                        variant.ProductId = VariantProductResponse.Id;

                        variantTypeRepository.Insert(variant);
                    }

                    return new CommandResponse
                    {
                        Code = 000,
                        Status = true,
                        Description = "Success - "+VariantProductResponse.Name
                    };

                    break;
                case "COMPOSITE PRODUCTS":
                    Product productComposite = new Product
                    {
                        Name = request.ProductDto.Name.Trim(),
                        ProductTypeId = request.ProductDto.ProductTypeId,
                        Price=request.ProductDto.Price,
                        SKU=request.ProductDto.SKU
                    };

                    if (request.ProductDto.CompositeProducts == null)
                        return new CommandResponse
                        {
                            Code = 100,
                            Status = false,
                            Description = "Accompanying products was not provided"
                        };

                    if (request.ProductDto.CompositeProducts.Count()==0)
                        return new CommandResponse
                        {
                            Code = 100,
                            Status = false,
                            Description = "Accompanying products was not provided"
                        };
                    var CompositeProducts = (List<CompositeProduct>)mapper.Map<IEnumerable<CompositeProduct>>(request.ProductDto.CompositeProducts);
                    var DupliComposites = CompositeProducts.GroupBy(x => x.AccompanyingProductId).Any(g => g.Count() > 1);
                    if (DupliComposites)
                        return new CommandResponse
                        {
                            Code = 100,
                            Status = false,
                            Description = "Duplicate SKUs detected from the variants' list"
                        };


                    var DupliCompositesInDB = (compositeProductRepository.GetAll());

                    var productCompositeResponse = repository.Insert(productComposite);
                    if(productCompositeResponse==null)
                        return new CommandResponse
                        {
                            Code = 100,
                            Status = false,
                            Description = "Failed - " + productCompositeResponse.Name
                        };

                    foreach (var c in CompositeProducts)
                    {
                        c.ProductId = productCompositeResponse.Id;
                        c.SKU = "";

                        compositeProductRepository.Insert(c);
                    }

                    return new CommandResponse
                    {
                        Code = 000,
                        Status = true,
                        Description = "Success - " + productCompositeResponse.Name
                    };

                    break;
                default:
                    return new CommandResponse
                    {
                        Code = 100,
                        Status = false,
                        Description = "Product type not found" 
                    };
                    break;

            }

            return null;
        }
    }
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, CommandResponse>
    {
        public readonly IRepository<Product> repository;
        public readonly IRepository<ProductType> productTypeRepository;
        public readonly IRepository<Variant> variantTypeRepository;
        public readonly IRepository<ProductVariantPrice> productVariantPriceRepository;
        public readonly IRepository<CompositeProduct> compositeProductRepository;
        private readonly IMapper mapper;
        public UpdateProductCommandHandler(IRepository<Product> _repository, IRepository<ProductType> _productTypeRepository,
            IRepository<Variant> _variantTypeRepository, IRepository<ProductVariantPrice> _productVariantPriceRepository,
            IRepository<CompositeProduct> _compositeProductRepository,IMapper _mapper)
        {
            repository = _repository;
            mapper = _mapper;
            productTypeRepository = _productTypeRepository;
            variantTypeRepository = _variantTypeRepository;
            productVariantPriceRepository = _productVariantPriceRepository;
            compositeProductRepository = _compositeProductRepository;
        }
        public async Task<CommandResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            Product productInDB = repository.GetById(request.ProductDto.Id);
            if (productInDB == null)
                return new CommandResponse
                {
                    Status = false,
                    Code = 101,
                    Description = "Product not found"
                };
            List<ProductDto> ProductsDTO = (List<ProductDto>)mapper.Map<IEnumerable<ProductDto>>(repository.GetAll());
            if (ProductsDTO.Where(p => p.Name.Equals(request.ProductDto.Name) & p.Id !=request.ProductDto.Id).SingleOrDefault() != null)
                return new CommandResponse
                {
                    Status = false,
                    Code = 101,
                    Description = request.ProductDto.Name + " Exists"
                };
            if (ProductsDTO.Where(p => p.SKU.Equals(request.ProductDto.SKU) & p.Id != request.ProductDto.Id).SingleOrDefault() != null)
                return new CommandResponse
                {
                    Status = false,
                    Code = 101,
                    Description ="Product with SKU "+ request.ProductDto.SKU + " Exists"
                };

            ProductType productType = productTypeRepository.GetById(request.ProductDto.ProductTypeId);
            if(productType==null)
                return new CommandResponse
                {
                    Status = false,
                    Code = 101,
                    Description = "Product type with the Id ("+request.ProductDto.ProductTypeId+") does not exist"
                };

            var ProductType = productTypeRepository.GetById(request.ProductDto.ProductTypeId);

            switch (ProductType.Name.Trim().ToUpper())
            {
                case "STANDARD PRODUCT":
                    productInDB.Name = request.ProductDto.Name;
                    productInDB.Price = request.ProductDto.Price;
                    productInDB.SKU = request.ProductDto.SKU;
                    productInDB.ProductTypeId = ProductType.Id;

                    if (request.ProductDto.Price == 0M)
                    {
                        return new CommandResponse
                        {
                            Code = 100,
                            Status = false,
                            Description = "Price cannot be zero(0)"
                        };
                    }

                    var ProductResponse=repository.Update(productInDB);

                    return new CommandResponse
                    {
                        Code = ProductResponse!=null?000:100,
                        Status = ProductResponse != null ? true:false,
                        Description = ProductResponse != null ? "Update Success - "+ ProductResponse.Name : " Update Failed"
                    };

                    break;
                case "PRODUCTS WITH VARIANTS":
                    productInDB.Name = request.ProductDto.Name.Trim();
                    productInDB.ProductTypeId = request.ProductDto.ProductTypeId;
                    productInDB.SKU = "N/A";

                    if(request.ProductDto.Variants==null)
                        return new CommandResponse
                        {
                            Code = 100,
                            Status = false,
                            Description = "Variants are compulsory"
                        };

                    var Variants = (List<Variant>)mapper.Map<IEnumerable<Variant>>(request.ProductDto.Variants);
                    var variantsInDB = (List<Variant>)mapper.Map<IEnumerable<Variant>>(variantTypeRepository.GetAll());
                    var DupliVariants= Variants.GroupBy(x => x.SKU).Any(g => g.Count() > 1);
                    if (DupliVariants)
                        return new CommandResponse
                        {
                            Code = 100,
                            Status = false,
                            Description = "Duplicate SKUs detected from the variants' list"
                        };
                    foreach (var variant in Variants)
                    {
                        if (variant.SKU == null)
                            return new CommandResponse
                            {
                                Code = 100,
                                Status = false,
                                Description = "SKU is required"
                            };
                        if (variant.SKU=="")
                            return new CommandResponse
                            {
                                Code = 100,
                                Status = false,
                                Description = "SKU is required"
                            };
                        if (variantsInDB.Where(v=>v.SKU.ToUpper().Trim().Equals(variant.SKU) & v.Id!=variant.Id).SingleOrDefault()!=null)
                            return new CommandResponse
                            {
                                Code = 100,
                                Status = false,
                                Description = "SKU ("+ variant.SKU + ") is already in use"
                            };
                        if (variant.Price==0)
                            return new CommandResponse
                            {
                                Code = 100,
                                Status = false,
                                Description = "Price is required"
                            };
                    }

                    var VariantProductResponse = repository.Update(productInDB);

                    if (VariantProductResponse == null)
                        return new CommandResponse
                        {
                            Code = 100,
                            Status = false,
                            Description = "Update Failed"
                        };
                    foreach (var variant in Variants)
                    {
                        var variantInDb = variantTypeRepository.GetById(variant.Id);
                        variantInDb.ProductId = VariantProductResponse.Id;
                        variantInDb.Price = variant.Price;
                        variantInDb.SKU = variant.SKU;
                        variantInDb.Attribute = variant.Attribute;

                        variantTypeRepository.Update(variantInDb);
                    }

                    return new CommandResponse
                    {
                        Code = 000,
                        Status = true,
                        Description = "Update Success - "+VariantProductResponse.Name
                    };

                    break;
                case "COMPOSITE PRODUCTS":
                    productInDB.Name = request.ProductDto.Name.Trim();
                    productInDB.ProductTypeId = request.ProductDto.ProductTypeId;
                    productInDB.Price = request.ProductDto.Price;
                    productInDB.SKU = request.ProductDto.SKU;

                    if (request.ProductDto.CompositeProducts == null)
                        return new CommandResponse
                        {
                            Code = 100,
                            Status = false,
                            Description = "Accompanying products was not provided"
                        };

                    if (request.ProductDto.CompositeProducts.Count()==0)
                        return new CommandResponse
                        {
                            Code = 100,
                            Status = false,
                            Description = "Accompanying products was not provided"
                        };

                    var CompositeProducts = (List<CompositeProduct>)mapper.Map<IEnumerable<CompositeProduct>>(request.ProductDto.CompositeProducts);
                    var DupliComposites = CompositeProducts.GroupBy(x => x.AccompanyingProductId).Any(g => g.Count() > 1);
                    if (DupliComposites)
                        return new CommandResponse
                        {
                            Code = 100,
                            Status = false,
                            Description = "Duplicate SKUs detected from the variants' list"
                        };


                    var DupliCompositesInDB = (compositeProductRepository.GetAll());

                    var productCompositeResponse = repository.Update(productInDB);
                    if(productCompositeResponse==null)
                        return new CommandResponse
                        {
                            Code = 100,
                            Status = false,
                            Description = "Update Failed - " + productCompositeResponse.Name
                        };

                    foreach (var c in CompositeProducts)
                    {
                        var CompositeProductsInDb = compositeProductRepository.GetById(c.Id);
                        CompositeProductsInDb.ProductId = productCompositeResponse.Id;
                        CompositeProductsInDb.SKU = "";

                        compositeProductRepository.Update(CompositeProductsInDb);
                    }

                    return new CommandResponse
                    {
                        Code = 000,
                        Status = true,
                        Description = "Update Success - " + productCompositeResponse.Name
                    };

                    break;
                default:
                    return new CommandResponse
                    {
                        Code = 100,
                        Status = false,
                        Description = "Product type not found" 
                    };
                    break;

            }

            return null;
        }
    }

    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, CommandResponse>
    {
        public readonly IRepository<Product> repository;
        public readonly IRepository<ProductType> productTypeRepository;
        public readonly IRepository<Variant> variantTypeRepository;
        public readonly IRepository<ProductVariantPrice> productVariantPriceRepository;
        public readonly IRepository<CompositeProduct> compositeProductRepository;
        private readonly IMapper mapper;
        public DeleteProductCommandHandler(IRepository<Product> _repository, IRepository<ProductType> _productTypeRepository,
            IRepository<Variant> _variantTypeRepository, IRepository<ProductVariantPrice> _productVariantPriceRepository,
            IRepository<CompositeProduct> _compositeProductRepository, IMapper _mapper)
        {
            repository = _repository;
            mapper = _mapper;
            productTypeRepository = _productTypeRepository;
            variantTypeRepository = _variantTypeRepository;
            productVariantPriceRepository = _productVariantPriceRepository;
            compositeProductRepository = _compositeProductRepository;
        }
        public async Task<CommandResponse> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            Product productInDB = repository.GetById(request.Id);
            if (productInDB == null)
                return new CommandResponse
                {
                    Status = false,
                    Code = 101,
                    Description = "Product not found"
                };
            bool Res=repository.Delete(productInDB.Id);

            return new CommandResponse
            {
                Status = Res ? true : false,
                Code = Res ? 000 : 100,
                Description = Res ? "Delete success": "Delete failed"
            };

        }
    }
}
