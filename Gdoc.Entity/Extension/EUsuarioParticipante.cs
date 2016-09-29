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
    public class EUsuarioParticipante:UsuarioParticipante
    {
        public string Tipo { get; set; }
        public long IDUsuarioGrupo { get; set; }
        public string Nombre { get; set; }
        public int Estado { get; set; }
    }
}
