using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gdoc.Entity.Models;

namespace Gdoc.Dao
{
    public class DGrupo
    {
        public List<Grupo> ListarGrupo()
        {
            var listGrupo = new List<Grupo>();
            try
            {
                using (var db = new DataBaseContext())
                {
                    var list = db.Grupoes.ToList();

                    list.ForEach(x => listGrupo.Add(new Grupo
                    {
                        IDGrupo = x.IDGrupo,
                        CodigoGrupo=x.CodigoGrupo,
                        NombreGrupo=x.NombreGrupo,
                        FechaModifica=x.FechaModifica,
                        UsuarioModifica=x.UsuarioModifica,
                        ComentarioGrupo=x.ComentarioGrupo,
                        EstadoGrupo=x.EstadoGrupo,

                    }));
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return listGrupo;
        }
    }
}
