using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gdoc.Entity.Models;


namespace Gdoc.Dao
{
    public class DAccesoSistema
    {
        public List<AccesoSistema> ListarAccesoSistema()
        {
            var listAccesoSistema = new List<AccesoSistema>();
            try
            {
                using (var db = new DataBaseContext())
                {
                    var list = db.AccesoSistemas.ToList();

                    var list2 = (from usuario in db.Usuarios
                                 join acceso in db.AccesoSistemas
                                 on usuario.IDUsuario equals acceso.IDUsuario

                                 join modulo in db.ModuloPaginaUrls
                                 on acceso.IDModuloPagina equals modulo.IDModuloPagina
                                 select new { usuario, acceso, modulo }).ToList();

                    list2.ForEach(x => listAccesoSistema.Add(new AccesoSistema
                    {
                        ModuloPaginaUrl= new ModuloPaginaUrl
                        {
                            ModuloSistema=x.modulo.ModuloSistema,
                            NombrePagina=x.modulo.NombrePagina,
                            DireccionFisicaPagina=x.modulo.DireccionFisicaPagina,
                            CodigoPaginaPadre=x.modulo.CodigoPaginaPadre,
                        },
                        FechaModificacion=x.acceso.FechaModificacion,
                        EstadoAcceso= x.acceso.EstadoAcceso
                        
                    }));
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return listAccesoSistema;
        }
    }
}
