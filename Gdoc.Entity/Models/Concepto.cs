using System;
using System.Collections.Generic;

namespace Gdoc.Entity.Models
{
    public partial class Concepto
    {
        public int IDEmpresa { get; set; }
        public string TipoConcepto { get; set; }
        public string CodiConcepto { get; set; }
        public string DescripcionConcepto { get; set; }
        public string DescripcionCorta { get; set; }
        public Nullable<long> ValorUno { get; set; }
        public Nullable<long> ValorDos { get; set; }
        public string TextoUno { get; set; }
        public string TextoDos { get; set; }
        public Nullable<int> EstadoConcepto { get; set; }
        public Nullable<int> EditarRegistro { get; set; }
        public string UsuarioModifica { get; set; }
        public Nullable<System.DateTime> FechaModifica { get; set; }
        public virtual Empresa Empresa { get; set; }
    }
}
