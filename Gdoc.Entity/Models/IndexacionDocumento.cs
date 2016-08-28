using System;
using System.Collections.Generic;

namespace Gdoc.Entity.Models
{
    public partial class IndexacionDocumento
    {
        public long IDIndiceDocto { get; set; }
        public string DescripcionIndice { get; set; }
        public string EstadoIndice { get; set; }
        public Nullable<long> IDOperacion { get; set; }
        public string CodigoTipoOperacion { get; set; }
        public virtual Operacion Operacion { get; set; }
    }
}
