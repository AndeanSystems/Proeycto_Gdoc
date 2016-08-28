using System;
using System.Collections.Generic;

namespace Gdoc.Entity.Models
{
    public partial class Personal
    {
        public Personal()
        {
            this.Usuarios = new List<Usuario>();
        }

        public long IDPersonal { get; set; }
        public Nullable<int> IDEmpresa { get; set; }
        public Nullable<long> IDSede { get; set; }
        public string CodigoPersonal { get; set; }
        public string NombrePers { get; set; }
        public string ApellidoPersonal { get; set; }
        public string SexoPersonal { get; set; }
        public string EmailPersonal { get; set; }
        public string EmailTrabrajo { get; set; }
        public Nullable<System.DateTime> FechaNacimiento { get; set; }
        public string TelefonoPersonal { get; set; }
        public string AnexoPersonal { get; set; }
        public string EstadoPersonal { get; set; }
        public string CodigoArea { get; set; }
        public string CodigoCargo { get; set; }
        public string ClasePersonal { get; set; }
        public string NumeroDNI { get; set; }
        public string DireccionPersonal { get; set; }
        public string CodigoUbigeo { get; set; }
        public string CelularPersonalUno { get; set; }
        public string CelularPersonalDos { get; set; }
        public virtual Empresa Empresa { get; set; }
        public virtual Sede Sede { get; set; }
        public virtual Ubigeo Ubigeo { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
