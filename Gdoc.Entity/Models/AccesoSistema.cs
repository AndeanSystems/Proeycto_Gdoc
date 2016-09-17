using System;
using System.Collections.Generic;

namespace Gdoc.Entity.Models
{
    public partial class AccesoSistema
    {
        public long IDAcceso { get; set; }
        public Nullable<long> IDUsuario { get; set; }
        public Nullable<long> IDModuloPagina { get; set; }
        public string IdeUsuarioRegistro { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public int EstadoAcceso { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual ModuloPaginaUrl ModuloPaginaUrl { get; set; }
    }
}
