namespace SchoolSystem.Web.Models;

public class PrincipalViewModel
{
    public Guid Id { get; set; }
    
    public string? FullName { get; set; }
    
    public Guid UserId { get; set; }
    public Guid SchoolId { get; set; }
    public string? SchoolName { get; set; }
    public string? Specialization { get; set; }
    public string? PhoneNumber { get; set; }
}
