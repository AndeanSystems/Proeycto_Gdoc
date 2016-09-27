using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gdoc.Common.Utilitario
{
    public class Constantes
    {
        public struct TipoOperacion
        {
            public const string DocumentoDigital = "02";
            public const string DocumentoElectronico = "03";
            public const string MesaVirtual = "04";
        }
        public struct TipoParticipante
        {
            public const string ColaboradorMV = "02";
            public const string DestinatarioDE = "03";
            public const string EmisorDE = "04";
            public const string OrganizadorMV = "05";
            public const string RemitenteDE = "06";
            public const string EmisorDD = "07";
            public const string DestinatarioDD = "08";
        }
        public struct EstadoParticipante
        {
            public const short Creado = 0;
            public const short Activo = 1;
            public const short Inactivo = 2;
        }
    }
    public class Estados {
        public struct EstadoEmpresa
        {
            public const short Creado = 0;
            public const short Activo = 1;
            public const short Inactivo = 2;
        }

        public struct EstadoAcceso
        {
            public const short Creado = 0;
            public const short Activo = 1;
            public const short Inactivo = 2;
        }
        public struct EstadoOperacion
        {
            public const short Creado = 0;
            public const short Activo = 1;
            public const short Inactivo = 2;
        }
        public struct EstadoAdjunto
        {
            public const short Creado = 0;
            public const short Activo = 1;
            public const short Inactivo = 2;
        }
    }
}
