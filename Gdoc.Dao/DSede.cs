using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gdoc.Entity.Models;
using Gdoc.Entity.Extension;

namespace Gdoc.Dao
{
    public class DSede
    {
        public List<Sede> ListarSede()
        {
            var listSede = new List<Sede>();
            try
            {
                using (var db = new DataBaseContext())
                {
                    var list = db.Sedes.ToList();

                    list.ForEach(x => listSede.Add(new Sede
                    {
                        IDSede=x.IDSede,
                        CodigoSede=x.CodigoSede,
                        IDEmpresa = x.IDEmpresa,
                        NombreSede=x.NombreSede,
                        CodigoUbigeo=x.CodigoUbigeo,
                        DireccionSede=x.DireccionSede,
                        TelefonoSede=x.TelefonoSede,
                        EstadoSede=x.EstadoSede,
                        UsuarioModifica=x.UsuarioModifica,
                        FechaModifica=x.FechaModifica
                    }));
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return listSede;
        }
    }
}
