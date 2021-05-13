using AutoMapper;
using Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using static Repository.MyRepo;

namespace ProductManagement.Commands.Handlers
{
    public class DeleteProductTypeCommandHandler : IRequestHandler<DeleteProductTypeCommand, CommandResponse>
    {
        public readonly IRepository<ProductType> repository;
        private readonly IMapper mapper;
        public DeleteProductTypeCommandHandler(IRepository<ProductType> _repository, IMapper _mapper)
        {
            repository = _repository;
            mapper = _mapper;
        }
        public async Task<CommandResponse> Handle(DeleteProductTypeCommand request, CancellationToken cancellationToken)
        {
            //var ProductType = mapper.Map<ProductType>(request.ProductTypeDto);

            var Product_Type = repository.GetById(request.ProductTypeDto.Id);
            if (Product_Type == null)
            {
                return new CommandResponse
                {
                    Status = false,
                    Code = 100,
                    Description = "Does not exist"
                };
            }

            var ProductResponse = repository.Delete(request.ProductTypeDto.Id);

            if (ProductResponse == false)
            {
                return new CommandResponse
                {
                    Status = false,
                    Code = 100,
                    Description = "Failed"
                };
            }

            CommandResponse response = new CommandResponse
            {
                Status = ProductResponse ? true : false,
                Code = ProductResponse ? 000 : 100,
                Description = ProductResponse ? "Success" : "Failed"
            };

            return response;
        }
    }
}
