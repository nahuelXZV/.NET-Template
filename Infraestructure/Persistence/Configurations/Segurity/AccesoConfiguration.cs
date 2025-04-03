using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Segurity;

namespace Infraestructure.Persistence.Configurations.Segurity;

public class AccesoConfiguration : IEntityTypeConfiguration<Acceso>
{
    public void Configure(EntityTypeBuilder<Acceso> builder)
    {
        builder.ToTable("Acceso", "Seguridad");

        builder.HasKey(a => a.Id);

        builder.HasOne(c => c.Modulo)
              .WithMany()
              .HasForeignKey(c => c.ModuloId);
    }
}

