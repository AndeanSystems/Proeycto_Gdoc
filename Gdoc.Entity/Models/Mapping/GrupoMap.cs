using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Gdoc.Entity.Models.Mapping
{
    public class GrupoMap : EntityTypeConfiguration<Grupo>
    {
        public GrupoMap()
        {
            // Primary Key
            this.HasKey(t => t.IDGrupo);

            // Properties
            this.Property(t => t.CodigoGrupo)
                .HasMaxLength(15);

            this.Property(t => t.NombreGrupo)
                .HasMaxLength(150);

            this.Property(t => t.UsuarioModifica)
                .HasMaxLength(15);

            this.Property(t => t.ComentarioGrupo)
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("Grupo");
            this.Property(t => t.IDGrupo).HasColumnName("IDGrupo");
            this.Property(t => t.CodigoGrupo).HasColumnName("CodigoGrupo");
            this.Property(t => t.NombreGrupo).HasColumnName("NombreGrupo");
            this.Property(t => t.FechaModifica).HasColumnName("FechaModifica");
            this.Property(t => t.UsuarioModifica).HasColumnName("UsuarioModifica");
            this.Property(t => t.ComentarioGrupo).HasColumnName("ComentarioGrupo");
            this.Property(t => t.EstadoGrupo).HasColumnName("EstadoGrupo");
        }
    }
}
