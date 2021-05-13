using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TransferObjects;

namespace ProductManagement.Commands.Handlers.Products
{
    public class ProductCommand:IRequest<CommandResponse>
    {
        public CreateProductDto ProductDto { get; set; }
    }
    public class UpdateProductCommand:IRequest<CommandResponse>
    {
        public UpdateProductDto ProductDto { get; set; }
    }
    public class DeleteProductCommand:IRequest<CommandResponse>
    {
        public Guid Id { get; set; }
    }
}
