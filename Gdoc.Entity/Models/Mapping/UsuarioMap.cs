using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Gdoc.Entity.Models.Mapping
{
    public class UsuarioMap : EntityTypeConfiguration<Usuario>
    {
        public UsuarioMap()
        {
            // Primary Key
            this.HasKey(t => t.IDUsuario);

            // Properties
            this.Property(t => t.NombreUsuario)
                .HasMaxLength(15);

            this.Property(t => t.ClaveUsuario)
                .HasMaxLength(20);

            this.Property(t => t.FirmaElectronica)
                .HasMaxLength(20);

            this.Property(t => t.TerminalUsuario)
                .IsFixedLength()
                .HasMaxLength(15);

            this.Property(t => t.UsuarioRegistro)
                .IsFixedLength()
                .HasMaxLength(15);

            this.Property(t => t.CodigoConexion)
                .IsFixedLength()
                .HasMaxLength(20);

            this.Property(t => t.CodigoRol)
                .HasMaxLength(5);

            this.Property(t => t.CodigoTipoUsua)
                .HasMaxLength(5);

            this.Property(t => t.ClaseUsuario)
                .HasMaxLength(5);

            this.Property(t => t.ExpiraClave)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.ExpiraFirma)
                .IsFixedLength()
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("Usuario");
            this.Property(t => t.IDUsuario).HasColumnName("IDUsuario");
            this.Property(t => t.NombreUsuario).HasColumnName("NombreUsuario");
            this.Property(t => t.ClaveUsuario).HasColumnName("ClaveUsuario");
            this.Property(t => t.FirmaElectronica).HasColumnName("FirmaElectronica");
            this.Property(t => t.EstadoUsuario).HasColumnName("EstadoUsuario");
            this.Property(t => t.FechaRegistro).HasColumnName("FechaRegistro");
            this.Property(t => t.FechaUltimoAcceso).HasColumnName("FechaUltimoAcceso");
            this.Property(t => t.FechaModifica).HasColumnName("FechaModifica");
            this.Property(t => t.IntentoErradoClave).HasColumnName("IntentoErradoClave");
            this.Property(t => t.IntentoerradoFirma).HasColumnName("IntentoerradoFirma");
            this.Property(t => t.TerminalUsuario).HasColumnName("TerminalUsuario");
            this.Property(t => t.UsuarioRegistro).HasColumnName("UsuarioRegistro");
            this.Property(t => t.CodigoConexion).HasColumnName("CodigoConexion");
            this.Property(t => t.IDPersonal).HasColumnName("IDPersonal");
            this.Property(t => t.CodigoRol).HasColumnName("CodigoRol");
            this.Property(t => t.CodigoTipoUsua).HasColumnName("CodigoTipoUsua");
            this.Property(t => t.ClaseUsuario).HasColumnName("ClaseUsuario");
            this.Property(t => t.ExpiraClave).HasColumnName("ExpiraClave");
            this.Property(t => t.ExpiraFirma).HasColumnName("ExpiraFirma");
            this.Property(t => t.FechaExpiraClave).HasColumnName("FechaExpiraClave");
            this.Property(t => t.FechaExpiraFirma).HasColumnName("FechaExpiraFirma");

            // Relationships
            this.HasOptional(t => t.Personal)
                .WithMany(t => t.Usuarios)
                .HasForeignKey(d => d.IDPersonal);

        }
    }
}
