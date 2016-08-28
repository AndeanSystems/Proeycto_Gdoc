using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Gdoc.Entity.Models.Mapping
{
    public class EmpresaMap : EntityTypeConfiguration<Empresa>
    {
        public EmpresaMap()
        {
            // Primary Key
            this.HasKey(t => t.IDEmpresa);

            // Properties
            this.Property(t => t.IDEmpresa)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.RazonSocial)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.DireccionEmpresa)
                .HasMaxLength(150);

            this.Property(t => t.TelefonoEmpresa)
                .HasMaxLength(20);

            this.Property(t => t.CodigoUbigeo)
                .IsFixedLength()
                .HasMaxLength(6);

            this.Property(t => t.UsuarioRegistro)
                .IsFixedLength()
                .HasMaxLength(15);

            // Table & Column Mappings
            this.ToTable("Empresa");
            this.Property(t => t.IDEmpresa).HasColumnName("IDEmpresa");
            this.Property(t => t.RucEmpresa).HasColumnName("RucEmpresa");
            this.Property(t => t.RazonSocial).HasColumnName("RazonSocial");
            this.Property(t => t.DireccionEmpresa).HasColumnName("DireccionEmpresa");
            this.Property(t => t.TelefonoEmpresa).HasColumnName("TelefonoEmpresa");
            this.Property(t => t.CodigoUbigeo).HasColumnName("CodigoUbigeo");
            this.Property(t => t.FechaRegistro).HasColumnName("FechaRegistro");
            this.Property(t => t.UsuarioRegistro).HasColumnName("UsuarioRegistro");
            this.Property(t => t.EstadoEmpresa).HasColumnName("EstadoEmpresa");
        }
    }
}
