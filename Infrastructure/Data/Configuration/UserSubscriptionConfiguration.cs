using Core.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration;

public class UserSubscriptionConfiguration : IEntityTypeConfiguration<UserSubscription>
{
    public void Configure(EntityTypeBuilder<UserSubscription> builder)
    {
        builder
            .ToTable("UserSubscription")
            .HasQueryFilter(x =>
                x.Subscription.IsActive
                && x.RemainingClasses > 0)
            .HasKey(k => k.Id);
    }
}