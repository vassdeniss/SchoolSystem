namespace SchoolSystem.Common;

public class PrincipalDto
{
    public Guid Id { get; set; }
    public string UserFullName { get; set; } = null!;
    public Guid UserId { get; set; }
    public Guid SchoolId { get; set; }
    public string SchoolName { get; set; } = null!;
    public string Specialization { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
}
