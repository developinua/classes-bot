using System.Threading;
using System.Threading.Tasks;
using Core.Aggregates.User;

namespace Application.Features.Language;

public interface IUserProfileService
{
    Task UpdateUserProfile(UserProfile userProfile, CancellationToken cancel);
}