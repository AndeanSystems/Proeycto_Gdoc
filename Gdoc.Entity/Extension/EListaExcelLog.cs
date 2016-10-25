using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gdoc.Entity.Extension
{
    public class EListaExcelLog
    {
        public long IDLogOperacion { get; set; }
        public System.DateTime FechaEvento { get; set; }
        public string NumeroOperacion { get; set; }
        public string Evento { get; set; }
        public string  NombreUsuario { get; set; }
    }
}
