using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Gdoc.Entity.Models.Mapping
{
    public class MensajeAlertaMap : EntityTypeConfiguration<MensajeAlerta>
    {
        public MensajeAlertaMap()
        {
            // Primary Key
            this.HasKey(t => t.IDMensajeAlerta);

            // Properties
            this.Property(t => t.CodigoEvento)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("MensajeAlerta");
            this.Property(t => t.IDMensajeAlerta).HasColumnName("IDMensajeAlerta");
            this.Property(t => t.IDOperacion).HasColumnName("IDOperacion");
            this.Property(t => t.FechaAlerta).HasColumnName("FechaAlerta");
            this.Property(t => t.TipoAlerta).HasColumnName("TipoAlerta");
            this.Property(t => t.CodigoEvento).HasColumnName("CodigoEvento");
            this.Property(t => t.EstadoMensajeAlerta).HasColumnName("EstadoMensajeAlerta");
            this.Property(t => t.IDUsuario).HasColumnName("IDUsuario");
            this.Property(t => t.Remitente).HasColumnName("Remitente");

            // Relationships
            this.HasOptional(t => t.Operacion)
                .WithMany(t => t.MensajeAlertas)
                .HasForeignKey(d => d.IDOperacion);
            this.HasOptional(t => t.Usuario)
                .WithMany(t => t.MensajeAlertas)
                .HasForeignKey(d => d.IDUsuario);

        }
    }
}
