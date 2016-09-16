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
    public class EOperacion:Operacion
    {
        public DocumentoDigitalOperacion DocumentoDigitalOperacion { get; set; }
        public DocumentoElectronicoOperacion DocumentoElectronicoOperacion { get; set; }
        public Usuario Usuario { get; set; }
        public UsuarioParticipante UsuarioParticipante { get; set; }
        public Concepto TipoDoc { get; set; }
        public Concepto Estado { get; set; }
    }
}
