using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Core.Aggregates.Subscription;
using Core.Aggregates.User;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ResultNet;

namespace Application.Features.Administration;

public class SeedHandler(
        IBotService botService,
        IClassesDbContext context)
    : IRequestHandler<SeedRequest, Result>
{
    public async Task<Result> Handle(SeedRequest request, CancellationToken cancel)
    {
        botService.UseChat(request.ChatId);
        
        if (!CanExecuteCommand(request.Username))
        {
            const string errorMessage = "Access denied. You can't execute this command.";
            await botService.SendTextMessageAsync(errorMessage, cancel);
            return Result.Failure().WithMessage(errorMessage);
        }

        var processSubscriptions = await AddDefaultSubscriptions(cancel);
        
        if (processSubscriptions.IsFailure())
            return Result.Failure().WithMessage("Error while seeding data.");

        await botService.SendTextMessageAsync("*Successfully seeded*", cancel);
        
        return Result.Success();
    }
    
    // todo: extract to separate class
    private static bool CanExecuteCommand(string username)
    {
        var allowedUsers = new[] { "nazikBro", "taras_zouk", "kovalinas" };
        return allowedUsers.Any(x => x.Equals(username));
    }

    private async Task<Result> AddDefaultSubscriptions(CancellationToken cancel)
    {
        await context.Subscriptions
            .Where(x => x.IsActive == true)
            .ExecuteDeleteAsync(cancel);

        var subscriptions = new List<Subscription>
        {
            #region SubscriptionType.None

            new()
            {
                Name = "Whoops",
                Description = "Nothing to do here",
                Price = 0,
                DiscountPercent = 0,
                ClassesCount = 0,
                Type = SubscriptionType.None,
                IsActive = true
            },

            #endregion

            #region SubscriptionType.Novice

            new()
            {
                Name = "One class",
                Description = "One class",
                Price = 200,
                DiscountPercent = 0,
                ClassesCount = 1,
                Type = SubscriptionType.Class,
                IsActive = true
            },
            new()
            {
                Name = "Week Classes",
                Description = "Two classes",
                Price = 400,
                DiscountPercent = 0,
                ClassesCount = 2,
                Type = SubscriptionType.Class,
                IsActive = true
            },
            new()
            {
                Name = "Two Week Classes",
                Description = "Four classes",
                Price = 800,
                DiscountPercent = 0,
                ClassesCount = 4,
                Type = SubscriptionType.Class,
                IsActive = true
            },
            new()
            {
                Name = "One Month Classes",
                Description = "Eight classes",
                Price = 1600,
                DiscountPercent = 0,
                ClassesCount = 8,
                Type = SubscriptionType.Class,
                IsActive = true
            },
            new()
            {
                Name = "Three Months Classes",
                Description = "Sixteen classes",
                Price = 3200,
                DiscountPercent = 15,
                ClassesCount = 16,
                Type = SubscriptionType.Class,
                IsActive = true
            },

            #endregion

            #region SubscriptionType.Medium

            new()
            {
                Name = "One class",
                Description = "One class",
                Price = 200,
                DiscountPercent = 0,
                ClassesCount = 1,
                Type = SubscriptionType.Class,
                IsActive = true
            },
            new()
            {
                Name = "Week Classes",
                Description = "Two classes",
                Price = 400,
                DiscountPercent = 0,
                ClassesCount = 2,
                Type = SubscriptionType.Class,
                IsActive = true
            },
            new()
            {
                Name = "Two Week Classes",
                Description = "Four classes",
                Price = 800,
                DiscountPercent = 0,
                ClassesCount = 4,
                Type = SubscriptionType.Class,
                IsActive = true
            },
            new()
            {
                Name = "One Month Classes",
                Description = "Eight classes",
                Price = 1600,
                DiscountPercent = 0,
                ClassesCount = 8,
                Type = SubscriptionType.Class,
                IsActive = true
            },
            new()
            {
                Name = "Three Months Classes",
                Description = "Sixteen classes",
                Price = 3200,
                DiscountPercent = 15,
                ClassesCount = 16,
                Type = SubscriptionType.Class,
                IsActive = true
            },

            #endregion

            #region SubscriptionType.LadyStyling

            new()
            {
                Name = "One class",
                Description = "One class",
                Price = 200,
                DiscountPercent = 0,
                ClassesCount = 1,
                Type = SubscriptionType.Class,
                IsActive = true
            },
            new()
            {
                Name = "Week Classes",
                Description = "Two classes",
                Price = 400,
                DiscountPercent = 0,
                ClassesCount = 2,
                Type = SubscriptionType.Class,
                IsActive = true
            },
            new()
            {
                Name = "Two Week Classes",
                Description = "Four classes",
                Price = 800,
                DiscountPercent = 0,
                ClassesCount = 4,
                Type = SubscriptionType.Class,
                IsActive = true
            },
            new()
            {
                Name = "One Month Classes",
                Description = "Eight classes",
                Price = 1600,
                DiscountPercent = 0,
                ClassesCount = 8,
                Type = SubscriptionType.Class,
                IsActive = true
            },
            new()
            {
                Name = "Three Months Classes",
                Description = "Sixteen classes",
                Price = 3200,
                DiscountPercent = 15,
                ClassesCount = 16,
                Type = SubscriptionType.Class,
                IsActive = true
            },

            #endregion
            
            #region SubscriptionType.ManStyling

            new()
            {
                Name = "One class",
                Description = "One class",
                Price = 200,
                DiscountPercent = 0,
                ClassesCount = 1,
                Type = SubscriptionType.Class,
                IsActive = true
            },
            new()
            {
                Name = "Week Classes",
                Description = "Two classes",
                Price = 400,
                DiscountPercent = 0,
                ClassesCount = 2,
                Type = SubscriptionType.Class,
                IsActive = true
            },
            new()
            {
                Name = "Two Week Classes",
                Description = "Four classes",
                Price = 800,
                DiscountPercent = 0,
                ClassesCount = 4,
                Type = SubscriptionType.Class,
                IsActive = true
            },
            new()
            {
                Name = "One Month Classes",
                Description = "Eight classes",
                Price = 1600,
                DiscountPercent = 0,
                ClassesCount = 8,
                Type = SubscriptionType.Class,
                IsActive = true
            },
            new()
            {
                Name = "Three Months Classes",
                Description = "Sixteen classes",
                Price = 3200,
                DiscountPercent = 15,
                ClassesCount = 16,
                Type = SubscriptionType.Class,
                IsActive = true
            },

            #endregion

            #region SubscriptionType.Premium

            new()
            {
                Name = "All classes",
                Description = "Novice, medium, lady style classes for week",
                Price = 1200,
                DiscountPercent = 5,
                ClassesCount = 12,
                Type = SubscriptionType.Class,
                IsActive = true
            },
            new()
            {
                Name = "All classes",
                Description = "Novice, medium, lady style classes for two weeks",
                Price = 2400,
                DiscountPercent = 5,
                ClassesCount = 24,
                Type = SubscriptionType.Class,
                IsActive = true
            },
            new()
            {
                Name = "All classes",
                Description = "Novice, medium, lady style classes for month",
                Price = 4800,
                DiscountPercent = 10,
                ClassesCount = 48,
                Type = SubscriptionType.Class,
                IsActive = true
            },
            new()
            {
                Name = "All classes",
                Description = "Novice, medium, lady style classes for three months",
                Price = 9600,
                DiscountPercent = 15,
                ClassesCount = 144,
                Type = SubscriptionType.Course,
                IsActive = true
            }

            #endregion
        };

        await context.Subscriptions.AddRangeAsync(subscriptions, cancel);
        await context.SaveChangesAsync(cancel);
        
        var userNazar = await context.Users
            .AsNoTracking()
            .Include(x => x.UserProfile.Culture)
            .FirstOrDefaultAsync(x => x.NickName == "nazikBro", cancel);
        
        if (userNazar is null)
            return Result.Failure().WithMessage("Invalid admin subscriptions data in db.");
        
        var userSubscriptions = subscriptions.Select(x => new UserSubscription
        {
            BotUser = userNazar,
            Subscription = x,
            RemainingClasses = x.ClassesCount
        });
        
        await context.UserSubscriptions.AddRangeAsync(userSubscriptions, cancel);
        await context.SaveChangesAsync(cancel);
        
        return Result.Success();
    }
}