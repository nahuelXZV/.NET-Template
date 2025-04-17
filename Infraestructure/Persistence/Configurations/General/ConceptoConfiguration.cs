using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Configuration;

namespace Infraestructure.Persistence.Configurations.General;

public class ConceptoConfiguration : IEntityTypeConfiguration<Concepto>
{
    public void Configure(EntityTypeBuilder<Concepto> builder)
    {
        builder.ToTable("Concepto", "General");

        builder.HasKey(a => a.Id);
    }
}
