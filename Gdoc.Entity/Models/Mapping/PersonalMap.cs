using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Gdoc.Entity.Models.Mapping
{
    public class PersonalMap : EntityTypeConfiguration<Personal>
    {
        public PersonalMap()
        {
            // Primary Key
            this.HasKey(t => t.IDPersonal);

            // Properties
            this.Property(t => t.CodigoPersonal)
                .HasMaxLength(8);

            this.Property(t => t.NombrePers)
                .HasMaxLength(80);

            this.Property(t => t.ApellidoPersonal)
                .HasMaxLength(80);

            this.Property(t => t.SexoPersonal)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.EmailPersonal)
                .HasMaxLength(50);

            this.Property(t => t.EmailTrabrajo)
                .HasMaxLength(50);

            this.Property(t => t.TelefonoPersonal)
                .IsFixedLength()
                .HasMaxLength(15);

            this.Property(t => t.AnexoPersonal)
                .IsFixedLength()
                .HasMaxLength(10);

            this.Property(t => t.EstadoPersonal)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.CodigoArea)
                .HasMaxLength(5);

            this.Property(t => t.CodigoCargo)
                .HasMaxLength(5);

            this.Property(t => t.ClasePersonal)
                .HasMaxLength(5);

            this.Property(t => t.NumeroDNI)
                .HasMaxLength(8);

            this.Property(t => t.DireccionPersonal)
                .HasMaxLength(100);

            this.Property(t => t.CodigoUbigeo)
                .IsFixedLength()
                .HasMaxLength(6);

            this.Property(t => t.CelularPersonalUno)
                .IsFixedLength()
                .HasMaxLength(15);

            this.Property(t => t.CelularPersonalDos)
                .IsFixedLength()
                .HasMaxLength(15);

            // Table & Column Mappings
            this.ToTable("Personal");
            this.Property(t => t.IDPersonal).HasColumnName("IDPersonal");
            this.Property(t => t.IDEmpresa).HasColumnName("IDEmpresa");
            this.Property(t => t.IDSede).HasColumnName("IDSede");
            this.Property(t => t.CodigoPersonal).HasColumnName("CodigoPersonal");
            this.Property(t => t.NombrePers).HasColumnName("NombrePers");
            this.Property(t => t.ApellidoPersonal).HasColumnName("ApellidoPersonal");
            this.Property(t => t.SexoPersonal).HasColumnName("SexoPersonal");
            this.Property(t => t.EmailPersonal).HasColumnName("EmailPersonal");
            this.Property(t => t.EmailTrabrajo).HasColumnName("EmailTrabrajo");
            this.Property(t => t.FechaNacimiento).HasColumnName("FechaNacimiento");
            this.Property(t => t.TelefonoPersonal).HasColumnName("TelefonoPersonal");
            this.Property(t => t.AnexoPersonal).HasColumnName("AnexoPersonal");
            this.Property(t => t.EstadoPersonal).HasColumnName("EstadoPersonal");
            this.Property(t => t.CodigoArea).HasColumnName("CodigoArea");
            this.Property(t => t.CodigoCargo).HasColumnName("CodigoCargo");
            this.Property(t => t.ClasePersonal).HasColumnName("ClasePersonal");
            this.Property(t => t.NumeroDNI).HasColumnName("NumeroDNI");
            this.Property(t => t.DireccionPersonal).HasColumnName("DireccionPersonal");
            this.Property(t => t.CodigoUbigeo).HasColumnName("CodigoUbigeo");
            this.Property(t => t.CelularPersonalUno).HasColumnName("CelularPersonalUno");
            this.Property(t => t.CelularPersonalDos).HasColumnName("CelularPersonalDos");

            // Relationships
            this.HasOptional(t => t.Empresa)
                .WithMany(t => t.Personals)
                .HasForeignKey(d => d.IDEmpresa);
            this.HasOptional(t => t.Sede)
                .WithMany(t => t.Personals)
                .HasForeignKey(d => d.IDSede);
            this.HasOptional(t => t.Ubigeo)
                .WithMany(t => t.Personals)
                .HasForeignKey(d => d.CodigoUbigeo);

        }
    }
}
