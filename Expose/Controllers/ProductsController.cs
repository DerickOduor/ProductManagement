using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Commands.Handlers;
using ProductManagement.Commands.Handlers.Products;
using ProductManagement.Queries.QueryHandlers.Product;
using TransferObjects;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Expose.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator mediator;
        public ProductsController(IMediator _mediator)
        {
            mediator = _mediator;
        }
        // GET: api/<Products>
        [HttpGet]
        public IEnumerable<DisplayProductDto> Get()
        {
            return mediator.Send(new ProductQuery { }).Result;
            //return new string[] { "value1", "value2" };
        }

        // GET api/<Products>/5
        [HttpGet("{id}")]
        public DisplayProductDto Get(Guid id)
        {
            return mediator.Send(new ProductQueryId { Id = id }).Result;
        }

        // GET api/<Products>/5
        [HttpGet("/filter/{key}")]
        public IEnumerable<DisplayProductDto> Get(string key)
        {
            return mediator.Send(new ProductQueryFilter { Key = key }).Result;
        }

        // POST api/<Products>
        [HttpPost]
        public CommandResponse Post([FromBody] ProductCommand value)
        {
            try
            {
                return mediator.Send(value).Result;
            }
            catch (Exception ex)
            {
                return new CommandResponse
                {
                    Description = ex.Message,
                    Code = 100,
                    Status = false
                };
            }
        }

        // PUT api/<Products>/5
        [HttpPut("{id}")]
        public CommandResponse Put(Guid id, [FromBody] UpdateProductCommand value)
        {
            if(id!=value.ProductDto.Id)
                return new CommandResponse
                {
                    Description = "Invalid Id provided",
                    Code = 100,
                    Status = false
                };
            try
            {
                return mediator.Send(value).Result;
            }
            catch(Exception ex)
            {
                return new CommandResponse
                {
                    Description = ex.Message,
                    Code = 100,
                    Status = false
                };
            }
        }

        // DELETE api/<Products>/5
        [HttpDelete("{id}")]
        public CommandResponse Delete(Guid id)
        {
            try
            {
                return mediator.Send(new DeleteProductCommand { Id=id}).Result;
            }
            catch (Exception ex)
            {
                return new CommandResponse
                {
                    Description = ex.Message,
                    Code = 100,
                    Status = false
                };
            }
        }
    }
}
