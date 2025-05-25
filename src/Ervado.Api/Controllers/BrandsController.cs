using Ervado.Application.Common.Models;
using Ervado.Application.Features.Brands.Commands.Create;
using Ervado.Application.Features.Brands.Commands.Delete;
using Ervado.Application.Features.Brands.Commands.Update;
using Ervado.Application.Features.Brands.Queries.GetBrandById;
using Ervado.Application.Features.Brands.Queries.GetBrands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ervado.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BrandsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BrandsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<Response<List<BrandListDto>>>> GetBrands([FromQuery] GetBrandsQuery query)
        {
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Response<BrandDto>>> GetBrandById(int id)
        {
            var response = await _mediator.Send(new GetBrandByIdQuery { Id = id });
            
            if (!response.Succeeded)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<Response<CreateBrandResponse>>> CreateBrand([FromBody] CreateBrandCommand command)
        {
            var response = await _mediator.Send(command);
            
            if (!response.Succeeded)
            {
                return BadRequest(response);
            }

            return CreatedAtAction(nameof(GetBrandById), new { id = response.Data.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Response>> UpdateBrand(int id, [FromBody] UpdateBrandCommand command)
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
        public async Task<ActionResult<Response>> DeleteBrand(int id)
        {
            var response = await _mediator.Send(new DeleteBrandCommand { Id = id });
            
            if (!response.Succeeded)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
} 