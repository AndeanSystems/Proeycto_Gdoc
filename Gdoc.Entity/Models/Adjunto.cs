using System;
using System.Collections.Generic;

namespace Gdoc.Entity.Models
{
    public partial class Adjunto
    {
        public Adjunto()
        {
            this.DocumentoAdjuntoes = new List<DocumentoAdjunto>();
        }

        public long IDAdjunto { get; set; }
        public Nullable<long> IDUsuario { get; set; }
        public string NombreOriginal { get; set; }
        public string RutaArchivo { get; set; }
        public string TamanoArchivo { get; set; }
        public Nullable<System.DateTime> FechaRegistro { get; set; }
        public Nullable<int> EstadoAdjunto { get; set; }
        public string TipoArchivo { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual ICollection<DocumentoAdjunto> DocumentoAdjuntoes { get; set; }
    }
}
