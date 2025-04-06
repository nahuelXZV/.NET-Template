
using Domain.Entities.Segurity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Persistence.Configurations.Segurity;

public class ModuloConfiguration : IEntityTypeConfiguration<Modulo>
{
    public void Configure(EntityTypeBuilder<Modulo> builder)
    {
        builder.ToTable("Modulo", "Seguridad");

        builder.HasKey(a => a.Id);

    }
}

