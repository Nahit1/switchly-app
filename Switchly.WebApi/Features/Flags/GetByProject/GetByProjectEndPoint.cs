using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Switchly.WebApi.Features.Flags.GetByProject;

public class GetByProjectEndPoint:ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/flag/get-by-project", async ([FromQuery]Guid organizationId, Guid projectId,IMediator mediator) =>
            {
                
                var res = await mediator.Send(new GetOrganizationListQuery(organizationId, projectId));
                return res.Success ? Results.Ok(res) : Results.BadRequest(res);
            })
            .WithTags("Flag")
            .WithName("GetByProject")
            .RequireAuthorization();
    }
}