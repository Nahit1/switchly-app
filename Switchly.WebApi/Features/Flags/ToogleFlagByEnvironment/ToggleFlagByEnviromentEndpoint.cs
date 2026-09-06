using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Switchly.WebApi.Features.Flags.ToogleFlagByEnvironment;

public class ToggleFlagByEnviromentEndpoint : ICarterModule
{
    public sealed record ToggleFlagEnvironmentBody(
        Guid OrganizationId,
        Guid ProjectId,
        bool IsEnabled
    );
    
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/flags/{flagId:guid}/environments/{featureFlagEnvironmentId:guid}/toggle",
                async (Guid flagId,
                    Guid featureFlagEnvironmentId,
                    [FromBody] ToggleFlagEnvironmentBody body,
                    IMediator mediator,
                    CancellationToken ct) =>
                {
                    var cmd = new ToggleFlagEnvironmentCommand(
                        body.OrganizationId,
                        body.ProjectId,
                        flagId,
                        featureFlagEnvironmentId,
                        body.IsEnabled
                    );

                    var res = await mediator.Send(cmd, ct);
                    return res.Success ? Results.Ok(res) : Results.BadRequest(res);
                })
            .WithTags("Flags")
            .WithName("ToggleFlagEnvironment")
            .RequireAuthorization();
    }
}
