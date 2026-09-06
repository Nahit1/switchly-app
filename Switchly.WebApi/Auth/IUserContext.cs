using Switchly.WebApi.Models.Common;

namespace Switchly.WebApi.Auth;

public interface IUserContext
{
    Guid UserId { get; }
    IReadOnlyCollection<OrganizationRoleDto> Organizations { get; }

    bool IsInRole(Guid organizationId, string role);
    bool IsOwner(Guid organizationId);
}