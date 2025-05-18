using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ervado.Application.Features.Auth.Commands.Login
{
    public record LoginResponse
    {
        public string Token { get; init; } = string.Empty;
        public string UserId { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public List<string> Roles { get; init; } = new();
    }
}
