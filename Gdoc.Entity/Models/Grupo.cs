using System;
using System.Collections.Generic;

namespace Gdoc.Entity.Models
{
    public partial class Grupo
    {
        public Grupo()
        {
            this.UsuarioGrupoes = new List<UsuarioGrupo>();
        }

        public long IDGrupo { get; set; }
        public string CodigoGrupo { get; set; }
        public string NombreGrupo { get; set; }
        public Nullable<System.DateTime> FechaModifica { get; set; }
        public string UsuarioModifica { get; set; }
        public string ComentarioGrupo { get; set; }
        public Nullable<int> EstadoGrupo { get; set; }
        public virtual ICollection<UsuarioGrupo> UsuarioGrupoes { get; set; }
    }
}
