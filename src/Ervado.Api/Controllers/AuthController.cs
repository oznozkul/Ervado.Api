using Ervado.Application.Common.Models;
using Ervado.Application.Features.Auth.Commands.Login;
using Ervado.Application.Features.Auth.Commands.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ervado.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<ActionResult<Response<LoginResponse>>> Login([FromBody] LoginCommand command)
    {
        var response = await _mediator.Send(command);
        
        if (!response.Succeeded)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpPost("register")]
    public async Task<ActionResult<Response<RegisterResponse>>> Register([FromBody] RegisterCommand command)
    {
        var response = await _mediator.Send(command);
        
        if (!response.Succeeded)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }
} 