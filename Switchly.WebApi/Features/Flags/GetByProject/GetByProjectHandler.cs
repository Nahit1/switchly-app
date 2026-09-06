using MediatR;
using Microsoft.EntityFrameworkCore;
using Switchly.WebApi.Auth;
using Switchly.WebApi.Context;
using Switchly.WebApi.Models.Common;
using Switchly.WebApi.Models.Enums;

namespace Switchly.WebApi.Features.Flags.GetByProject;
public sealed record GetOrganizationListQuery(Guid organizationId, Guid projectId)
    : IRequest<Response<List<GetFlagByProjectDto>>>;

public sealed record GetFlagByProjectDto
{
    public Guid Id { get; set; }
    public string Key { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
  // Boolean, Multivariant, Config
    public DateTimeOffset CreatedAt { get; set; }
    public List<GetFlagEnvironmentDto> Environments { get; set; } = new();
}

public sealed record GetFlagEnvironmentDto
{
    public Guid ProjectEnvironmentId { get; set; }
    public Guid FeatureFlagEnvironmentId { get; set; }
    public string EnvironmentKey { get; set; } = default!;
    public string EnvironmentName { get; set; } = default!;

    public bool IsEnabled { get; set; }
    public RolloutKind DefaultRolloutKind { get; set; }
    public int DefaultRolloutPercentage { get; set; }

}

public class GetByProjectHandler(SwitchlyDbContext context, IUserContext userContext)
    :IRequestHandler<GetOrganizationListQuery, Response<List<GetFlagByProjectDto>>>
{
    public async Task<Response<List<GetFlagByProjectDto>>> Handle(GetOrganizationListQuery request, CancellationToken cancellationToken)
    {
        var isMember = await context.OrganizationMembers
            .AnyAsync(m =>
                m.OrganizationId == request.organizationId &&
                m.UserId == userContext.UserId, cancellationToken);
        
        if (!isMember)
            throw new UnauthorizedAccessException("Bu organization için yetkin yok.");
        
        var flagList = await context.FeatureFlags
            .AsNoTracking()
            .Where(f => f.ProjectId == request.projectId)
            .OrderBy(f => f.Key)
            .Select(f => new GetFlagByProjectDto
            {
                Id = f.Id,
                Key = f.Key,
                Name = f.Name,
                Description = f.Description,
                CreatedAt = f.CreatedAt,
                Environments = f.FeatureFlagEnvironments
                    .OrderBy(fe => fe.ProjectEnvironment.SortOrder)
                    .Select(fe => new GetFlagEnvironmentDto
                    {
                        ProjectEnvironmentId = fe.ProjectEnvironmentId,
                        EnvironmentKey = fe.ProjectEnvironment.Key,
                        EnvironmentName = fe.ProjectEnvironment.Name,
                        IsEnabled = fe.IsEnabled,
                        DefaultRolloutKind = fe.DefaultRolloutKind,
                        DefaultRolloutPercentage = fe.DefaultRolloutPercentage,
                        FeatureFlagEnvironmentId = fe.Id,
                        

                    }).ToList(),
                
            })
            .ToListAsync(cancellationToken);
        
        return Response<List<GetFlagByProjectDto>>.Ok(flagList, "Flag list");
    }
}