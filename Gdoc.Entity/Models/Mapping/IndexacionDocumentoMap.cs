using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Gdoc.Entity.Models.Mapping
{
    public class IndexacionDocumentoMap : EntityTypeConfiguration<IndexacionDocumento>
    {
        public IndexacionDocumentoMap()
        {
            // Primary Key
            this.HasKey(t => t.IDIndiceDocto);

            // Properties
            this.Property(t => t.DescripcionIndice)
                .HasMaxLength(100);

            this.Property(t => t.EstadoIndice)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.CodigoTipoOperacion)
                .HasMaxLength(5);

            // Table & Column Mappings
            this.ToTable("IndexacionDocumento");
            this.Property(t => t.IDIndiceDocto).HasColumnName("IDIndiceDocto");
            this.Property(t => t.DescripcionIndice).HasColumnName("DescripcionIndice");
            this.Property(t => t.EstadoIndice).HasColumnName("EstadoIndice");
            this.Property(t => t.IDOperacion).HasColumnName("IDOperacion");
            this.Property(t => t.CodigoTipoOperacion).HasColumnName("CodigoTipoOperacion");

            // Relationships
            this.HasOptional(t => t.Operacion)
                .WithMany(t => t.IndexacionDocumentoes)
                .HasForeignKey(d => d.IDOperacion);

        }
    }
}
