using MediatR;
using Microsoft.EntityFrameworkCore;
using Switchly.WebApi.Auth;
using Switchly.WebApi.Context;
using Switchly.WebApi.Entities;
using Switchly.WebApi.Models.Common;
using Switchly.WebApi.Models.Enums;

namespace Switchly.WebApi.Features.Flags.CreateFlag;

public record CreateFlagRequest(
    Guid OrganizationId,
    Guid ProjectId,
    string Key,
    string Name,
    string? Description
): IRequest<Response<CreateFlagDto>>;

public sealed record CreateFlagDto
{
    public Guid Id { get; init; }
}


public class CreateFlagHandler(SwitchlyDbContext context, IUserContext userContext)
    : IRequestHandler<CreateFlagRequest, Response<CreateFlagDto>>
{
    public async Task<Response<CreateFlagDto>> Handle(CreateFlagRequest request, CancellationToken cancellationToken)
    {
        var isMember = await context.OrganizationMembers
            .AnyAsync(m =>
                m.OrganizationId == request.OrganizationId &&
                m.UserId == userContext.UserId &&
                (m.Role == OrganizationRole.Admin || m.Role == OrganizationRole.Owner), cancellationToken);

        if (!isMember)
            return Response<CreateFlagDto>.Fail("Bu organization için flag oluşturma yetkin yok.");
        
        var project = await context.FlagsProjects
            .FirstOrDefaultAsync(p =>
                p.Id == request.ProjectId &&
                p.OrganizationId == request.OrganizationId, cancellationToken);

        if (project is null)
            throw new InvalidOperationException("Project bulunamadı.");
        
        var exists = await context.FeatureFlags
            .AnyAsync(f =>
                f.ProjectId == project.Id &&
                f.Key == request.Key, cancellationToken);

        if (exists)
            throw new InvalidOperationException("Bu key ile daha önce flag tanımlanmış.");
        
        var envs = await context.ProjectEnvironments
            .AsNoTracking()
            .Where(e => e.ProjectId == request.ProjectId)
            .OrderBy(e => e.SortOrder)
            .ToListAsync(cancellationToken);
        
        var now = DateTimeOffset.UtcNow;

        var flag = new FeatureFlag
        {
            ProjectId = project.Id,
            Key = request.Key,
            Name = request.Name,
            Description = request.Description,
            IsArchived = false,
            CreatedAt = now
        };
        
        context.FeatureFlags.Add(flag);
        
        var flagEnvs = envs.Select(env =>
        {
            var isDefault = env.IsDefault;

            return new FeatureFlagEnvironment
            {
                Id = Guid.NewGuid(),
                FeatureFlagId = flag.Id,
                ProjectEnvironmentId = env.Id,
                IsEnabled = isDefault,
                DefaultRolloutKind = isDefault ? RolloutKind.AllUsers : RolloutKind.Off,
                DefaultRolloutPercentage = isDefault ? 100 : 0,

                UpdatedAt = now
            };
        }).ToList();
        
        context.FeatureFlagEnvironments.AddRange(flagEnvs);
        
        await context.SaveChangesAsync(cancellationToken);

        return Response<CreateFlagDto>.Ok(new CreateFlagDto
        {
            Id = flag.Id,
        });
    }
}
