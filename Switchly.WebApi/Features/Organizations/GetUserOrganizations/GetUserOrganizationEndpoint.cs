using Carter;
using MediatR;

namespace Switchly.WebApi.Features.Organizations.GetUserOrganizations;

public class GetUserOrganizationEndpoint:ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/organization/getAll", async (IMediator mediator) =>
            {
                
                var res = await mediator.Send(new OrganizationListQuery());
                return res.Success ? Results.Ok(res) : Results.BadRequest(res);
            })
            .WithTags("Organization")
            .WithName("GetAll")
            .RequireAuthorization();
    }
}