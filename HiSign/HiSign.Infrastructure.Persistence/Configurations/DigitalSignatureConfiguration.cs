using HiSign.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HiSign.Infrastructure.Persistence.Configurations
{
    public class DigitalSignatureConfiguration : IEntityTypeConfiguration<DigitalSignature>
    {
        public void Configure(EntityTypeBuilder<DigitalSignature> builder)
        {
            builder.ToTable(nameof(DigitalSignature));
            builder.HasOne(x => x.Company).WithMany(x => x.DigitalSignatures).HasForeignKey(x => x.CompanyId);
        }
    }
}
