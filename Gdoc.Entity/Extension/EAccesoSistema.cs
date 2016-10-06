using Gdoc.Entity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gdoc.Entity.Extension
{
    [NotMapped]
    public class EAccesoSistema:AccesoSistema
    {
        public Personal Persona { get; set; }
        public string NombreCompleto { get; set; }
        public long IDModuloPagina2 { get; set; }
    }
}
