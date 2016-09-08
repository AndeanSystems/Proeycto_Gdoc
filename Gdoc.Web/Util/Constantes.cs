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
            public const string Emisor = "04";
            public const string Remitente = "06";
            public const string Destinatario = "03";
        }
    }
}