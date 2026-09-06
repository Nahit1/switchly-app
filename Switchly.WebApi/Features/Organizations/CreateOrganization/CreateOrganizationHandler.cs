using MediatR;
using Switchly.WebApi.Auth;
using Switchly.WebApi.Context;
using Switchly.WebApi.Entities;
using Switchly.WebApi.Models.Common;
using Switchly.WebApi.Models.Enums;
using Switchly.WebApi.Services.Helpers;

namespace Switchly.WebApi.Features.Organizations.CreateOrganization;

public sealed record CreateOrganizationRequest(string Name):IRequest<Response<CreateOrganizationDto>>;

public sealed record CreateOrganizationDto
{
    public string PublicKey { get; init; }
}

public class CreateOrganizationHandler(SwitchlyDbContext context, IUserContext userContext)
    : IRequestHandler<CreateOrganizationRequest, Response<CreateOrganizationDto>>
{
    public async Task<Response<CreateOrganizationDto>> Handle(CreateOrganizationRequest request, CancellationToken cancellationToken)
    {
        var checkExists = context.Organizations.FirstOrDefault(x => x.Name == request.Name);
        if (checkExists is not null)
        {
            return Response<CreateOrganizationDto>.Fail("This organization already exists.");
        }
        
        var userId = userContext.UserId;
        
        var org = new Organization
        {
            Name = request.Name,
            PublicKey = GenerateOrganizationKey.GenerateKey(),
            CreatedAt = DateTime.UtcNow
        };
        
        await context.Organizations.AddAsync(org, cancellationToken);
        
        var member = new OrganizationMember
        {
            OrganizationId = org.Id,
            UserId = userId,
            Role = OrganizationRole.Owner,
            CreatedAt = DateTime.UtcNow
        };
        
        await context.OrganizationMembers.AddAsync(member, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        
        return Response<CreateOrganizationDto>.Ok(new CreateOrganizationDto
        {
            PublicKey = org.PublicKey,
        });
    }
}