using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Gdoc.Entity.Models.Mapping
{
    public class UbigeoMap : EntityTypeConfiguration<Ubigeo>
    {
        public UbigeoMap()
        {
            // Primary Key
            this.HasKey(t => t.CodigoUbigeo);

            // Properties
            this.Property(t => t.CodigoUbigeo)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(6);

            this.Property(t => t.DescripcionUbicacion)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Ubigeo");
            this.Property(t => t.CodigoUbigeo).HasColumnName("CodigoUbigeo");
            this.Property(t => t.CodigoPais).HasColumnName("CodigoPais");
            this.Property(t => t.CodigoDepartamento).HasColumnName("CodigoDepartamento");
            this.Property(t => t.CodigoProvincia).HasColumnName("CodigoProvincia");
            this.Property(t => t.CodigoDistrito).HasColumnName("CodigoDistrito");
            this.Property(t => t.DescripcionUbicacion).HasColumnName("DescripcionUbicacion");
            this.Property(t => t.EstadoUbigeo).HasColumnName("EstadoUbigeo");
        }
    }
}
