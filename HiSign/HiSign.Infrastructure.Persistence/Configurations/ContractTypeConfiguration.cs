using HiSign.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HiSign.Infrastructure.Persistence.Configurations
{
    public class ContractTypeConfiguration : IEntityTypeConfiguration<ContractType>
    {
        public void Configure(EntityTypeBuilder<ContractType> builder)
        {
            builder.ToTable(nameof(ContractType));

            builder.HasMany(x => x.Contracts).WithOne(x => x.ContractType).HasForeignKey(x => x.ContractTypeId)
                .OnDelete(DeleteBehavior.ClientNoAction);
        }
    }
}
