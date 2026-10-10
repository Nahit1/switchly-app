using MediatR;
using Microsoft.EntityFrameworkCore;
using Switchly.WebApi.Context;

namespace Switchly.WebApi.Features.Sdk.GetFlags;

public sealed record GetSdkFlagsQuery(
    string OrganizationKey,
    string ProjectKey,
    string EnvironmentKey) : IRequest<SdkFlagsResponse?>;

public sealed record SdkFlagsResponse(
    IReadOnlyDictionary<string, bool> Flags);

public sealed class GetSdkFlagsHandler(SwitchlyDbContext db)
    : IRequestHandler<GetSdkFlagsQuery, SdkFlagsResponse?>
{
    public async Task<SdkFlagsResponse?> Handle(GetSdkFlagsQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.OrganizationKey) ||
            string.IsNullOrWhiteSpace(request.ProjectKey) ||
            string.IsNullOrWhiteSpace(request.EnvironmentKey))
            return null;

        var environmentId = await db.ProjectEnvironments
            .AsNoTracking()
            .Where(e =>
                e.Key == request.EnvironmentKey &&
                e.FlagsProject.Key == request.ProjectKey &&
                !e.FlagsProject.IsArchived &&
                e.FlagsProject.Organization.PublicKey == request.OrganizationKey)
            .Select(e => e.Id)
            .SingleOrDefaultAsync(cancellationToken);

        if (environmentId == Guid.Empty)
            return null;

        var flags = await db.FeatureFlagEnvironments
            .AsNoTracking()
            .Where(fe =>
                fe.ProjectEnvironmentId == environmentId &&
                !fe.FeatureFlag.IsArchived)
            .OrderBy(fe => fe.FeatureFlag.Key)
            .Select(fe => new { fe.FeatureFlag.Key, fe.IsEnabled })
            .ToDictionaryAsync(fe => fe.Key, fe => fe.IsEnabled, cancellationToken);

        return new SdkFlagsResponse(flags);
    }
}
