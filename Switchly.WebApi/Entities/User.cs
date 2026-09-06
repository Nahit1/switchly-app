namespace Switchly.WebApi.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Email { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public DateTimeOffset CreatedAt { get; set; }
    
    public ICollection<OrganizationMember> OrganizationMembers { get; set; } = new List<OrganizationMember>();
}