using Core.Entities.Base;

namespace Core.Entities.User;

public class BotUser : BaseModel
{
    public string NickName { get; set; } = string.Empty;
    public UserProfile UserProfile { get; set; } = null!;
    public bool IsActive { get; set; }
}