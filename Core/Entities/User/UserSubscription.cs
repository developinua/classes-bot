using System.Text.Json.Serialization;
using Core.Entities.Base;

namespace Core.Entities.User;

public class UserSubscription : BaseModel
{
    public BotUser BotUser { get; set; } = new();
    public Subscription.Subscription Subscription { get; set; } = new();
    public int RemainingClasses { get; set; }

    [JsonIgnore]
    public bool CanCheckInOnClass => RemainingClasses != 0;

    public void CheckInOnClass()
    {
        if (!CanCheckInOnClass) return;
        RemainingClasses--;
    }
}