﻿using Core.Aggregates.Subscription;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Db.Configuration;

public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder
            .ToTable("Subscription")
            .HasQueryFilter(x => x.IsActive)
            .HasKey(k => k.Id);
    }
}