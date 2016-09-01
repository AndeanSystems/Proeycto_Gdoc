using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gdoc.Entity.Models;

namespace Gdoc.Dao
{
    public class DUbigeo
    {
        public List<Ubigeo> ListarUbigeo()
        {
            var listUbigeo = new List<Ubigeo>();
            try
            {
                using (var db = new DataBaseContext())
                {
                    var list = db.Ubigeos.ToList();
                    list.ForEach(x => listUbigeo.Add(new Ubigeo
                    {
                        CodigoUbigeo = x.CodigoUbigeo,
                        CodigoPais =x.CodigoPais,
                        CodigoDepartamento = x.CodigoDepartamento,
                        CodigoProvincia = x.CodigoProvincia,
                        CodigoDistrito = x.CodigoDistrito,
                        DescripcionUbicacion = x.DescripcionUbicacion,
                        EstadoUbigeo =x.EstadoUbigeo
                    }));
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return listUbigeo;
        }
    }
}
