using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TODO.Core.Models;

namespace TODO.Core.Models.EntityTypeConfiguration
{
    public class ReminderConfiguration : IEntityTypeConfiguration<Reminder>
    {
        public void Configure(EntityTypeBuilder<Reminder> builder) { 
             builder.HasKey(reminder => reminder.Id);
            builder.Property(reminder => reminder.Title).HasMaxLength(50);
            builder.Property(reminder => reminder.Description).HasMaxLength(250);
            builder
            .HasMany(reminder => reminder.Tags)
            .WithMany(tag => tag.Reminders)
            .UsingEntity<Dictionary<string, object>>(
                    "ReminderTags",
                    j => j.HasOne<Tag>().WithMany().HasForeignKey("TagId"),
                    j => j.HasOne<Reminder>().WithMany().HasForeignKey("ReminderId"));
        }
    }
}
