using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Gdoc.Entity.Models.Mapping
{
    public class OperacionMap : EntityTypeConfiguration<Operacion>
    {
        public OperacionMap()
        {
            // Primary Key
            this.HasKey(t => t.IDOperacion);

            // Properties
            this.Property(t => t.TipoOperacion)
                .HasMaxLength(5);

            this.Property(t => t.NumeroOperacion)
                .HasMaxLength(20);

            this.Property(t => t.TituloOperacion)
                .HasMaxLength(100);

            this.Property(t => t.AccesoOperacion)
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.PrioridadOperacion)
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.DocumentoAdjunto)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.TipoComunicacion)
                .IsFixedLength()
                .HasMaxLength(10);

            this.Property(t => t.NotificacionOperacion)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.TipoDocumento)
                .HasMaxLength(5);

            // Table & Column Mappings
            this.ToTable("Operacion");
            this.Property(t => t.IDOperacion).HasColumnName("IDOperacion");
            this.Property(t => t.IDEmpresa).HasColumnName("IDEmpresa");
            this.Property(t => t.TipoOperacion).HasColumnName("TipoOperacion");
            this.Property(t => t.FechaEmision).HasColumnName("FechaEmision");
            this.Property(t => t.NumeroOperacion).HasColumnName("NumeroOperacion");
            this.Property(t => t.TituloOperacion).HasColumnName("TituloOperacion");
            this.Property(t => t.AccesoOperacion).HasColumnName("AccesoOperacion");
            this.Property(t => t.EstadoOperacion).HasColumnName("EstadoOperacion");
            this.Property(t => t.DescripcionOperacion).HasColumnName("DescripcionOperacion");
            this.Property(t => t.PrioridadOperacion).HasColumnName("PrioridadOperacion");
            this.Property(t => t.FechaCierre).HasColumnName("FechaCierre");
            this.Property(t => t.FechaRegistro).HasColumnName("FechaRegistro");
            this.Property(t => t.FechaEnvio).HasColumnName("FechaEnvio");
            this.Property(t => t.FechaVigente).HasColumnName("FechaVigente");
            this.Property(t => t.DocumentoAdjunto).HasColumnName("DocumentoAdjunto");
            this.Property(t => t.TipoComunicacion).HasColumnName("TipoComunicacion");
            this.Property(t => t.NotificacionOperacion).HasColumnName("NotificacionOperacion");
            this.Property(t => t.TipoDocumento).HasColumnName("TipoDocumento");
            this.Property(t => t.NombreFinal).HasColumnName("NombreFinal");
        }
    }
}
