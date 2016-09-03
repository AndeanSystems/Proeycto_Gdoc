using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gdoc.Entity.Models;
using Gdoc.Entity.Extension;

namespace Gdoc.Dao
{
    public class DGrupo
    {
        public List<EGrupo> ListarGrupo()
        {
            var listGrupo = new List<EGrupo>();
            try
            {
                using (var db = new DataBaseContext())
                {
                    var list = db.Grupoes.ToList();

                    var list2 = (from grupo in db.Grupoes
                                join usugrupo in db.UsuarioGrupoes
                                on grupo.IDGrupo equals usugrupo.IDGrupo
                                group new {grupo,usugrupo} by  new {
                                    grupo.CodigoGrupo,
                                    grupo.NombreGrupo,
                                    grupo.ComentarioGrupo,
                                    grupo.EstadoGrupo}
                                into grp
                                     select new { Count = grp.Count(), 
                                         grp.Key.CodigoGrupo,
                                         grp.Key.NombreGrupo,
                                         grp.Key.ComentarioGrupo,
                                         grp.Key.EstadoGrupo}).ToList();
                    // grupo, usugrupo 

                    list2.ForEach(x => listGrupo.Add(new EGrupo
                    {
                        CodigoGrupo = x.CodigoGrupo,
                        NombreGrupo = x.NombreGrupo,
                        ComentarioGrupo = x.ComentarioGrupo,
                        EstadoGrupo = x.EstadoGrupo,
                        CantidadUsuarios=x.Count,
                        

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
