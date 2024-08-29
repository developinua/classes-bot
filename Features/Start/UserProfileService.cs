using Core.Entities.Aggregates.User;
using Features.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Features.Start;

public class UserProfileService(IClassesDbContext context) : IUserProfileService
{
    public async Task UpdateUserProfile(UserProfile userProfile, CancellationToken cancel)
    {
        var userProfileDb = await context.UserProfiles
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ChatId == userProfile.ChatId, cancel);

        if (userProfileDb is null)
        {
            context.UserProfiles.Add(userProfile);
        }
        else
        {
            context.UserProfiles.Update(userProfile);
        }

        await context.SaveChangesAsync(cancel);
    }
}