using System;
using System.Collections.Generic;

namespace Gdoc.Entity.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            this.AccesoSistemas = new List<AccesoSistema>();
            this.Adjuntoes = new List<Adjunto>();
            this.Generals = new List<General>();
            this.LogOperacions = new List<LogOperacion>();
            this.MensajeAlertas = new List<MensajeAlerta>();
            this.MesaVirtualComentarios = new List<MesaVirtualComentario>();
            this.UsuarioAutorizadors = new List<UsuarioAutorizador>();
            this.UsuarioGrupoes = new List<UsuarioGrupo>();
            this.UsuarioParticipantes = new List<UsuarioParticipante>();
        }

        public long IDUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public string ClaveUsuario { get; set; }
        public string FirmaElectronica { get; set; }
        public Nullable<int> EstadoUsuario { get; set; }
        public Nullable<System.DateTime> FechaRegistro { get; set; }
        public Nullable<System.DateTime> FechaUltimoAcceso { get; set; }
        public Nullable<System.DateTime> FechaModifica { get; set; }
        public Nullable<short> IntentoErradoClave { get; set; }
        public Nullable<short> IntentoerradoFirma { get; set; }
        public string TerminalUsuario { get; set; }
        public string UsuarioRegistro { get; set; }
        public string CodigoConexion { get; set; }
        public Nullable<long> IDPersonal { get; set; }
        public string CodigoRol { get; set; }
        public string CodigoTipoUsua { get; set; }
        public string ClaseUsuario { get; set; }
        public string ExpiraClave { get; set; }
        public string ExpiraFirma { get; set; }
        public Nullable<System.DateTime> FechaExpiraClave { get; set; }
        public Nullable<System.DateTime> FechaExpiraFirma { get; set; }
        public virtual ICollection<AccesoSistema> AccesoSistemas { get; set; }
        public virtual ICollection<Adjunto> Adjuntoes { get; set; }
        public virtual ICollection<General> Generals { get; set; }
        public virtual ICollection<LogOperacion> LogOperacions { get; set; }
        public virtual ICollection<MensajeAlerta> MensajeAlertas { get; set; }
        public virtual ICollection<MesaVirtualComentario> MesaVirtualComentarios { get; set; }
        public virtual Personal Personal { get; set; }
        public virtual ICollection<UsuarioAutorizador> UsuarioAutorizadors { get; set; }
        public virtual ICollection<UsuarioGrupo> UsuarioGrupoes { get; set; }
        public virtual ICollection<UsuarioParticipante> UsuarioParticipantes { get; set; }
    }
}
