using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Switchly.WebApi.Auth;
using Switchly.WebApi.Context;
using Switchly.WebApi.Models.Common;
using Switchly.WebApi.Services.Helpers;

namespace Switchly.WebApi.Features.Users.Login;

public sealed record LoginUserCommand(string Email, string Password)
    : IRequest<Response<UserLoginDto>>;


public sealed record UserLoginDto
{
    public Guid UserId { get; init; }
    public string Email { get; set; }
    public ICollection<OrganizationRoleDto> Organizations { get; init; } = new List<OrganizationRoleDto>();
    public string Token { get; init; } = string.Empty;
}


public class RegisterHandlerCommandValidator : AbstractValidator<LoginUserCommand>
{
    public RegisterHandlerCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
    }
}

public class LoginUserHandler(SwitchlyDbContext context, IJwtTokenGenerator jwtTokenGenerator):IRequestHandler<LoginUserCommand, Response<UserLoginDto>>
{
    public async Task<Response<UserLoginDto>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await context
            .Users
            .FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);
        
        if (user is null)
            return Response<UserLoginDto>.Fail("Email ya da şifre hatalı.");

        var verificationResult = HashPasswordService.Verify(user, request.Password);
        if (verificationResult == PasswordVerificationResult.Failed)
            return Response<UserLoginDto>.Fail("Email ya da şifre hatalı.");

        if (verificationResult == PasswordVerificationResult.SuccessRehashNeeded)
        {
            user.PasswordHash = HashPasswordService.Hash(user, request.Password);
            await context.SaveChangesAsync(cancellationToken);
        }

        var organizations = await context.OrganizationMembers
            .Where(x => x.UserId == user.Id)
            .Select(x=>new OrganizationRoleDto(x.OrganizationId, x.Role.ToString()))
            .ToListAsync(cancellationToken: cancellationToken);

        var token = jwtTokenGenerator.GenerateToken(user.Id, organizations);
        return Response<UserLoginDto>.Ok(new UserLoginDto{
            UserId = user.Id,
            Email = user.Email,
            Organizations = organizations,
            Token = token,
        });
    }
    
    
}
