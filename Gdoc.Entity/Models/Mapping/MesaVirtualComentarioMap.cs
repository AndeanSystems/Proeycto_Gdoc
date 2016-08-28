using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Gdoc.Entity.Models.Mapping
{
    public class MesaVirtualComentarioMap : EntityTypeConfiguration<MesaVirtualComentario>
    {
        public MesaVirtualComentarioMap()
        {
            // Primary Key
            this.HasKey(t => t.IDComentarioMesaVirtual);

            // Properties
            // Table & Column Mappings
            this.ToTable("MesaVirtualComentario");
            this.Property(t => t.IDComentarioMesaVirtual).HasColumnName("IDComentarioMesaVirtual");
            this.Property(t => t.ComentarioMesaVirtual).HasColumnName("ComentarioMesaVirtual");
            this.Property(t => t.FechaPublicacion).HasColumnName("FechaPublicacion");
            this.Property(t => t.EstadoComentario).HasColumnName("EstadoComentario");
            this.Property(t => t.IDOperacion).HasColumnName("IDOperacion");
            this.Property(t => t.IDUsuario).HasColumnName("IDUsuario");

            // Relationships
            this.HasOptional(t => t.Operacion)
                .WithMany(t => t.MesaVirtualComentarios)
                .HasForeignKey(d => d.IDOperacion);
            this.HasOptional(t => t.Usuario)
                .WithMany(t => t.MesaVirtualComentarios)
                .HasForeignKey(d => d.IDUsuario);

        }
    }
}
