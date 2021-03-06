﻿using Gdoc.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gdoc.Entity.Models;
using Gdoc.Entity.Extension;
namespace Gdoc.Negocio
{
    public class NDocumentoAdjunto : IDisposable
    {
        private DDocumentoAdjunto dDocumentoAdjunto = new DDocumentoAdjunto();
        public void Dispose()
        {
            dDocumentoAdjunto = null;
        }
        public List<EDocumentoAdjunto> ListarDocumentoAdjunto()
        {
            try
            {
                return dDocumentoAdjunto.ListarDocumentoAdjunto();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public short GrabarDocumentoAdjunto(DocumentoAdjunto DocumentoAdjunto)
        {
            try
            {
                dDocumentoAdjunto.GrabarDocumentoAdjunto(DocumentoAdjunto);
                return 1;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
