using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Segurity;

namespace Infraestructure.Persistence.Configurations.Segurity;

class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuario", "Seguridad");

        builder.HasKey(a => a.Id);

        builder.HasOne(a => a.Perfil)
            .WithMany()
            .HasForeignKey(a => a.PerfilId);
    }
}