using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TransferObjects;

namespace ProductManagement.Queries.QueryHandlers.Product
{
    public class ProductQuery:IRequest<IEnumerable<DisplayProductDto>>
    {
    }
    public class ProductQueryId:IRequest<DisplayProductDto>
    {
        public Guid Id { get; set; }
    }
    public class ProductQueryFilter:IRequest<IEnumerable<DisplayProductDto>>
    {
        public string Key { get; set; }
    }
}
