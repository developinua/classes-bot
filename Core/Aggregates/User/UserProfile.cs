using Core.Entities;
using Core.Entities.Base;

namespace Core.Aggregates.User;

public class UserProfile : BaseModel
{
    public long ChatId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public bool IsPremium { get; set; }
    public bool IsBot { get; set; }
    public Culture Culture { get; set; } = null!;

    public void UpdateCultureFromPreviousProfile(UserProfile userProfile)
    {
        Culture = new Culture
        {
            Id = userProfile.Culture.Id,
            Name = userProfile.Culture.Name,
            Code = userProfile.Culture.Code,
            CreatedAt = Culture.CreatedAt,
            UpdatedAt = DateTime.UtcNow
        };
    }
}