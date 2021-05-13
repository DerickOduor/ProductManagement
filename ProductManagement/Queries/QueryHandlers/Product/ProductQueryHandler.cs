using AutoMapper;
using Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TransferObjects;

namespace ProductManagement.Queries.QueryHandlers.Product
{
    public class ProductQueryHandler : IRequestHandler<ProductQuery, IEnumerable<DisplayProductDto>>
    {
        ApplicationDatabaseContext db;
        private readonly IMapper mapper;
        public ProductQueryHandler(ApplicationDatabaseContext _db, IMapper _mapper)
        {
            db = _db;
            mapper = _mapper;
        }
        public async Task<IEnumerable<DisplayProductDto>> Handle(ProductQuery request, CancellationToken cancellationToken)
        {
            var ProductsDTO = (from P in db.Products.ToList()
                               join V in db.Variants on P.Id equals V.ProductId into V1
                               join C in db.CompositeProducts on P.Id equals C.ProductId into C1
                               select new DisplayProductDto
                               {
                                   Name = P.Name,
                                   Price = (P.Price),
                                   SKU = P.SKU,
                                   Id = P.Id,
                                   ProductType = (DisplayProductTypeDto)mapper.Map<DisplayProductTypeDto>(db.ProductTypes.Where(o => o.Id == P.Id).FirstOrDefault()),
                                   Variants = (IEnumerable<DisplayVariantDto>)mapper.Map<IEnumerable<DisplayVariantDto>>(db.Variants.Where(r => r.ProductId == P.Id).ToList()),
                                   CompositeProducts = (IEnumerable<DisplayCompositeProductDto>)mapper.
                                   Map<IEnumerable<DisplayCompositeProductDto>>(db.CompositeProducts.Where(r => r.ProductId == P.Id).ToList())
                               });

            return ProductsDTO;
        }
    }
    public class ProductQueryIdHandler : IRequestHandler<ProductQueryId, DisplayProductDto>
    {
        ApplicationDatabaseContext db;
        private readonly IMapper mapper;
        public ProductQueryIdHandler(ApplicationDatabaseContext _db, IMapper _mapper)
        {
            db = _db;
            mapper = _mapper;
        }
        public async Task<DisplayProductDto> Handle(ProductQueryId request, CancellationToken cancellationToken)
        {
            var ProductsDTO = (from P in db.Products.Where(p => p.Id == request.Id).ToList()
                               join V in db.Variants on P.Id equals V.ProductId into V1
                               join C in db.CompositeProducts on P.Id equals C.ProductId into C1
                               select new DisplayProductDto
                               {
                                   Name = P.Name,
                                   Price = P.Price,
                                   SKU = P.SKU,
                                   Id = P.Id,
                                   ProductType = (DisplayProductTypeDto)mapper.Map<DisplayProductTypeDto>(db.ProductTypes.Where(o => o.Id == P.Id).FirstOrDefault()),
                                   Variants = (IEnumerable<DisplayVariantDto>)mapper.Map<IEnumerable<DisplayVariantDto>>(db.Variants.Where(r => r.ProductId == P.Id).ToList()),
                                   CompositeProducts = (IEnumerable<DisplayCompositeProductDto>)mapper.
                                   Map<IEnumerable<DisplayCompositeProductDto>>(db.CompositeProducts.Where(r => r.ProductId == P.Id).ToList())
                               });

            return ProductsDTO.First();
        }
    }
    public class ProductQueryFilterHandler : IRequestHandler<ProductQueryFilter,IEnumerable<DisplayProductDto>>
    {
        ApplicationDatabaseContext db;
        private readonly IMapper mapper;
        public ProductQueryFilterHandler(ApplicationDatabaseContext _db, IMapper _mapper)
        {
            db = _db;
            mapper = _mapper;
        }
        public async Task<IEnumerable<DisplayProductDto>> Handle(ProductQueryFilter request, CancellationToken cancellationToken)
        {
            var ProductsDTO = (from P in db.Products.Where(p => p.Name.ToUpper().Contains(request.Key.ToUpper())).ToList()
                               join V in db.Variants on P.Id equals V.ProductId into V1
                                                    join C in db.CompositeProducts on P.Id equals C.ProductId into C1
                                                    select new DisplayProductDto
                                                    { 
                                                        Name=P.Name,
                                                        Price=P.Price,
                                                        SKU=P.SKU,
                                                        Id=P.Id,
                                                        ProductType= (DisplayProductTypeDto)mapper.Map<DisplayProductTypeDto>(db.ProductTypes.Where(o=>o.Id==P.Id).FirstOrDefault()),
                                                        Variants=(IEnumerable<DisplayVariantDto>)mapper.Map<IEnumerable<DisplayVariantDto>>(db.Variants.Where(r=>r.ProductId==P.Id).ToList()),
                                                        CompositeProducts = (IEnumerable<DisplayCompositeProductDto>)mapper.
                                   Map<IEnumerable<DisplayCompositeProductDto>>(db.CompositeProducts.Where(r => r.ProductId == P.Id).ToList())
                                                    });

            return ProductsDTO;
        }
    }
}
