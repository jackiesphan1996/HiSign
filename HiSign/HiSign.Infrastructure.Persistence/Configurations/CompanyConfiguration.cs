using HiSign.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HiSign.Infrastructure.Persistence.Configurations
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable(nameof(Company));

            builder.HasMany(x => x.ApplicationUsers).WithOne(x => x.Company).HasForeignKey(x => x.CompanyId);
            builder.HasMany(x => x.ContractTypes).WithOne(x => x.Company).HasForeignKey(x => x.CompanyId);
            builder.HasMany(x => x.Contracts).WithOne(x => x.Company).HasForeignKey(x => x.CompanyId);
        }
    }
}
