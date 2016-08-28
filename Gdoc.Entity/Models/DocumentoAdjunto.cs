using System;
using System.Collections.Generic;

namespace Gdoc.Entity.Models
{
    public partial class DocumentoAdjunto
    {
        public long IDDoctoAdjunto { get; set; }
        public Nullable<long> CodigoOperacion { get; set; }
        public string TipoOperacion { get; set; }
        public Nullable<long> CodigoDoctoAdjunto { get; set; }
        public string TipoDoctoAdjunto { get; set; }
        public Nullable<long> CodigoComentarioMesa { get; set; }
        public Nullable<int> EstadoDoctoAdjunto { get; set; }
        public virtual Operacion Operacion { get; set; }
    }
}
