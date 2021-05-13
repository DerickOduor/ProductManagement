using MediatR;
using ProductManagement.Commands.Handlers;
using System;
using System.Collections.Generic;
using System.Text;
using TransferObjects;

namespace ProductManagement.Commands
{
    public class ProductTypeCommand:IRequest<CommandResponse>
    {
        public CreateProductTypeDto ProductTypeDto { get; set; }
    }
    public class UpdateProductTypeCommand:IRequest<CommandResponse>
    {
        public UpdateProductTypeDto ProductTypeDto { get; set; }
    }
    public class DeleteProductTypeCommand:IRequest<CommandResponse>
    {
        public DeleteProductTypeDto ProductTypeDto { get; set; }
    }
}
