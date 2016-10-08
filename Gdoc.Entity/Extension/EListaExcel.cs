using Gdoc.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gdoc.Entity.Extension
{
    public class EListaExcel
    {
        public int IDEmpresa { get; set; }
        public string TipoOperacion { get; set; }
        public Nullable<System.DateTime> FechaEmision { get; set; }
        public string NumeroOperacion { get; set; }
        public string TituloOperacion { get; set; }
        public string AccesoOperacion { get; set; }
        public Nullable<int> EstadoOperacion { get; set; }
        public string DescripcionOperacion { get; set; }
        public string PrioridadOperacion { get; set; }
        public Nullable<System.DateTime> FechaCierre { get; set; }
        public Nullable<System.DateTime> FechaRegistro { get; set; }
        public Nullable<System.DateTime> FechaEnvio { get; set; }
        public Nullable<System.DateTime> FechaVigente { get; set; }
        public string DocumentoAdjunto { get; set; }
        public string TipoComunicacion { get; set; }
        public string NotificacionOperacion { get; set; }
        public string TipoDocumento { get; set; }
        public string NombreFinal { get; set; }
        public string TipoOpe { get; set; }
        public string TipoDoc { get; set; }
        public string Estado { get; set; }
        public string Prioridad { get; set; }
    }
}
