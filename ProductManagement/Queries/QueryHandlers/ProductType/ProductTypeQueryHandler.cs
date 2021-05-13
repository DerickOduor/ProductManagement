using AutoMapper;
using Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TransferObjects;
using static Repository.MyRepo;

namespace ProductManagement.Queries.QueryHandlers
{
    public class ProductTypeQueryHandler : IRequestHandler<ProductTypeQuery, IEnumerable<ProductTypeDto>>
    {
        public readonly IRepository<ProductType> repository;
        private readonly IMapper mapper;
        public ProductTypeQueryHandler(IRepository<ProductType> _repository, IMapper _mapper)
        {
            repository = _repository;
            mapper = _mapper;
        }
        public async Task<IEnumerable<ProductTypeDto>> Handle(ProductTypeQuery request, CancellationToken cancellationToken)
        {

            ProductType productType = new ProductType
            {
                Name = "Single"
            };
            //repository.Insert(productType);
            List<ProductTypeDto> ProductTypesDTO_ = (List<ProductTypeDto>)mapper.Map<IEnumerable<ProductTypeDto>>(repository.GetAll());
            var ProductTypesDTO = mapper.Map<IEnumerable<ProductTypeDto>>(repository.GetAll());


            //return (Task<IEnumerable<ProductTypeDto>>)ProductTypesDTO;
            return ProductTypesDTO;
        }
    }
}
