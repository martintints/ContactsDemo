using Contacts.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Contacts.Infrastructure.DAL.EntityConfigurations
{
    public class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(baseEntity => baseEntity.Id);

            Seed(builder);
        }

        public virtual void Seed(EntityTypeBuilder<T> builder)
        {
        }
    }

}
