using Ervado.Application.Common.Models;
using Ervado.Application.Common.Services;
using Ervado.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Ervado.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Response<RegisterResponse>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtService _jwtService;

    public RegisterCommandHandler(UserManager<ApplicationUser> userManager, IJwtService jwtService)
    {
        _userManager = userManager;
        _jwtService = jwtService;
    }

    public async Task<Response<RegisterResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            return Response<RegisterResponse>.Failure("User with this email already exists.");
        }

        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return Response<RegisterResponse>.Failure(result.Errors.Select(e => e.Description).ToList());
        }

        var token = _jwtService.GenerateToken(user.Id, user.Email, new List<string>());

        return Response<RegisterResponse>.Success(new RegisterResponse
        {
            Token = token,
            UserId = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName
        });
    }
} 