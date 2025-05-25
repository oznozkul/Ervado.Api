using Ervado.Application.Common.Models;
using Ervado.Application.Features.StockMovements.Commands.Create;
using Ervado.Application.Features.StockMovements.Commands.Delete;
using Ervado.Application.Features.StockMovements.Queries.GetStockMovement;
using Ervado.Application.Features.StockMovements.Queries.GetStockMovementById;
using Ervado.Application.Features.StockMovements.Queries.GetStockMovements;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ervado.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockMovementsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StockMovementsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<Response<List<StockMovementListDto>>>> GetStockMovements([FromQuery] GetStockMovementsQuery query)
        {
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Response<StockMovementDto>>> GetStockMovementById(int id)
        {
            var response = await _mediator.Send(new GetStockMovementByIdQuery { Id = id });
            
            if (!response.Succeeded)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet("inventory/{inventoryId}")]
        public async Task<ActionResult<Response<List<StockMovementListDto>>>> GetStockMovementsByInventory(int inventoryId)
        {
            var query = new GetStockMovementsQuery { InventoryId = inventoryId };
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<Response<CreateStockMovementResponse>>> CreateStockMovement([FromBody] CreateStockMovementCommand command)
        {
            var response = await _mediator.Send(command);
            
            if (!response.Succeeded)
            {
                return BadRequest(response);
            }

            return CreatedAtAction(nameof(GetStockMovementById), new { id = response.Data.Id }, response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Response>> DeleteStockMovement(int id)
        {
            var response = await _mediator.Send(new DeleteStockMovementCommand { Id = id });
            
            if (!response.Succeeded)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
} 