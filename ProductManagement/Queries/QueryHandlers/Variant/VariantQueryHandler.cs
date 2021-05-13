using AutoMapper;
using Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TransferObjects;

namespace ProductManagement.Queries.QueryHandlers.Variant
{
    public class VariantQueryHandler : IRequestHandler<VariantQuery, IEnumerable<DisplayVariantDto>>
    {
        ApplicationDatabaseContext db;
        private readonly IMapper mapper;
        public VariantQueryHandler(ApplicationDatabaseContext _db, IMapper _mapper)
        {
            db = _db;
            mapper = _mapper;
        }
        public async Task<IEnumerable<DisplayVariantDto>> Handle(VariantQuery request, CancellationToken cancellationToken)
        {
            var ProductsDTO = (IEnumerable<DisplayVariantDto>)mapper.Map<IEnumerable<DisplayVariantDto>>(db.Variants.Where(v => v.ProductId == request.ProductId).ToList());

            return ProductsDTO;
        }
    }
    public class VariantQueryIdHandler : IRequestHandler<VariantQueryId, DisplayVariantDto>
    {
        ApplicationDatabaseContext db;
        private readonly IMapper mapper;
        public VariantQueryIdHandler(ApplicationDatabaseContext _db, IMapper _mapper)
        {
            db = _db;
            mapper = _mapper;
        }
        public async Task<DisplayVariantDto> Handle(VariantQueryId request, CancellationToken cancellationToken)
        {
            var ProductsDTO = (DisplayVariantDto)mapper.Map<DisplayVariantDto>(db.Variants.Where(v => v.ProductId == request.Id).SingleOrDefault());

            return ProductsDTO;
        }
    }
}
