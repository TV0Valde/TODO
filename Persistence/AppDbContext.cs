using Microsoft.EntityFrameworkCore;
using TODO.Core.Models;
using TODO.Core.Models.EntityTypeConfiguration;
namespace TODO.Persistence
{
    public class AppDbContext :DbContext
    {
        public DbSet<Note> Notes { get; set; }
        public DbSet<Reminder> Reminders { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
       
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new NoteConfiguration());
            builder.ApplyConfiguration(new ReminderConfiguration());
            builder.ApplyConfiguration(new TagsConfiguration());
            base.OnModelCreating(builder);
        }
    }
}
