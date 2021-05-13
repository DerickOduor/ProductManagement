using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Commands;
using ProductManagement.Commands.Handlers;
using ProductManagement.Queries;
using TransferObjects;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Expose.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoryController : ControllerBase
    {
        private readonly IMediator mediator;
        public ProductCategoryController(IMediator _mediator)
        {
            mediator = _mediator;
        }
        // GET: api/<ProductCategoryController>
        [HttpGet]
        public IEnumerable<ProductTypeDto> Get()
        {
            try
            {
                ProductTypeQuery productTypeQuery = new ProductTypeQuery();

                return mediator.Send(productTypeQuery).Result;

            }catch(Exception ex) { }

            return null;
        }

        // GET api/<ProductCategoryController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ProductCategoryController>
        [HttpPost]
        public CommandResponse Post([FromBody] ProductTypeCommand value)
        {
            try
            {
                return mediator.Send(value).Result;
            }catch(Exception ex)
            {
                return new CommandResponse
                {
                    Description = ex.Message,
                    Code = 100,
                    Status = false
                };
            }
        }

        // PUT api/<ProductCategoryController>/5
        [HttpPut("{id}")]
        public CommandResponse Put(Guid id, [FromBody] UpdateProductTypeCommand value)
        {
            if (id != value.ProductTypeDto.Id)
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

        // DELETE api/<ProductCategoryController>/5
        [HttpDelete("{id}")]
        public CommandResponse Delete(Guid id)
        {
            try
            {
                return mediator.Send(new DeleteProductTypeCommand {ProductTypeDto=new DeleteProductTypeDto { Id=id} }).Result;
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
