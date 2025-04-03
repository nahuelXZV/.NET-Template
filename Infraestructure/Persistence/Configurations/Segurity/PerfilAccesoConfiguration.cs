using Domain.Entities.Segurity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Persistence.Configurations.Segurity;

public class PerfilAccesoConfiguration : IEntityTypeConfiguration<PerfilAcceso>
{
    public void Configure(EntityTypeBuilder<PerfilAcceso> builder)
    {
        builder.ToTable("PerfilAcceso", "Seguridad");

        builder.HasKey(a => a.Id);

        builder.HasOne(c => c.Acceso)
              .WithMany()
              .HasForeignKey(c => c.AccesoId);
    }
}

