using Ervado.Application.Common.Models;
using Ervado.Application.Features.ProductCategories.Commands.Create;
using Ervado.Application.Features.ProductCategories.Commands.Delete;
using Ervado.Application.Features.ProductCategories.Commands.Update;
using Ervado.Application.Features.ProductCategories.Queries.GetProductCategoryById;
using Ervado.Application.Features.ProductCategories.Queries.GetProductCategories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ervado.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductCategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductCategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<Response<List<ProductCategoryListDto>>>> GetProductCategories([FromQuery] GetProductCategoriesQuery query)
        {
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Response<ProductCategoryDto>>> GetProductCategoryById(int id)
        {
            var response = await _mediator.Send(new GetProductCategoryByIdQuery { Id = id });
            
            if (!response.Succeeded)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<Response<CreateProductCategoryResponse>>> CreateProductCategory([FromBody] CreateProductCategoryCommand command)
        {
            var response = await _mediator.Send(command);
            
            if (!response.Succeeded)
            {
                return BadRequest(response);
            }

            return CreatedAtAction(nameof(GetProductCategoryById), new { id = response.Data.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Response>> UpdateProductCategory(int id, [FromBody] UpdateProductCategoryCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("The ID in the URL does not match the ID in the request body.");
            }
            
            var response = await _mediator.Send(command);
            
            if (!response.Succeeded)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Response>> DeleteProductCategory(int id)
        {
            var response = await _mediator.Send(new DeleteProductCategoryCommand { Id = id });
            
            if (!response.Succeeded)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
} 