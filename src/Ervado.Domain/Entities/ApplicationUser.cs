using Microsoft.AspNetCore.Identity;

namespace Ervado.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int? DomainId { get; set; }
    public int? CompanyId { get; set; }
    public int CreatedUserId { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedUserId { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public int? DeleteUserId { get; set; }
    public int? DeleteDate { get; set; }
    public bool IsDeleted { get; set; }
} 