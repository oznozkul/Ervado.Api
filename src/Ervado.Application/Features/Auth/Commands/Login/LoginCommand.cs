using Ervado.Application.Common.Models;
using MediatR;

namespace Ervado.Application.Features.Auth.Commands.Login;

public record LoginCommand : IRequest<Response<LoginResponse>>
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}