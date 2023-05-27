using MediatR;
using Microsoft.AspNetCore.Mvc;
using CatalogService.Application.Entities;
using CatalogService.BLL.Categories.Commands;
using CatalogService.BLL.Categories.Queries;
using CatalogService.BLL.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CatalogService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IEnumerable<Category>> GetCategories([FromQuery] int? count)
        {
            if (count.HasValue)
                return await _mediator.Send(new GetCategoriesWithCountQuery(count.Value));

            return await _mediator.Send(new GetCategoriesQuery());
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<Category> GetCategory(int id)
        {
            return await _mediator.Send(new GetCategoryByIdQuery(id));
        }

        [HttpPost]
        public async Task<ActionResult> Create(Category category)
        {
            try
            {
                await _mediator.Send(new InsertCategoryCommand(category));
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _mediator.Send(new DeleteCategoryCommand(id));
                return Ok();
            }
            catch(NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> Update(Category category)
        {
            try
            {
                await _mediator.Send(new UpdateCategoryCommand(category));
                return Ok();
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
