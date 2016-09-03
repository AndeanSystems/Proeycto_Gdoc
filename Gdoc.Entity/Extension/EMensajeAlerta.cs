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
    public class EMensajeAlerta:MensajeAlerta
    {
        public Concepto TipoOperacion { get; set; }
        public Concepto TipoDocumento { get; set; }
        public UsuarioParticipante UsuarioParticipante { get; set; }
        public Usuario Usuario { get; set; }
    }
}
