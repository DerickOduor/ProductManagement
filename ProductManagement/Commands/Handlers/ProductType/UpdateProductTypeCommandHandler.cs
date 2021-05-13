using AutoMapper;
using Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using static Repository.MyRepo;

namespace ProductManagement.Commands.Handlers
{
    public class UpdateProductTypeCommandHandler : IRequestHandler<UpdateProductTypeCommand, CommandResponse>
    {
        public readonly IRepository<ProductType> repository;
        private readonly IMapper mapper;
        public UpdateProductTypeCommandHandler(IRepository<ProductType> _repository, IMapper _mapper)
        {
            repository = _repository;
            mapper = _mapper;
        }
        public async Task<CommandResponse> Handle(UpdateProductTypeCommand request, CancellationToken cancellationToken)
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

            Product_Type.Name = request.ProductTypeDto.Name;
            var ProductResponse=repository.Update(Product_Type);

            if (ProductResponse == null)
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
                Status = ProductResponse != null ? true : false,
                Code = ProductResponse != null ? 000 : 100,
                Description = ProductResponse != null ? "Success" : "Failed"
            };

            return response;
        }
    }
}
