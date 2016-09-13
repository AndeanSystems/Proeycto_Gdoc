using System;
using System.Collections.Generic;

namespace Gdoc.Entity.Models
{
    public partial class DocumentoDigitalOperacion
    {
        public long IDDoctoDigitalOperacion { get; set; }
        public long IDOperacion { get; set; }
        public string DerivarDocto { get; set; }
        public string NombreOriginal { get; set; }
        public string RutaFisica { get; set; }
        public Nullable<int> TamanoDocto { get; set; }
        public string NombreFisico { get; set; }
        public string Comentario { get; set; }
        public virtual Operacion Operacion { get; set; }
        public string TipoArchivo { get; set; }
    }
}
