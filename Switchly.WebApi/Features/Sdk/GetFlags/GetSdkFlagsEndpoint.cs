using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Switchly.WebApi.Features.Sdk.GetFlags;

public sealed class GetSdkFlagsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/sdk/flags", async (
                [FromQuery] string organizationKey,
                [FromQuery] string projectKey,
                [FromQuery] string environmentKey,
                IMediator mediator,
                CancellationToken ct) =>
            {
                var result = await mediator.Send(
                    new GetSdkFlagsQuery(organizationKey, projectKey, environmentKey), ct);

                return result is null ? Results.NotFound() : Results.Ok(result);
            })
            .WithTags("SDK")
            .WithName("GetSdkFlags")
            .RequireRateLimiting("track");
    }
}
