using Core.Entities.Base;

namespace Core.Aggregates.User;

public class BotUser : BaseModel
{
    public string NickName { get; set; } = string.Empty;
    public UserProfile UserProfile { get; set; } = null!;
    public bool IsActive { get; set; }
}