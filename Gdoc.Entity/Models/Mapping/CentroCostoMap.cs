using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Gdoc.Entity.Models.Mapping
{
    public class CentroCostoMap : EntityTypeConfiguration<CentroCosto>
    {
        public CentroCostoMap()
        {
            // Primary Key
            this.HasKey(t => t.IDCentroCosto);

            // Properties
            this.Property(t => t.CodigoCentroResponsable)
                .HasMaxLength(10);

            this.Property(t => t.TipoCentroResponsable)
                .HasMaxLength(5);

            this.Property(t => t.DescripcionCentroCosto)
                .HasMaxLength(50);

            this.Property(t => t.UsuarioRegistro)
                .IsFixedLength()
                .HasMaxLength(15);

            // Table & Column Mappings
            this.ToTable("CentroCosto");
            this.Property(t => t.IDCentroCosto).HasColumnName("IDCentroCosto");
            this.Property(t => t.IDEmpresa).HasColumnName("IDEmpresa");
            this.Property(t => t.CodigoCentroResponsable).HasColumnName("CodigoCentroResponsable");
            this.Property(t => t.TipoCentroResponsable).HasColumnName("TipoCentroResponsable");
            this.Property(t => t.CodigoCentroCosto).HasColumnName("CodigoCentroCosto");
            this.Property(t => t.DescripcionCentroCosto).HasColumnName("DescripcionCentroCosto");
            this.Property(t => t.EstadoCentroCosto).HasColumnName("EstadoCentroCosto");
            this.Property(t => t.FechaRegistro).HasColumnName("FechaRegistro");
            this.Property(t => t.UsuarioRegistro).HasColumnName("UsuarioRegistro");

            // Relationships
            this.HasOptional(t => t.Empresa)
                .WithMany(t => t.CentroCostoes)
                .HasForeignKey(d => d.IDEmpresa);

        }
    }
}
