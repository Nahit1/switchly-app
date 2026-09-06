using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Switchly.WebApi.Context;
using Switchly.WebApi.Entities;
using Switchly.WebApi.Models.Common;
using Switchly.WebApi.Services.Helpers;

namespace Switchly.WebApi.Features.Users.Register;

public sealed record RegisterUserCommand(string Email, string Password)
    : IRequest<Response<UserRegisterDto>>;
    
public sealed record UserRegisterDto
{
    public Guid UserId { get; init; }
}
    
public class RegisterHandlerCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterHandlerCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Email is required.");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
    }
}

public class RegisterUserHandler(SwitchlyDbContext context)
    : IRequestHandler<RegisterUserCommand, Response<UserRegisterDto>>
{
    public async Task<Response<UserRegisterDto>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var checkUserExists = await context.Users.FirstOrDefaultAsync(x=>x.Email == request.Email);

        if (checkUserExists is not null)
        {
            return Response<UserRegisterDto>.Fail("Email already exists.");
        }

        var newUser = new User
        {
            Email = request.Email,
            CreatedAt = DateTime.UtcNow,
        };

        newUser.PasswordHash = HashPasswordService.Hash(newUser, request.Password);
        
        await context.Users.AddAsync(newUser);
        await context.SaveChangesAsync(cancellationToken);

        return Response<UserRegisterDto>.Ok(new UserRegisterDto { UserId = newUser.Id });


    }
    
    
}
