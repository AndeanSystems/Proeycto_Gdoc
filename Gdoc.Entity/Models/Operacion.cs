using System;
using System.Collections.Generic;

namespace Gdoc.Entity.Models
{
    public partial class Operacion
    {
        public Operacion()
        {
            this.DocumentoAdjuntoes = new List<DocumentoAdjunto>();
            this.DocumentoDigitalOperacions = new List<DocumentoDigitalOperacion>();
            this.DocumentoElectronicoOperacions = new List<DocumentoElectronicoOperacion>();
            this.IndexacionDocumentoes = new List<IndexacionDocumento>();
            this.LogOperacions = new List<LogOperacion>();
            this.MensajeAlertas = new List<MensajeAlerta>();
            this.MesaVirtualComentarios = new List<MesaVirtualComentario>();
            this.UsuarioAutorizadors = new List<UsuarioAutorizador>();
            this.UsuarioParticipantes = new List<UsuarioParticipante>();
        }

        public long IDOperacion { get; set; }
        public int IDEmpresa { get; set; }
        public string TipoOperacion { get; set; }
        public Nullable<System.DateTime> FechaEmision { get; set; }
        public string NumeroOperacion { get; set; }
        public string TituloOperacion { get; set; }
        public string AccesoOperacion { get; set; }
        public string EstadoOperacion { get; set; }
        public string DescripcionOperacion { get; set; }
        public string PrioridadOperacion { get; set; }
        public Nullable<System.DateTime> FechaCierre { get; set; }
        public Nullable<System.DateTime> FechaRegistro { get; set; }
        public Nullable<System.DateTime> FechaEnvio { get; set; }
        public Nullable<System.DateTime> FechaVigente { get; set; }
        public string DocumentoAdjunto { get; set; }
        public string TipoComunicacion { get; set; }
        public string NotificacionOperacion { get; set; }
        public virtual ICollection<DocumentoAdjunto> DocumentoAdjuntoes { get; set; }
        public virtual ICollection<DocumentoDigitalOperacion> DocumentoDigitalOperacions { get; set; }
        public virtual ICollection<DocumentoElectronicoOperacion> DocumentoElectronicoOperacions { get; set; }
        public virtual Empresa Empresa { get; set; }
        public virtual ICollection<IndexacionDocumento> IndexacionDocumentoes { get; set; }
        public virtual ICollection<LogOperacion> LogOperacions { get; set; }
        public virtual ICollection<MensajeAlerta> MensajeAlertas { get; set; }
        public virtual ICollection<MesaVirtualComentario> MesaVirtualComentarios { get; set; }
        public virtual ICollection<UsuarioAutorizador> UsuarioAutorizadors { get; set; }
        public virtual ICollection<UsuarioParticipante> UsuarioParticipantes { get; set; }
        public string TipoDocumento { get; set; }
    }
}
