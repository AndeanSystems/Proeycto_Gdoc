using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Gdoc.Entity.Models.Mapping
{
    public class AccesoSistemaMap : EntityTypeConfiguration<AccesoSistema>
    {
        public AccesoSistemaMap()
        {
            // Primary Key
            this.HasKey(t => t.IDAcceso);

            // Properties
            this.Property(t => t.IdeUsuarioRegistro)
                .HasMaxLength(15);

            // Table & Column Mappings
            this.ToTable("AccesoSistema");
            this.Property(t => t.IDAcceso).HasColumnName("IDAcceso");
            this.Property(t => t.IdeUsuarioRegistro).HasColumnName("IdeUsuarioRegistro");
            this.Property(t => t.FechaModificacion).HasColumnName("FechaModificacion");
            this.Property(t => t.EstadoAcceso).HasColumnName("EstadoAcceso");
            this.Property(t => t.IDUsuario).HasColumnName("IDUsuario");
            this.Property(t => t.IDModuloPagina).HasColumnName("IDModuloPagina");

            // Relationships
            this.HasOptional(t => t.Usuario)
                .WithMany(t => t.AccesoSistemas)
                .HasForeignKey(d => d.IDUsuario);
            this.HasOptional(t => t.ModuloPaginaUrl)
                .WithMany(t => t.AccesoSistemas)
                .HasForeignKey(d => d.IDModuloPagina);

        }
    }
}
