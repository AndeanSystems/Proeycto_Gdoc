using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Gdoc.Entity.Models.Mapping
{
    public class FolioMap : EntityTypeConfiguration<Folio>
    {
        public FolioMap()
        {
            // Primary Key
            this.HasKey(t => t.IDFolioOperacion);

            // Properties
            this.Property(t => t.TipoDocto)
                .IsRequired()
                .HasMaxLength(5);

            this.Property(t => t.UsuaModifica)
                .IsFixedLength()
                .HasMaxLength(15);

            // Table & Column Mappings
            this.ToTable("Folio");
            this.Property(t => t.IDFolioOperacion).HasColumnName("IDFolioOperacion");
            this.Property(t => t.IDEmpresa).HasColumnName("IDEmpresa");
            this.Property(t => t.TipoDocto).HasColumnName("TipoDocto");
            this.Property(t => t.NumeroSerieOpereraion).HasColumnName("NumeroSerieOpereraion");
            this.Property(t => t.NumeroFolio).HasColumnName("NumeroFolio");
            this.Property(t => t.LimiteFolio).HasColumnName("LimiteFolio");
            this.Property(t => t.FechaModifica).HasColumnName("FechaModifica");
            this.Property(t => t.UsuaModifica).HasColumnName("UsuaModifica");

            // Relationships
            this.HasRequired(t => t.Empresa)
                .WithMany(t => t.Folios)
                .HasForeignKey(d => d.IDEmpresa);

        }
    }
}
