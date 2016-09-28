using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gdoc.Entity.Models;
using Gdoc.Entity.Extension;
using Gdoc.Common.Utilitario;

namespace Gdoc.Dao
{
    public class DDocumentoElectronicoOperacion
    {
        public short Grabar(DocumentoElectronicoOperacion eDocumentoElectronicoOperacion) {
            try
            {
                using (var db = new DataBaseContext())
                {
                    db.DocumentoElectronicoOperacions.Add(eDocumentoElectronicoOperacion);
                    db.SaveChanges();
                }
                return 1;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public short Editar(DocumentoElectronicoOperacion eDocumentoElectronicoOperacion)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    var entidad = db.DocumentoElectronicoOperacions.Find(eDocumentoElectronicoOperacion.IDDoctoElectronicoOperacion);
                    entidad.Memo = eDocumentoElectronicoOperacion.Memo;
                    db.SaveChanges();
                }
                return 1;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
