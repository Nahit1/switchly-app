using MediatR;
using Microsoft.EntityFrameworkCore;
using Switchly.WebApi.Auth;
using Switchly.WebApi.Context;
using Switchly.WebApi.Models.Common;

namespace Switchly.WebApi.Features.Organizations.GetUserOrganizations;

public sealed record OrganizationListQuery()
    : IRequest<Response<List<OrganizationListDto>>>;

public sealed record OrganizationListDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string PublicKey { get; set; }
}

public class GetUserOrganizationHandler(SwitchlyDbContext db,IUserContext userContext)
    :IRequestHandler<OrganizationListQuery, Response<List<OrganizationListDto>>>
{
    public async Task<Response<List<OrganizationListDto>>> Handle(OrganizationListQuery request, CancellationToken cancellationToken)
    {
        var userId = userContext.UserId;

        var organization = await db.OrganizationMembers.Where(x => x.UserId == userId)
            .Select(x=>x.Organization)
            .Select(x=> new OrganizationListDto
            {
                Id = x.Id,
                Name = x.Name,
                PublicKey = x.PublicKey
            })
            .ToListAsync(cancellationToken: cancellationToken);
        
        return Response<List<OrganizationListDto>>.Ok(new List<OrganizationListDto>(organization));
    }
}