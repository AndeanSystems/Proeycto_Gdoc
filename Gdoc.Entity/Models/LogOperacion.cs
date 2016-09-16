using System;
using System.Collections.Generic;

namespace Gdoc.Entity.Models
{
    public partial class LogOperacion
    {
        public long IDLogOperacion { get; set; }
        public System.DateTime FechaEvento { get; set; }
        public long IDOperacion { get; set; }
        public string CodigoEvento { get; set; }
        public long IDUsuario { get; set; }
        public string CodigoConexion { get; set; }
        public string TerminalConexion { get; set; }
        public virtual Operacion Operacion { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
