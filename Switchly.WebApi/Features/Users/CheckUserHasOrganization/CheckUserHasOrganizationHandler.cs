using MediatR;
using Microsoft.EntityFrameworkCore;
using Switchly.WebApi.Auth;
using Switchly.WebApi.Context;
using Switchly.WebApi.Models.Common;

namespace Switchly.WebApi.Features.Users.CheckUserHasOrganization;

public sealed record CheckUserHasOrganizationQuery()
    : IRequest<Response<bool>>;


public class CheckUserHasOrganizationHandler(SwitchlyDbContext db,IUserContext userContext)
    :IRequestHandler<CheckUserHasOrganizationQuery, Response<bool>>
{
    public async Task<Response<bool>> Handle(CheckUserHasOrganizationQuery request, CancellationToken cancellationToken)
    {
        var userId = userContext.UserId;
        var organizationMember = await db.OrganizationMembers
            .FirstOrDefaultAsync(x=>x.UserId == userId, cancellationToken);

        return Response<bool>.Ok(organizationMember != null);
    }
}