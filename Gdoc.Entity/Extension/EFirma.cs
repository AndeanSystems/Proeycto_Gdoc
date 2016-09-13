using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gdoc.Entity.Extension
{
    public class EFirma
    {
        public string NombreOriginal { get; set; }
        public string RutaFisica { get; set; }
        public Nullable<int> TamanoDocto { get; set; }
        public string NombreFisico { get; set; }
        public string TipoArchivo { get; set; }
    }
}
