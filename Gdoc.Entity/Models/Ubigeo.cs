using System;
using System.Collections.Generic;

namespace Gdoc.Entity.Models
{
    public partial class Ubigeo
    {
        public Ubigeo()
        {
            this.Personals = new List<Personal>();
            this.Sedes = new List<Sede>();
        }

        public string CodigoUbigeo { get; set; }
        public long CodigoPais { get; set; }
        public Nullable<int> CodigoDepartamento { get; set; }
        public Nullable<int> CodigoProvincia { get; set; }
        public Nullable<int> CodigoDistrito { get; set; }
        public string DescripcionUbicacion { get; set; }
        public Nullable<int> EstadoUbigeo { get; set; }
        public virtual ICollection<Personal> Personals { get; set; }
        public virtual ICollection<Sede> Sedes { get; set; }
    }
}
