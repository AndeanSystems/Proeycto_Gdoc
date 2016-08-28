using System;
using System.Collections.Generic;

namespace Gdoc.Entity.Models
{
    public partial class CentroCosto
    {
        public int IDCentroCosto { get; set; }
        public Nullable<int> IDEmpresa { get; set; }
        public string CodigoCentroResponsable { get; set; }
        public string TipoCentroResponsable { get; set; }
        public Nullable<long> CodigoCentroCosto { get; set; }
        public string DescripcionCentroCosto { get; set; }
        public Nullable<int> EstadoCentroCosto { get; set; }
        public Nullable<System.DateTime> FechaRegistro { get; set; }
        public string UsuarioRegistro { get; set; }
        public virtual Empresa Empresa { get; set; }
    }
}
