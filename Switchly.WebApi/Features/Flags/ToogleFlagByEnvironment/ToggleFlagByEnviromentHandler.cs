using MediatR;
using Microsoft.EntityFrameworkCore;
using Switchly.WebApi.Auth;
using Switchly.WebApi.Context;
using Switchly.WebApi.Models.Common;
using Switchly.WebApi.Models.Enums;

namespace Switchly.WebApi.Features.Flags.ToogleFlagByEnvironment;

public sealed record ToggleFlagEnvironmentCommand(
    Guid OrganizationId,
    Guid ProjectId,
    Guid FeatureFlagId,
    Guid FeatureFlagEnvironmentId,
    bool IsEnabled
) : IRequest<Response<bool>>;

public class ToggleFlagByEnviromentHandler(
    SwitchlyDbContext context,
    IUserContext userContext
) : IRequestHandler<ToggleFlagEnvironmentCommand, Response<bool>>
{
    public async Task<Response<bool>> Handle(ToggleFlagEnvironmentCommand request, CancellationToken cancellationToken)
    {
        var hasPermission = await context.OrganizationMembers
            .AsNoTracking()
            .AnyAsync(m =>
                m.OrganizationId == request.OrganizationId &&
                m.UserId == userContext.UserId &&
                (m.Role == OrganizationRole.Admin || m.Role == OrganizationRole.Owner), cancellationToken);

        if (!hasPermission)
            return Response<bool>.Fail("Bu organization için flag değiştirme yetkin yok.");
        
        var valid = await context.FeatureFlagEnvironments
            .AsNoTracking()
            .AnyAsync(x =>
                    x.FeatureFlagId == request.FeatureFlagId &&
                    x.Id == request.FeatureFlagEnvironmentId &&
                    x.FeatureFlag.ProjectId == request.ProjectId &&
                    x.FeatureFlag.FlagsProject.OrganizationId == request.OrganizationId,
                cancellationToken);

        if (!valid)
            return Response<bool>.Fail("Flag ve environment bu project'e ait değil.");
        
        var entity = await context.FeatureFlagEnvironments
            .FirstOrDefaultAsync(x =>
                x.FeatureFlagId == request.FeatureFlagId &&
                x.Id == request.FeatureFlagEnvironmentId, cancellationToken);

        if (entity is null)
            return Response<bool>.Fail("FeatureFlagEnvironment kaydı bulunamadı.");
        
        // ✅ SADECE toggle
        entity.IsEnabled = request.IsEnabled;
        entity.UpdatedAt = DateTimeOffset.UtcNow;

        await context.SaveChangesAsync(cancellationToken);

        return Response<bool>.Ok(true, "Updated");
    }
}
