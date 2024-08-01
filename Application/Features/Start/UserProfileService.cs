using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Core.Aggregates.User;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Start;

public class UserProfileService(IClassesDbContext context) : IUserProfileService
{
    public async Task UpdateUserProfile(UserProfile userProfile, CancellationToken cancel)
    {
        var userProfileDb = await context.UserProfiles
            .AsNoTracking()
            .Include(x => x.Culture)
            .FirstOrDefaultAsync(x => x.ChatId == userProfile.ChatId, cancel);

        if (userProfileDb is null)
        {
            // todo: check logic
            userProfileDb = context.UserProfiles.Add(userProfile).Entity;
            await context.SaveChangesAsync(cancel);
        }
        
        userProfile.UpdateCultureFromPreviousProfile(userProfileDb);
        context.UserProfiles.Update(userProfile);
        await context.SaveChangesAsync(cancel);
    }
}