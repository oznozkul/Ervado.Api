using Ervado.Application.Common.Models;
using MediatR;

namespace Ervado.Application.Features.Auth.Commands.Register;

public record RegisterCommand : IRequest<Response<RegisterResponse>>
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string ConfirmPassword { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
}