using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Switchly.WebApi.Features.Projects.CreateProject;

public class CreateProjectEndpoint:ICarterModule
{
    public sealed class Request
    {
        public Guid OrganizationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
    
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/project/create", async ([FromBody] Request r, IMediator mediator) =>
            {
                var cmd = new CreateProjectRequest(r.OrganizationId, r.Name, r.Description);
                var res = await mediator.Send(cmd);
                return res.Success ? Results.Ok(res) : Results.BadRequest(res);
            })
            .WithTags("Project")
            .WithName("CreateProject")
            .RequireAuthorization();
    }
}