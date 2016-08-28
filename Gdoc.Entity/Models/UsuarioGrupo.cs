using System;
using System.Collections.Generic;

namespace Gdoc.Entity.Models
{
    public partial class UsuarioGrupo
    {
        public long IDUsuarioGrupo { get; set; }
        public long IDUsuario { get; set; }
        public long IDGrupo { get; set; }
        public string UsuarioRegistro { get; set; }
        public Nullable<System.DateTime> FechaRegistro { get; set; }
        public int EstadoUsuarioGrupo { get; set; }
        public virtual Grupo Grupo { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
