using System;
using System.Collections.Generic;

namespace Gdoc.Entity.Models
{
    public partial class ModuloPaginaUrl
    {
        public ModuloPaginaUrl()
        {
            this.AccesoSistemas = new List<AccesoSistema>();
        }

        public long IDModuloPagina { get; set; }
        public string NombrePagina { get; set; }
        public string ComentarioPagina { get; set; }
        public string DireccionFisicaPagina { get; set; }
        public Nullable<int> EstadoPagina { get; set; }
        public Nullable<long> CodigoPaginaPadre { get; set; }
        public string ModuloSistema { get; set; }
        public virtual ICollection<AccesoSistema> AccesoSistemas { get; set; }
    }
}
