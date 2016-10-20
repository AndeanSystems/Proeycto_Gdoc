using System;
using System.Collections.Generic;

namespace Gdoc.Entity.Models
{
    public partial class General
    {
        public long IDCodigoParametro { get; set; }
        public int IDEmpresa { get; set; }
        public Nullable<long> IDUsuario { get; set; }
        public Nullable<int> PlazoDoctoElectronico { get; set; }
        public Nullable<int> ExtensionPlazoDoctoElectronico { get; set; }
        public Nullable<int> AlertaDoctoElectronico { get; set; }
        public Nullable<int> PlazoMesaVirtual { get; set; }
        public Nullable<int> ExtensionPlazoMesaVirtual { get; set; }
        public Nullable<int> AlertaMesaVirtual { get; set; }
        public Nullable<int> AlertaMailLaboral { get; set; }
        public Nullable<int> AlertaMailPersonal { get; set; }
        public Nullable<System.DateTime> HoraActualizaEstadoOperacion { get; set; }
        public Nullable<System.DateTime> HoraCierreLabores { get; set; }
        public Nullable<int> PlazoExpiraFirma { get; set; }
        public string RutaGdocImagenes { get; set; }
        public string RutaGdocPDF { get; set; }
        public string RutaGdocAdjuntos { get; set; }
        public string RutaGdocExternos { get; set; }
        public string Servidor { get; set; }
        public Nullable<int> TamanoMaxArchivo { get; set; }
        public Nullable<int> TamanoMaxArchivos { get; set; }
        public virtual Empresa Empresa { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
