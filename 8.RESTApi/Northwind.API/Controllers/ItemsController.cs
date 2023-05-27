using MediatR;
using Microsoft.AspNetCore.Mvc;
using CatalogService.Application.Entities;
using CatalogService.BLL.Common.Exceptions;
using CatalogService.BLL.Items.Commands;
using CatalogService.BLL.Items.Queries;
using CatalogService.BLL.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CatalogService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ItemsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{count}")]
        public async Task<IEnumerable<Item>> GetItems([FromQuery] int? count)
        {
            if (count.HasValue)
                return await _mediator.Send(new GetItemsWithCountQuery(count.Value));

            return await _mediator.Send(new GetItemsQuery());
        }

        [HttpGet]
        public async Task<ItemPaginationVm> GetItems([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] int? categoryId = null)
        {
            return await _mediator.Send(new GetItemsWithPaginationQuery(pageNumber, pageSize, categoryId));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<Item> GetItem(int id)
        {
            return await _mediator.Send(new GetItemByIdQuery(id));
        }

        [HttpPost]
        public async Task<ActionResult> Create(Item item)
        {
            try
            {
                await _mediator.Send(new InsertItemCommand(item));
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
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

        [HttpPut]
        public async Task<ActionResult> Update(Item item)
        {
            try
            {
                await _mediator.Send(new UpdateItemCommand(item));
                return Ok();
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
