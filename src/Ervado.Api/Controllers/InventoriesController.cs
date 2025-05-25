using Ervado.Application.Common.Models;
using Ervado.Application.Features.Inventories.Commands.Create;
using Ervado.Application.Features.Inventories.Commands.Delete;
using Ervado.Application.Features.Inventories.Commands.Update;
using Ervado.Application.Features.Inventories.Queries.GetInventoryById;
using Ervado.Application.Features.Inventories.Queries.GetInventories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ervado.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InventoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<Response<List<InventoryListDto>>>> GetInventories([FromQuery] GetInventoriesQuery query)
        {
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Response<InventoryDto>>> GetInventoryById(int id)
        {
            var response = await _mediator.Send(new GetInventoryByIdQuery { Id = id });
            
            if (!response.Succeeded)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet("product/{productId}")]
        public async Task<ActionResult<Response<List<InventoryListDto>>>> GetInventoriesByProduct(int productId)
        {
            var query = new GetInventoriesQuery { ProductId = productId };
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<Response<CreateInventoryResponse>>> CreateInventory([FromBody] CreateInventoryCommand command)
        {
            var response = await _mediator.Send(command);
            
            if (!response.Succeeded)
            {
                return BadRequest(response);
            }

            return CreatedAtAction(nameof(GetInventoryById), new { id = response.Data.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Response>> UpdateInventory(int id, [FromBody] UpdateInventoryCommand command)
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
        public async Task<ActionResult<Response>> DeleteInventory(int id)
        {
            var response = await _mediator.Send(new DeleteInventoryCommand { Id = id });
            
            if (!response.Succeeded)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
} 