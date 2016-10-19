using System;
using System.Collections.Generic;

namespace Gdoc.Entity.Models
{
    public partial class MensajeAlerta
    {
        public long IDMensajeAlerta { get; set; }
        public Nullable<long> IDOperacion { get; set; }
        public Nullable<System.DateTime> FechaAlerta { get; set; }
        public Nullable<int> TipoAlerta { get; set; }
        public string CodigoEvento { get; set; }
        public Nullable<int> EstadoMensajeAlerta { get; set; }
        public Nullable<long> IDUsuario { get; set; }
        public string Remitente { get; set; }
        public Nullable<long> IDComentarioMesaVirtual { get; set; }
        public virtual Operacion Operacion { get; set; }
        public virtual MesaVirtualComentario MesaVirtualComentario { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
