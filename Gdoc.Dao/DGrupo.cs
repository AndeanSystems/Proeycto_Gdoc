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

                                join estado in db.Conceptoes
                                on grupo.EstadoGrupo.ToString() equals estado.CodiConcepto

                                where estado.TipoConcepto.Equals("018")
                                group new {grupo,usugrupo,estado} by  new {
                                    grupo.IDGrupo,
                                    grupo.CodigoGrupo,
                                    grupo.NombreGrupo,
                                    grupo.ComentarioGrupo,
                                    grupo.EstadoGrupo,
                                    estado.DescripcionConcepto}
                                into grp
                                     select new { Count = grp.Count(),
                                         grp.Key.IDGrupo,
                                         grp.Key.CodigoGrupo,
                                         grp.Key.NombreGrupo,
                                         grp.Key.ComentarioGrupo,
                                         grp.Key.EstadoGrupo,
                                         grp.Key.DescripcionConcepto}).ToList();
                    // grupo, usugrupo 

                    list2.ForEach(x => listGrupo.Add(new EGrupo
                    {
                        IDGrupo = x.IDGrupo,
                        CodigoGrupo = x.CodigoGrupo,
                        NombreGrupo = x.NombreGrupo,
                        ComentarioGrupo = x.ComentarioGrupo,
                        EstadoGrupo = x.EstadoGrupo,
                        Estado = new Concepto { DescripcionConcepto = x.DescripcionConcepto },
                        CantidadUsuarios = x.Count,
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
        public Grupo EditarGrupo(Grupo grupo)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    var entidad = db.Grupoes.Find(grupo.IDGrupo);
                    entidad.CodigoGrupo = grupo.CodigoGrupo;
                    entidad.NombreGrupo = grupo.NombreGrupo;
                    entidad.ComentarioGrupo = grupo.ComentarioGrupo;
                    db.SaveChanges();
                }
                return grupo;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public Grupo EliminarGrupo(Grupo grupo)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    var grup = db.Grupoes.Find(grupo.IDGrupo);
                    grup.EstadoGrupo = grupo.EstadoGrupo;
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
