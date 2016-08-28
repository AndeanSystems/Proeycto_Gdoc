using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Gdoc.Entity.Models.Mapping
{
    public class SedeMap : EntityTypeConfiguration<Sede>
    {
        public SedeMap()
        {
            // Primary Key
            this.HasKey(t => t.IDSede);

            // Properties
            this.Property(t => t.CodigoSede)
                .HasMaxLength(6);

            this.Property(t => t.NombreSede)
                .HasMaxLength(250);

            this.Property(t => t.CodigoUbigeo)
                .IsFixedLength()
                .HasMaxLength(6);

            this.Property(t => t.DireccionSede)
                .HasMaxLength(250);

            this.Property(t => t.TelefonoSede)
                .HasMaxLength(30);

            this.Property(t => t.UsuarioModifica)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("Sede");
            this.Property(t => t.IDSede).HasColumnName("IDSede");
            this.Property(t => t.CodigoSede).HasColumnName("CodigoSede");
            this.Property(t => t.IDEmpresa).HasColumnName("IDEmpresa");
            this.Property(t => t.NombreSede).HasColumnName("NombreSede");
            this.Property(t => t.CodigoUbigeo).HasColumnName("CodigoUbigeo");
            this.Property(t => t.DireccionSede).HasColumnName("DireccionSede");
            this.Property(t => t.TelefonoSede).HasColumnName("TelefonoSede");
            this.Property(t => t.EstadoSede).HasColumnName("EstadoSede");
            this.Property(t => t.UsuarioModifica).HasColumnName("UsuarioModifica");
            this.Property(t => t.FechaModifica).HasColumnName("FechaModifica");

            // Relationships
            this.HasOptional(t => t.Empresa)
                .WithMany(t => t.Sedes)
                .HasForeignKey(d => d.IDEmpresa);
            this.HasOptional(t => t.Ubigeo)
                .WithMany(t => t.Sedes)
                .HasForeignKey(d => d.CodigoUbigeo);

        }
    }
}
