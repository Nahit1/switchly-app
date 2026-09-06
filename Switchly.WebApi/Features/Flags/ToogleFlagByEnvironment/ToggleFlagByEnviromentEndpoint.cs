using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Switchly.WebApi.Features.Flags.ToogleFlagByEnvironment;

public class ToggleFlagByEnviromentEndpoint:CarterModule
{
    public sealed record ToggleFlagEnvironmentBody(
        Guid OrganizationId,
        Guid ProjectId,
        bool IsEnabled
    );
    
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/flags/{flagId:guid}/environments/{envId:guid}/toggle",
                async (Guid flagId,
                    Guid envId,
                    [FromBody] ToggleFlagEnvironmentBody body,
                    IMediator mediator,
                    CancellationToken ct) =>
                {
                    var cmd = new ToggleFlagEnvironmentCommand(
                        body.OrganizationId,
                        body.ProjectId,
                        flagId,
                        envId,
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