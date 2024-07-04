using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace TODO.Core.Models.EntityTypeConfiguration
{

    public class NoteConfiguration : IEntityTypeConfiguration<Note>
    {
        public void Configure(EntityTypeBuilder<Note> builder) { 
            builder.HasKey(note => note.Id);
            builder.Property(note => note.Title).HasMaxLength(50);
            builder.Property(note => note.Description).HasMaxLength(250);
            builder
            .HasMany(note => note.Tags)
            .WithMany(tag => tag.Notes)
            .UsingEntity<Dictionary<string, object>>(
                    "NoteTags",
                    j => j.HasOne<Tag>().WithMany().HasForeignKey("TagId"),
                    j => j.HasOne<Note>().WithMany().HasForeignKey("NoteId"));

        }
    }
}
