using Core.Entities.Base;

namespace Core.Entities.Aggregates.User;

public class UserSubscription : BaseModel
{
    public User User { get; set; } = new();
    public Subscription.Subscription Subscription { get; set; } = new();
    public int RemainingClasses { get; set; }

    public bool CanCheckinOnClass() => RemainingClasses > 0;

    public bool CheckInOnClass()
    {
        if (!CanCheckinOnClass())
        {
            return false;
        }
        
        RemainingClasses--;
        return true;
    }
}