using System;
using System.Collections.Generic;

namespace Gdoc.Entity.Models
{
    public partial class Empresa
    {
        public Empresa()
        {
            this.CentroCostoes = new List<CentroCosto>();
            this.Conceptoes = new List<Concepto>();
            this.Folios = new List<Folio>();
            this.Generals = new List<General>();
            this.Personals = new List<Personal>();
            this.Sedes = new List<Sede>();
        }

        public int IDEmpresa { get; set; }
        public long RucEmpresa { get; set; }
        public string RazonSocial { get; set; }
        public string DireccionEmpresa { get; set; }
        public string TelefonoEmpresa { get; set; }
        public string CodigoUbigeo { get; set; }
        public Nullable<System.DateTime> FechaRegistro { get; set; }
        public string UsuarioRegistro { get; set; }
        public int EstadoEmpresa { get; set; }
        public virtual ICollection<CentroCosto> CentroCostoes { get; set; }
        public virtual ICollection<Concepto> Conceptoes { get; set; }
        public virtual ICollection<Folio> Folios { get; set; }
        public virtual ICollection<General> Generals { get; set; }
        public virtual ICollection<Personal> Personals { get; set; }
        public virtual ICollection<Sede> Sedes { get; set; }
    }
}
