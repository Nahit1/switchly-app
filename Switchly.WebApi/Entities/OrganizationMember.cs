using Switchly.WebApi.Models.Enums;

namespace Switchly.WebApi.Entities;

public class OrganizationMember
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    public Guid OrganizationId { get; set; }
    public Organization Organization { get; set; } = default!;

    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public OrganizationRole Role { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
}