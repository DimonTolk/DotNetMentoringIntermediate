using MediatR;
using Microsoft.AspNetCore.Mvc;
using CatalogService.Application.Entities;
using CatalogService.BLL.Common.Exceptions;
using CatalogService.BLL.Products.Commands;
using CatalogService.BLL.Products.Queries;
using CatalogService.BLL.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CatalogService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ItemsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("products")]
        public async Task<IEnumerable<Item>> GetProducts([FromQuery] int? count)
        {
            if (count.HasValue)
                return await _mediator.Send(new GetItemsWithCountQuery(count.Value));

            return await _mediator.Send(new GetItemsQuery());
        }

        [HttpGet]
        [Route("products/pagination")]
        public async Task<ItemPaginationVm> GetProducts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] int? categoryId = null)
        {
            return await _mediator.Send(new GetItemsWithPaginationQuery(pageNumber, pageSize, categoryId));
        }

        [HttpGet]
        [Route("product")]
        public async Task<Item> GetProduct(int id)
        {
            return await _mediator.Send(new GetItemByIdQuery(id));
        }

        [HttpPost("product")]
        public async Task<ActionResult> Create(Item product)
        {
            try
            {
                await _mediator.Send(new InsertItemCommand(product));
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("product")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _mediator.Send(new DeleteItemCommand(id));
                return Ok();
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("product")]
        public async Task<ActionResult> Update(Item product)
        {
            try
            {
                await _mediator.Send(new UpdateItemCommand(product));
                return Ok();
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
