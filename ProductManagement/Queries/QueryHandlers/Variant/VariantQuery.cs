using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TransferObjects;

namespace ProductManagement.Queries.QueryHandlers.Variant
{
    public class VariantQuery:IRequest<IEnumerable<DisplayVariantDto>>
    {
        public Guid ProductId { get; set; }
    }
    public class VariantQueryId:IRequest<DisplayVariantDto>
    {
        public Guid Id { get; set; }
    }
}
