using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkHive.Data.Models.Entities;

namespace WorkHive.Data.Models.Mappings
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");

            builder.HasKey(u => u.UserId);

            builder
                .Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder
                .Property(u => u.EmailAddress)
                .IsRequired()
                .HasMaxLength(60);

            builder
                .Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(60);
        }
    }
}
