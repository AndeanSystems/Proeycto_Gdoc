using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gdoc.Web.Util
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
    }
}