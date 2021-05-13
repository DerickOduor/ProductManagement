using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TransferObjects;

namespace ProductManagement.Queries
{
    public class ProductTypeQuery:IRequest<IEnumerable<ProductTypeDto>>
    {

    }

    public class SingleProductTypeQuery : IRequest<ProductTypeDto>
    {
        public Guid Id { get; set; }
    }
}
