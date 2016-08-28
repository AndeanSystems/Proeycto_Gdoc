using System;
using System.Collections.Generic;

namespace Gdoc.Entity.Models
{
    public partial class UsuarioAutorizador
    {
        public long IDUsuarioAutorizador { get; set; }
        public long IDUsuario { get; set; }
        public Nullable<long> CodigoOperacion { get; set; }
        public string TipoOperacion { get; set; }
        public string RespuestaAutorizador { get; set; }
        public Nullable<System.DateTime> FechaAutorizacion { get; set; }
        public string ComentarioAutorizacion { get; set; }
        public string EstadoAutorizacion { get; set; }
        public virtual Operacion Operacion { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
