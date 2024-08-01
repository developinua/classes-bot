using Core.Aggregates.User;

namespace Features.Start;

public interface IUserProfileService
{
    Task UpdateUserProfile(UserProfile userProfile, CancellationToken cancel);
}