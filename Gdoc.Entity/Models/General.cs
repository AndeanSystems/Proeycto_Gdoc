using System;
using System.Collections.Generic;

namespace Gdoc.Entity.Models
{
    public partial class General
    {
        public long IDCodigoParametro { get; set; }
        public int IDEmpresa { get; set; }
        public Nullable<int> PlazoDoctoElectronico { get; set; }
        public Nullable<int> ExtensionPlazoDoctoElectronico { get; set; }
        public Nullable<int> AlertaDoctoElectronico { get; set; }
        public Nullable<int> PlazoMesaVirtual { get; set; }
        public Nullable<int> ExtensionPlazoMesaVirtual { get; set; }
        public Nullable<int> AlertaMesaVirtual { get; set; }
        public Nullable<int> AlertaMailLaboral { get; set; }
        public Nullable<int> AlertaMailPersonal { get; set; }
        public Nullable<System.DateTime> HoraActualizaEstadoOperacion { get; set; }
        public Nullable<System.DateTime> HoraCierreLabores { get; set; }
        public virtual Empresa Empresa { get; set; }
    }
}
