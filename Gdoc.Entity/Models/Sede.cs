using System;
using System.Collections.Generic;

namespace Gdoc.Entity.Models
{
    public partial class Sede
    {
        public Sede()
        {
            this.Personals = new List<Personal>();
        }

        public long IDSede { get; set; }
        public string CodigoSede { get; set; }
        public Nullable<int> IDEmpresa { get; set; }
        public string NombreSede { get; set; }
        public string CodigoUbigeo { get; set; }
        public string DireccionSede { get; set; }
        public string TelefonoSede { get; set; }
        public Nullable<int> EstadoSede { get; set; }
        public string UsuarioModifica { get; set; }
        public Nullable<System.DateTime> FechaModifica { get; set; }
        public virtual Empresa Empresa { get; set; }
        public virtual ICollection<Personal> Personals { get; set; }
        public virtual Ubigeo Ubigeo { get; set; }
    }
}
