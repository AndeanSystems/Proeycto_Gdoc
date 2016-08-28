using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Gdoc.Entity.Models.Mapping
{
    public class ConceptoMap : EntityTypeConfiguration<Concepto>
    {
        public ConceptoMap()
        {
            // Primary Key
            this.HasKey(t => new { t.IDEmpresa, t.TipoConcepto, t.CodiConcepto });

            // Properties
            this.Property(t => t.IDEmpresa)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.TipoConcepto)
                .IsRequired()
                .HasMaxLength(3);

            this.Property(t => t.CodiConcepto)
                .IsRequired()
                .HasMaxLength(5);

            this.Property(t => t.DescripcionConcepto)
                .HasMaxLength(250);

            this.Property(t => t.DescripcionCorta)
                .HasMaxLength(15);

            this.Property(t => t.TextoUno)
                .HasMaxLength(50);

            this.Property(t => t.TextoDos)
                .HasMaxLength(50);

            this.Property(t => t.UsuarioModifica)
                .HasMaxLength(15);

            // Table & Column Mappings
            this.ToTable("Concepto");
            this.Property(t => t.IDEmpresa).HasColumnName("IDEmpresa");
            this.Property(t => t.TipoConcepto).HasColumnName("TipoConcepto");
            this.Property(t => t.CodiConcepto).HasColumnName("CodiConcepto");
            this.Property(t => t.DescripcionConcepto).HasColumnName("DescripcionConcepto");
            this.Property(t => t.DescripcionCorta).HasColumnName("DescripcionCorta");
            this.Property(t => t.ValorUno).HasColumnName("ValorUno");
            this.Property(t => t.ValorDos).HasColumnName("ValorDos");
            this.Property(t => t.TextoUno).HasColumnName("TextoUno");
            this.Property(t => t.TextoDos).HasColumnName("TextoDos");
            this.Property(t => t.EstadoConcepto).HasColumnName("EstadoConcepto");
            this.Property(t => t.EditarRegistro).HasColumnName("EditarRegistro");
            this.Property(t => t.UsuarioModifica).HasColumnName("UsuarioModifica");
            this.Property(t => t.FechaModifica).HasColumnName("FechaModifica");

            // Relationships
            this.HasRequired(t => t.Empresa)
                .WithMany(t => t.Conceptoes)
                .HasForeignKey(d => d.IDEmpresa);

        }
    }
}
