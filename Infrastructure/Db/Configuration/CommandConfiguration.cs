using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Db.Configuration;

public class CommandConfiguration : IEntityTypeConfiguration<Command>
{
    public void Configure(EntityTypeBuilder<Command> builder)
    {
        builder
            .ToTable("Command")
            .HasQueryFilter(x => x.IsActive)
            .HasKey(k => k.Id);
    }
}