using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TODO.Core.Models.EntityTypeConfiguration
{
    public class TagsConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure (EntityTypeBuilder<Tag> builder) {
            builder.HasKey(tag => tag.Id);
            builder.Property(tag => tag.Name).HasMaxLength(50);
        }

    }
}
