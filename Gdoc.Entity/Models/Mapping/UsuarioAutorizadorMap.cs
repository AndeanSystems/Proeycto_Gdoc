using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Gdoc.Entity.Models.Mapping
{
    public class UsuarioAutorizadorMap : EntityTypeConfiguration<UsuarioAutorizador>
    {
        public UsuarioAutorizadorMap()
        {
            // Primary Key
            this.HasKey(t => t.IDUsuarioAutorizador);

            // Properties
            this.Property(t => t.TipoOperacion)
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.RespuestaAutorizador)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.EstadoAutorizacion)
                .IsFixedLength()
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("UsuarioAutorizador");
            this.Property(t => t.IDUsuarioAutorizador).HasColumnName("IDUsuarioAutorizador");
            this.Property(t => t.IDUsuario).HasColumnName("IDUsuario");
            this.Property(t => t.CodigoOperacion).HasColumnName("CodigoOperacion");
            this.Property(t => t.TipoOperacion).HasColumnName("TipoOperacion");
            this.Property(t => t.RespuestaAutorizador).HasColumnName("RespuestaAutorizador");
            this.Property(t => t.FechaAutorizacion).HasColumnName("FechaAutorizacion");
            this.Property(t => t.ComentarioAutorizacion).HasColumnName("ComentarioAutorizacion");
            this.Property(t => t.EstadoAutorizacion).HasColumnName("EstadoAutorizacion");

            // Relationships
            this.HasOptional(t => t.Operacion)
                .WithMany(t => t.UsuarioAutorizadors)
                .HasForeignKey(d => d.CodigoOperacion);
            this.HasRequired(t => t.Usuario)
                .WithMany(t => t.UsuarioAutorizadors)
                .HasForeignKey(d => d.IDUsuario);

        }
    }
}
