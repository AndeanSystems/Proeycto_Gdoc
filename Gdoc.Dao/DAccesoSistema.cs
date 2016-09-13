using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gdoc.Entity.Models;
using Gdoc.Entity.Extension;


namespace Gdoc.Dao
{
    public class DAccesoSistema
    {
        public List<EAccesoSistema> ListarAccesoSistema()
        {
            var listAccesoSistema = new List<EAccesoSistema>();
            try
            {
                using (var db = new DataBaseContext())
                {

                    var list2 = (from usuario in db.Usuarios
                                 join acceso in db.AccesoSistemas
                                 on usuario.IDUsuario equals acceso.IDUsuario

                                 join modulo in db.ModuloPaginaUrls
                                 on acceso.IDModuloPagina equals modulo.IDModuloPagina

                                 join persona in db.Personals
                                 on usuario.IDPersonal equals persona.IDPersonal
                                 select new { usuario, acceso, modulo, persona }).ToList();

                    var list3 = (from modulopagina in db.ModuloPaginaUrls
                                 join acceso in db.AccesoSistemas
                                 on modulopagina.IDModuloPagina equals acceso.IDModuloPagina

                                 join usuario in db.Usuarios
                                 on acceso.IDUsuario equals usuario.IDUsuario

                                 select new { modulopagina, acceso, usuario }).ToList();

                    list2.ForEach(x => listAccesoSistema.Add(new EAccesoSistema
                    {
                        IDAcceso=x.acceso.IDAcceso,
                        IDUsuario=x.acceso.IDUsuario,
                        IDModuloPagina=x.acceso.IDModuloPagina,
                        IdeUsuarioRegistro=x.acceso.IdeUsuarioRegistro,
                        ModuloPaginaUrl = new ModuloPaginaUrl
                        {
                            ModuloSistema = x.modulo.ModuloSistema,
                            NombrePagina = x.modulo.NombrePagina,
                            DireccionFisicaPagina = x.modulo.DireccionFisicaPagina,
                            CodigoPaginaPadre = x.modulo.CodigoPaginaPadre,
                        },

                        Usuario = new Usuario
                        {
                            NombreUsuario = x.usuario.NombreUsuario,

                        },
                        FechaModificacion = x.acceso.FechaModificacion,
                        EstadoAcceso = x.acceso.EstadoAcceso,
                        NombreCompleto = string.Format("{0}, {1}", x.persona.NombrePers, x.persona.ApellidoPersonal),

                        
                        //Persona = new Personal
                        //{
                        //    NombrePers = string.Format("{0}, {1}", x.persona.NombrePers, x.persona.ApellidoPersonal),
                        //},
                        
                    }));
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return listAccesoSistema;
        }
        public AccesoSistema CambiarEstadoAcceso(AccesoSistema accesosistema)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    var acceso = db.AccesoSistemas.Find(accesosistema.IDAcceso);
                    acceso.EstadoAcceso = accesosistema.EstadoAcceso;
                    db.SaveChanges();
                }
                return accesosistema;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
