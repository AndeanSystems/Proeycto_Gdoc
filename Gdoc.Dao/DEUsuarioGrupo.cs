﻿using Gdoc.Entity.Extension;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Gdoc.Dao
{
    public class DEUsuarioGrupo
    {
        public List<EUsuarioGrupo> ObtenerUsuarioGrupo(EUsuarioGrupo eUsuarioGrupo) {
            var listEUsuarioGrupo = new List<EUsuarioGrupo>();
            try
            {
                var NombreParameter = new SqlParameter { ParameterName = "@avNombre", Value = eUsuarioGrupo.Nombre };
                using (var db = new DataBaseContext())
                {
                    var listResult = db.Database.SqlQuery<EUsuarioGrupo>("gdoc_sel_ObtenerUsuarioGrupo @avNombre", NombreParameter).ToList();
                    foreach (var item in listResult)
                    {
                        var vUsuarioGrupo = new EUsuarioGrupo
                        {
                            Estado = item.Estado,
                            Nombre = item.Nombre,
                            IDUsuarioGrupo = item.IDUsuarioGrupo,
                            Tipo = item.Tipo
                        };
                        listEUsuarioGrupo.Add(vUsuarioGrupo);
                    }
                }
                return listEUsuarioGrupo;
            }
            catch (System.Exception ex)
            {

                throw;
            }
        }
        public List<EUsuarioGrupo> ObtenerUsuario(EUsuarioGrupo eUsuarioGrupo)
        {
            var listEUsuarioGrupo = new List<EUsuarioGrupo>();
            try
            {
                using (var db = new DataBaseContext())
                {
                    var query = db.Usuarios.Where(x => x.IDUsuario == eUsuarioGrupo.IDUsuarioGrupo).ToList();
                    query.ForEach(x => listEUsuarioGrupo.Add(
                        new EUsuarioGrupo {IDUsuarioGrupo = x.IDUsuario,Nombre = x.NombreUsuario,Tipo = "U" }
                        ));
                }
                return listEUsuarioGrupo;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
