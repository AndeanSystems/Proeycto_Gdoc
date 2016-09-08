using System;
using System.Collections.Generic;

namespace Gdoc.Entity.Models
{
    public partial class UsuarioParticipante
    {
        public long IDUsuarioParticipante { get; set; }
        public long IDUsuario { get; set; }
        public long IDOperacion { get; set; }
        public string TipoOperacion { get; set; }
        public string TipoParticipante { get; set; }
        public string AprobarOperacion { get; set; }
        public string EnviarNotificiacion { get; set; }
        public Nullable<System.DateTime> FechaNotificacion { get; set; }
        public int EstadoUsuarioParticipante { get; set; }
        public string ConfirmacionLectura { get; set; }
        public string PostergacionLectura { get; set; }
        public string ReenvioOperacion { get; set; }
        public string EnvioOperacion { get; set; }
        public virtual Operacion Operacion { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
