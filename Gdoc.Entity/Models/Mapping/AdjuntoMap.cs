using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Gdoc.Entity.Models.Mapping
{
    public class AdjuntoMap : EntityTypeConfiguration<Adjunto>
    {
        public AdjuntoMap()
        {
            // Primary Key
            this.HasKey(t => t.IDAdjunto);

            // Properties
            this.Property(t => t.NombreOriginal)
                .HasMaxLength(200);

            this.Property(t => t.RutaArchivo)
                .HasMaxLength(200);

            this.Property(t => t.TamanoArchivo)
                .HasMaxLength(200);

            this.Property(t => t.TipoArchivo)
                .HasMaxLength(300);

            // Table & Column Mappings
            this.ToTable("Adjunto");
            this.Property(t => t.IDAdjunto).HasColumnName("IDAdjunto");
            this.Property(t => t.IDUsuario).HasColumnName("IDUsuario");
            this.Property(t => t.NombreOriginal).HasColumnName("NombreOriginal");
            this.Property(t => t.RutaArchivo).HasColumnName("RutaArchivo");
            this.Property(t => t.TamanoArchivo).HasColumnName("TamanoArchivo");
            this.Property(t => t.FechaRegistro).HasColumnName("FechaRegistro");
            this.Property(t => t.EstadoAdjunto).HasColumnName("EstadoAdjunto");
            this.Property(t => t.TipoArchivo).HasColumnName("TipoArchivo");

            // Relationships
            this.HasOptional(t => t.Usuario)
                .WithMany(t => t.Adjuntoes)
                .HasForeignKey(d => d.IDUsuario);

        }
    }
}
