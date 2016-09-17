using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Gdoc.Entity.Models.Mapping
{
    public class UsuarioParticipanteMap : EntityTypeConfiguration<UsuarioParticipante>
    {
        public UsuarioParticipanteMap()
        {
            // Primary Key
            this.HasKey(t => t.IDUsuarioParticipante);

            // Properties
            this.Property(t => t.TipoOperacion)
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.TipoParticipante)
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.AprobarOperacion)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.EnviarNotificiacion)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.ConfirmacionLectura)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.PostergacionLectura)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.ReenvioOperacion)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.EnvioOperacion)
                .IsFixedLength()
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("UsuarioParticipante");
            this.Property(t => t.IDUsuarioParticipante).HasColumnName("IDUsuarioParticipante");
            this.Property(t => t.IDUsuario).HasColumnName("IDUsuario");
            this.Property(t => t.IDOperacion).HasColumnName("IDOperacion");
            this.Property(t => t.TipoOperacion).HasColumnName("TipoOperacion");
            this.Property(t => t.TipoParticipante).HasColumnName("TipoParticipante");
            this.Property(t => t.AprobarOperacion).HasColumnName("AprobarOperacion");
            this.Property(t => t.EnviarNotificiacion).HasColumnName("EnviarNotificiacion");
            this.Property(t => t.FechaNotificacion).HasColumnName("FechaNotificacion");
            this.Property(t => t.EstadoUsuarioParticipante).HasColumnName("EstadoUsuarioParticipante");
            this.Property(t => t.ConfirmacionLectura).HasColumnName("ConfirmacionLectura");
            this.Property(t => t.PostergacionLectura).HasColumnName("PostergacionLectura");
            this.Property(t => t.ReenvioOperacion).HasColumnName("ReenvioOperacion");
            this.Property(t => t.EnvioOperacion).HasColumnName("EnvioOperacion");

            // Relationships
            this.HasRequired(t => t.Operacion)
                .WithMany(t => t.UsuarioParticipantes)
                .HasForeignKey(d => d.IDOperacion);
            this.HasRequired(t => t.Usuario)
                .WithMany(t => t.UsuarioParticipantes)
                .HasForeignKey(d => d.IDUsuario);

        }
    }
}
