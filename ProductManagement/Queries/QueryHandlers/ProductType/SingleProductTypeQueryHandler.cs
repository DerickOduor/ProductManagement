using AutoMapper;
using Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TransferObjects;
using static Repository.MyRepo;

namespace ProductManagement.Queries.QueryHandlers
{
    public class SingleProductTypeQueryHandler : IRequestHandler<SingleProductTypeQuery, ProductTypeDto>
    {
        public readonly IRepository<ProductType> repository;
        private readonly IMapper mapper;
        public SingleProductTypeQueryHandler(IRepository<ProductType> _repository, IMapper _mapper)
        {
            repository = _repository;
            mapper = _mapper;
        }
        public async Task<ProductTypeDto> Handle(SingleProductTypeQuery request, CancellationToken cancellationToken)
        {
            var ProductTypeDTO = mapper.Map<ProductTypeDto>(repository.GetById(request.Id));

            return ProductTypeDTO != null ? ProductTypeDTO : null;
        }
    }
}
