using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Gdoc.Entity.Models.Mapping
{
    public class LogOperacionMap : EntityTypeConfiguration<LogOperacion>
    {
        public LogOperacionMap()
        {
            // Primary Key
            this.HasKey(t => t.IDLogOperacion);

            // Properties
            this.Property(t => t.CodigoTipoOperacion)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.CodigoEvento)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(5);

            this.Property(t => t.CodigoConexion)
                .IsFixedLength()
                .HasMaxLength(15);

            this.Property(t => t.TerminalConexion)
                .HasMaxLength(15);

            // Table & Column Mappings
            this.ToTable("LogOperacion");
            this.Property(t => t.IDLogOperacion).HasColumnName("IDLogOperacion");
            this.Property(t => t.FechaEvento).HasColumnName("FechaEvento");
            this.Property(t => t.CodigoTipoOperacion).HasColumnName("CodigoTipoOperacion");
            this.Property(t => t.CodigoOperacion).HasColumnName("CodigoOperacion");
            this.Property(t => t.CodigoEvento).HasColumnName("CodigoEvento");
            this.Property(t => t.IDUsuario).HasColumnName("IDUsuario");
            this.Property(t => t.CodigoConexion).HasColumnName("CodigoConexion");
            this.Property(t => t.TerminalConexion).HasColumnName("TerminalConexion");

            // Relationships
            this.HasRequired(t => t.Operacion)
                .WithMany(t => t.LogOperacions)
                .HasForeignKey(d => d.CodigoOperacion);
            this.HasRequired(t => t.Usuario)
                .WithMany(t => t.LogOperacions)
                .HasForeignKey(d => d.IDUsuario);

        }
    }
}
