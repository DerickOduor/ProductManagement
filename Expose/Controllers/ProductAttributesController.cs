using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Queries.QueryHandlers.Variant;
using TransferObjects;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Expose.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductAttributesController : ControllerBase
    {
        private readonly IMediator mediator;
        public ProductAttributesController(IMediator _mediator)
        {
            mediator = _mediator;
        }
        // GET: api/<ProductAttributesController>
        [HttpGet("/productAttributes/{productid}")]
        public IEnumerable<DisplayVariantDto> GetProductAttr(Guid productid)
        {
            return mediator.Send(new VariantQuery { ProductId = productid }).Result;
        }

        // GET api/<ProductAttributesController>/5
        [HttpGet("{id}")]
        public DisplayVariantDto Get(Guid id)
        {
            return mediator.Send(new VariantQueryId { Id = id }).Result;
        }

        // POST api/<ProductAttributesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ProductAttributesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ProductAttributesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
