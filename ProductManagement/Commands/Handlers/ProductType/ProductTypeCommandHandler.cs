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
using static Repository.MyRepo;

namespace ProductManagement.Commands.Handlers
{
    public class ProductTypeCommandHandler : IRequestHandler<ProductTypeCommand, CommandResponse>
    {
        public readonly IRepository<ProductType> repository;
        private readonly IMapper mapper;
        public ProductTypeCommandHandler(IRepository<ProductType> _repository, IMapper _mapper)
        {
            repository = _repository;
            mapper = _mapper;
        }
        public async Task<CommandResponse> Handle(ProductTypeCommand request, CancellationToken cancellationToken)
        {
            var ProductTypes = repository.GetAll();
            List<ProductTypeDto> ProductTypesDTO = (List<ProductTypeDto>)mapper.Map<IEnumerable<ProductTypeDto>>(repository.GetAll());
            if (ProductTypesDTO.Where(p => p.Name.Trim().ToUpper().Equals(request.ProductTypeDto.Name.Trim().ToUpper())).SingleOrDefault() != null)
            {
                return new CommandResponse
                {
                    Status = false,
                    Code = 101,
                    Description = request.ProductTypeDto.Name+" Exists"
                };
            }

           var productTypeDto= mapper.Map<ProductTypeDto>(repository.Insert(new ProductType { Name=request.ProductTypeDto.Name.Trim()}));

            CommandResponse response = new CommandResponse {
                Status = productTypeDto != null ? true : false,
                Code=productTypeDto!=null?000:100,
                Description= productTypeDto != null ? "Success":"Failed"
            };

            return response;
        }
    }
}
