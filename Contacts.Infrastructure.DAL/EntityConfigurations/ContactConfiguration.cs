using Contacts.Domain.Contact;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Contacts.Infrastructure.DAL.EntityConfigurations
{
    public class ContactConfiguration : BaseEntityConfiguration<Contact>
    {
        public override void Configure(EntityTypeBuilder<Contact> builder)
        {
            base.Configure(builder);

            builder.Property(p => p.FirstName).HasMaxLength(100).IsRequired();
            builder.Property(p => p.LastName).HasMaxLength(100).IsRequired();
            builder.Property(p => p.Email).HasMaxLength(254).IsRequired();
            builder.Property(p => p.Sequence).HasColumnType("NUMERIC(32,16)").IsRequired();
            builder.Property(p => p.Phone).HasMaxLength(254);

            builder.HasIndex(p => p.Sequence);
        }

        public override void Seed(EntityTypeBuilder<Contact> builder)
        {
            builder.HasData(
                new Contact()
                {
                    Id = 1,
                    FirstName = "Juhan",
                    LastName = "Juurikas",
                    Phone = "+3725123456",
                    Email = "juhan.juurikas@gmail.com",
                    Sequence = 4000
                },
                new Contact()
                {
                    Id = 2,
                    FirstName = "Mari",
                    LastName = "Maasikas",
                    Phone = "+3725223456",
                    Email = "mari.maasikas@gmail.com",
                    Sequence = 2000
                },
                new Contact()
                {
                    Id = 3,
                    FirstName = "John",
                    LastName = "Doe",
                    Phone = "+1-202-555-0139",
                    Email = "john.doe@gmail.com",
                    Sequence = 1000
                },
                new Contact()
                {
                    Id = 4,
                    FirstName = "Jane",
                    LastName = "Doe",
                    Phone = "+1-202-555-0182",
                    Email = "jane.doe@gmail.com",
                    Sequence = 9000
                }
                );
        }
    }
}
