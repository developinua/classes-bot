using Core.Aggregates.User;

namespace Features.Language;

public interface IUserProfileService
{
    Task UpdateUserProfile(UserProfile userProfile, CancellationToken cancel);
}