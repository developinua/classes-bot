using Core.Entities.Base;

namespace Core.Entities.Aggregates.User;

public class UserProfile : BaseModel
{
    public long ChatId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Phone { get; set; }
    public DateOnly DateBirth { get; set; }
}