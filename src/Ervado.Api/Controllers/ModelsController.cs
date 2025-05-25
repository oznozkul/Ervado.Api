using Ervado.Application.Common.Models;
using Ervado.Application.Features.Models.Commands.Create;
using Ervado.Application.Features.Models.Commands.Delete;
using Ervado.Application.Features.Models.Commands.Update;
using Ervado.Application.Features.Models.Queries.GetModelById;
using Ervado.Application.Features.Models.Queries.GetModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ervado.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ModelsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ModelsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<Response<List<ModelListDto>>>> GetModels([FromQuery] GetModelsQuery query)
        {
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Response<ModelDto>>> GetModelById(int id)
        {
            var response = await _mediator.Send(new GetModelByIdQuery { Id = id });
            
            if (!response.Succeeded)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet("brand/{brandId}")]
        public async Task<ActionResult<Response<List<ModelListDto>>>> GetModelsByBrand(int brandId)
        {
            var query = new GetModelsQuery { BrandId = brandId };
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<Response<CreateModelResponse>>> CreateModel([FromBody] CreateModelCommand command)
        {
            var response = await _mediator.Send(command);
            
            if (!response.Succeeded)
            {
                return BadRequest(response);
            }

            return CreatedAtAction(nameof(GetModelById), new { id = response.Data.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Response>> UpdateModel(int id, [FromBody] UpdateModelCommand command)
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
        public async Task<ActionResult<Response>> DeleteModel(int id)
        {
            var response = await _mediator.Send(new DeleteModelCommand { Id = id });
            
            if (!response.Succeeded)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
} 