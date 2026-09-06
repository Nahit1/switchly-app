using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Switchly.WebApi.Features.Flags.CreateFlag;

public class CreateFlagEndpoint : ICarterModule
{
    public sealed class Request
    {
        public Guid OrganizationId { get; set; }
        public Guid ProjectId { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
    
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/flag/create", async ([FromBody] Request r, IMediator mediator) =>
            {
                
                var cmd = new CreateFlagRequest(
                    r.OrganizationId, r.ProjectId, r.Key, r.Name, r.Description);
                var res = await mediator.Send(cmd);
                return res.Success ? Results.Ok(res) : Results.BadRequest(res);
            })
            .WithTags("Flag")
            .WithName("CreateFlag")
            .RequireAuthorization();
    }
}
