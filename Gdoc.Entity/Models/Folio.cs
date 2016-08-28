using System;
using System.Collections.Generic;

namespace Gdoc.Entity.Models
{
    public partial class Folio
    {
        public long IDFolioOperacion { get; set; }
        public int IDEmpresa { get; set; }
        public string TipoDocto { get; set; }
        public int NumeroSerieOpereraion { get; set; }
        public long NumeroFolio { get; set; }
        public Nullable<long> LimiteFolio { get; set; }
        public Nullable<System.DateTime> FechaModifica { get; set; }
        public string UsuaModifica { get; set; }
        public virtual Empresa Empresa { get; set; }
    }
}
