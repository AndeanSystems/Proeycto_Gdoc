using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Gdoc.Entity.Models.Mapping
{
    public class GeneralMap : EntityTypeConfiguration<General>
    {
        public GeneralMap()
        {
            // Primary Key
            this.HasKey(t => t.IDCodigoParametro);

            // Properties
            this.Property(t => t.IDCodigoParametro)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("General");
            this.Property(t => t.IDCodigoParametro).HasColumnName("IDCodigoParametro");
            this.Property(t => t.IDEmpresa).HasColumnName("IDEmpresa");
            this.Property(t => t.IDUsuario).HasColumnName("IDUsuario");
            this.Property(t => t.PlazoDoctoElectronico).HasColumnName("PlazoDoctoElectronico");
            this.Property(t => t.ExtensionPlazoDoctoElectronico).HasColumnName("ExtensionPlazoDoctoElectronico");
            this.Property(t => t.AlertaDoctoElectronico).HasColumnName("AlertaDoctoElectronico");
            this.Property(t => t.PlazoMesaVirtual).HasColumnName("PlazoMesaVirtual");
            this.Property(t => t.ExtensionPlazoMesaVirtual).HasColumnName("ExtensionPlazoMesaVirtual");
            this.Property(t => t.AlertaMesaVirtual).HasColumnName("AlertaMesaVirtual");
            this.Property(t => t.AlertaMailLaboral).HasColumnName("AlertaMailLaboral");
            this.Property(t => t.AlertaMailPersonal).HasColumnName("AlertaMailPersonal");
            this.Property(t => t.HoraActualizaEstadoOperacion).HasColumnName("HoraActualizaEstadoOperacion");
            this.Property(t => t.HoraCierreLabores).HasColumnName("HoraCierreLabores");
            this.Property(t => t.PlazoExpiraFirma).HasColumnName("PlazoExpiraFirma");
            this.Property(t => t.RutaGdocImagenes).HasColumnName("RutaGdocImagenes");
            this.Property(t => t.RutaGdocPDF).HasColumnName("RutaGdocPDF");
            this.Property(t => t.RutaGdocAdjuntos).HasColumnName("RutaGdocAdjuntos");
            this.Property(t => t.RutaGdocExternos).HasColumnName("RutaGdocExternos");
            this.Property(t => t.Servidor).HasColumnName("Servidor");
            this.Property(t => t.TamanoMaxArchivo).HasColumnName("TamanoMaxArchivo");
            this.Property(t => t.TamanoMaxArchivos).HasColumnName("TamanoMaxArchivos");

            // Relationships
            this.HasRequired(t => t.Empresa)
                .WithMany(t => t.Generals)
                .HasForeignKey(d => d.IDEmpresa);
            this.HasOptional(t => t.Usuario)
                .WithMany(t => t.Generals)
                .HasForeignKey(d => d.IDUsuario);

        }
    }
}
