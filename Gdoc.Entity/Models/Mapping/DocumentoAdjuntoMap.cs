using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Gdoc.Entity.Models.Mapping
{
    public class DocumentoAdjuntoMap : EntityTypeConfiguration<DocumentoAdjunto>
    {
        public DocumentoAdjuntoMap()
        {
            // Primary Key
            this.HasKey(t => t.IDDocumentoAdjunto);

            // Properties
            this.Property(t => t.TipoDoctoAdjunto)
                .IsFixedLength()
                .HasMaxLength(2);

            // Table & Column Mappings
            this.ToTable("DocumentoAdjunto");
            this.Property(t => t.IDDocumentoAdjunto).HasColumnName("IDDocumentoAdjunto");
            this.Property(t => t.IDOperacion).HasColumnName("IDOperacion");
            this.Property(t => t.IDAdjunto).HasColumnName("IDAdjunto");
            this.Property(t => t.IDComentarioMesavirtual).HasColumnName("IDComentarioMesavirtual");
            this.Property(t => t.TipoDoctoAdjunto).HasColumnName("TipoDoctoAdjunto");
            this.Property(t => t.EstadoDoctoAdjunto).HasColumnName("EstadoDoctoAdjunto");

            // Relationships
            this.HasOptional(t => t.Operacion)
                .WithMany(t => t.DocumentoAdjuntoes)
                .HasForeignKey(d => d.IDOperacion);

            this.HasOptional(t => t.Adjunto)
                .WithMany(t => t.DocumentoAdjuntoes)
                .HasForeignKey(d => d.IDAdjunto);

        }
    }
}
