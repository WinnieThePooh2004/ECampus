using ECampus.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Serilog.Events;

namespace ECampus.Infrastructure.ModelConfigurations;

public class LogConfiguration : IEntityTypeConfiguration<Log>
{
    public void Configure(EntityTypeBuilder<Log> builder)
    {
        builder.ToTable("Logs", options => options.ExcludeFromMigrations());
        builder.Property(log => log.Level)
            .HasConversion(level => level.ToString(),
                level => Enum.Parse<LogEventLevel>(level));
    }
}