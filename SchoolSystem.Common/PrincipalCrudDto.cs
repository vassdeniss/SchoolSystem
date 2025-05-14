namespace SchoolSystem.Common;

public class PrincipalCrudDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid SchoolId { get; set; }
    public string Specialization { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
}
