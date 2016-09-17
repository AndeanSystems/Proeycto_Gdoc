using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Gdoc.Entity.Models.Mapping
{
    public class DocumentoElectronicoOperacionMap : EntityTypeConfiguration<DocumentoElectronicoOperacion>
    {
        public DocumentoElectronicoOperacionMap()
        {
            // Primary Key
            this.HasKey(t => t.IDDoctoElectronicoOperacion);

            // Properties
            // Table & Column Mappings
            this.ToTable("DocumentoElectronicoOperacion");
            this.Property(t => t.IDDoctoElectronicoOperacion).HasColumnName("IDDoctoElectronicoOperacion");
            this.Property(t => t.IDOperacion).HasColumnName("IDOperacion");
            this.Property(t => t.Memo).HasColumnName("Memo");

            // Relationships
            this.HasRequired(t => t.Operacion)
                .WithMany(t => t.DocumentoElectronicoOperacions)
                .HasForeignKey(d => d.IDOperacion);

        }
    }
}
