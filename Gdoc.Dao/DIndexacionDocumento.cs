using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gdoc.Entity.Models;
using Gdoc.Entity.Extension;


namespace Gdoc.Dao
{
    public class DIndexacionDocumento
    {
        public List<IndexacionDocumento> ListarIndexacionDocumento()
        {
            var listIndexacionDocumento = new List<IndexacionDocumento>();
            try
            {
                using (var db = new DataBaseContext())
                {
                    var list = db.IndexacionDocumentoes.ToList();

                    list.ForEach(x => listIndexacionDocumento.Add(new IndexacionDocumento
                    {
                        IDIndiceDocto=x.IDIndiceDocto,
                        DescripcionIndice=x.DescripcionIndice,
                        EstadoIndice=x.EstadoIndice,
                        IDOperacion = x.IDOperacion,
                        CodigoTipoOperacion=x.CodigoTipoOperacion,
                    }));
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return listIndexacionDocumento;
        }
        public short GrabarIndexacion(List<IndexacionDocumento> listIndexacion)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    db.IndexacionDocumentoes.AddRange(listIndexacion);
                    db.SaveChanges();
                }
                return 1;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public short EditarIndexacion(IndexacionDocumento indexacionDocumento)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    var indexacion = db.IndexacionDocumentoes.Find(indexacionDocumento.IDIndiceDocto);
                    indexacion.EstadoIndice = indexacionDocumento.EstadoIndice;
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
