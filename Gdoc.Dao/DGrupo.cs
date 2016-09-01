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

                    var list2 = (from grupo in db.Grupoes
                                join usugrupo in db.UsuarioGrupoes
                                on grupo.IDGrupo equals usugrupo.IDGrupo
                                select new { grupo, usugrupo }).ToList();

                    list.ForEach(x => listGrupo.Add(new Grupo
                    {
                        IDGrupo = x.IDGrupo,
                        CodigoGrupo = x.CodigoGrupo,
                        NombreGrupo = x.NombreGrupo,
                        FechaModifica = x.FechaModifica,
                        UsuarioModifica = x.UsuarioModifica,
                        ComentarioGrupo = x.ComentarioGrupo,
                        EstadoGrupo = x.EstadoGrupo,

                    }));
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return listGrupo;
        }
        public Grupo GrabarGrupoUsuarios(Grupo grupo)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    db.Grupoes.Add(grupo);
                    db.SaveChanges();
                }
                return grupo;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
