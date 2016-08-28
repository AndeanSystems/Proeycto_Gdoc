using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Gdoc.Entity.Models.Mapping
{
    public class DocumentoDigitalOperacionMap : EntityTypeConfiguration<DocumentoDigitalOperacion>
    {
        public DocumentoDigitalOperacionMap()
        {
            // Primary Key
            this.HasKey(t => t.IDDoctoDigitalOperacion);

            // Properties
            this.Property(t => t.IDDoctoDigitalOperacion)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.DerivarDocto)
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.NombreOriginal)
                .HasMaxLength(100);

            this.Property(t => t.RutaFisica)
                .HasMaxLength(300);

            this.Property(t => t.DoctoExterno)
                .HasMaxLength(10);

            this.Property(t => t.NombreFisico)
                .HasMaxLength(300);

            this.Property(t => t.Comentario)
                .HasMaxLength(300);

            // Table & Column Mappings
            this.ToTable("DocumentoDigitalOperacion");
            this.Property(t => t.IDDoctoDigitalOperacion).HasColumnName("IDDoctoDigitalOperacion");
            this.Property(t => t.CodigoOperacion).HasColumnName("CodigoOperacion");
            this.Property(t => t.DerivarDocto).HasColumnName("DerivarDocto");
            this.Property(t => t.NombreOriginal).HasColumnName("NombreOriginal");
            this.Property(t => t.RutaFisica).HasColumnName("RutaFisica");
            this.Property(t => t.TamanoDocto).HasColumnName("TamanoDocto");
            this.Property(t => t.DoctoExterno).HasColumnName("DoctoExterno");
            this.Property(t => t.NombreFisico).HasColumnName("NombreFisico");
            this.Property(t => t.Comentario).HasColumnName("Comentario");

            // Relationships
            this.HasRequired(t => t.Operacion)
                .WithMany(t => t.DocumentoDigitalOperacions)
                .HasForeignKey(d => d.CodigoOperacion);

        }
    }
}
