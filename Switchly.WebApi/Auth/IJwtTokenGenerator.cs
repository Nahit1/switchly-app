using Switchly.WebApi.Models.Common;

namespace Switchly.WebApi.Auth;

public interface IJwtTokenGenerator
{
    string GenerateToken(Guid userId, IEnumerable<OrganizationRoleDto> organizations);
}