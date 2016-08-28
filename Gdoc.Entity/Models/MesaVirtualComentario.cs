using System;
using System.Collections.Generic;

namespace Gdoc.Entity.Models
{
    public partial class MesaVirtualComentario
    {
        public long IDComentarioMesaVirtual { get; set; }
        public string ComentarioMesaVirtual { get; set; }
        public Nullable<System.DateTime> FechaPublicacion { get; set; }
        public int EstadoComentario { get; set; }
        public Nullable<long> IDOperacion { get; set; }
        public Nullable<long> IDUsuario { get; set; }
        public virtual Operacion Operacion { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
