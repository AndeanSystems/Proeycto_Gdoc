using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Gdoc.Entity.Models.Mapping
{
    public class ModuloPaginaUrlMap : EntityTypeConfiguration<ModuloPaginaUrl>
    {
        public ModuloPaginaUrlMap()
        {
            // Primary Key
            this.HasKey(t => t.IDModuloPagina);

            // Properties
            this.Property(t => t.NombrePagina)
                .HasMaxLength(200);

            this.Property(t => t.ComentarioPagina)
                .HasMaxLength(50);

            this.Property(t => t.DireccionFisicaPagina)
                .HasMaxLength(200);

            this.Property(t => t.ModuloSistema)
                .HasMaxLength(5);

            // Table & Column Mappings
            this.ToTable("ModuloPaginaUrl");
            this.Property(t => t.IDModuloPagina).HasColumnName("IDModuloPagina");
            this.Property(t => t.NombrePagina).HasColumnName("NombrePagina");
            this.Property(t => t.ComentarioPagina).HasColumnName("ComentarioPagina");
            this.Property(t => t.DireccionFisicaPagina).HasColumnName("DireccionFisicaPagina");
            this.Property(t => t.EstadoPagina).HasColumnName("EstadoPagina");
            this.Property(t => t.CodigoPaginaPadre).HasColumnName("CodigoPaginaPadre");
            this.Property(t => t.ModuloSistema).HasColumnName("ModuloSistema");
        }
    }
}
