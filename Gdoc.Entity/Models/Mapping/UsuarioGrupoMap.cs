using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Gdoc.Entity.Models.Mapping
{
    public class UsuarioGrupoMap : EntityTypeConfiguration<UsuarioGrupo>
    {
        public UsuarioGrupoMap()
        {
            // Primary Key
            this.HasKey(t => t.IDUsuarioGrupo);

            // Properties
            this.Property(t => t.UsuarioRegistro)
                .HasMaxLength(15);

            // Table & Column Mappings
            this.ToTable("UsuarioGrupo");
            this.Property(t => t.IDUsuarioGrupo).HasColumnName("IDUsuarioGrupo");
            this.Property(t => t.IDUsuario).HasColumnName("IDUsuario");
            this.Property(t => t.IDGrupo).HasColumnName("IDGrupo");
            this.Property(t => t.UsuarioRegistro).HasColumnName("UsuarioRegistro");
            this.Property(t => t.FechaRegistro).HasColumnName("FechaRegistro");
            this.Property(t => t.EstadoUsuarioGrupo).HasColumnName("EstadoUsuarioGrupo");

            // Relationships
            this.HasRequired(t => t.Grupo)
                .WithMany(t => t.UsuarioGrupoes)
                .HasForeignKey(d => d.IDGrupo);
            this.HasRequired(t => t.Usuario)
                .WithMany(t => t.UsuarioGrupoes)
                .HasForeignKey(d => d.IDUsuario);

        }
    }
}
