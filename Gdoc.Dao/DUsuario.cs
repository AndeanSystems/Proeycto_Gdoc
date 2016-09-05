using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gdoc.Entity.Models;
using Gdoc.Entity.Extension;

namespace Gdoc.Dao
{
    public class DUsuario 
    {
        public Usuario ValidarLogin(Usuario usuario)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    var usu = (from usua in db.Usuarios
                               join persona in db.Personals
                               on usua.IDPersonal equals persona.IDPersonal

                               join cargo in db.Conceptoes
                               on persona.CodigoCargo equals cargo.CodiConcepto

                               where usua.NombreUsuario == usuario.NombreUsuario &&
                                       usua.ClaveUsuario == usuario.ClaveUsuario 
                                       //cargo.TipoConcepto.Equals("007")
                               select new { usua,persona,cargo}).FirstOrDefault();
                    return new Usuario() {
                         IDUsuario = usu.usua.IDUsuario,
                         NombreUsuario = usu.usua.NombreUsuario,
                         Personal = usu.persona
                         
                    };
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<EUsuario> ListarUsuario()
        {
            var listUsuario = new List<EUsuario>();
            try
            {
                using (var db = new DataBaseContext())
                {
                    var list4 = (from u in db.Usuarios
                                 join p in db.Personals
                                 on u.IDPersonal equals p.IDPersonal

                                 join Cargo in db.Conceptoes
                                 on p.CodigoCargo equals Cargo.CodiConcepto

                                 join Tipo in db.Conceptoes
                                 on u.CodigoTipoUsua equals Tipo.CodiConcepto

                                 join Area in db.Conceptoes
                                 on p.CodigoArea equals Area.CodiConcepto

                                 join Clase in db.Conceptoes
                                 on u.ClaseUsuario equals Clase.CodiConcepto

                                 join e in db.Empresas
                                 on p.IDEmpresa equals e.IDEmpresa

                                 where Cargo.TipoConcepto.Equals("007") &&
                                         Tipo.TipoConcepto.Equals("010") &&
                                         Area.TipoConcepto.Equals("013") &&
                                         Clase.TipoConcepto.Equals("021")


                                 select new { u, p, Cargo, Tipo, Area, Clase, e}).ToList();


                    list4.ForEach(x => listUsuario.Add(new EUsuario
                    {
                        IDUsuario = x.u.IDUsuario,
                        NombreUsuario = x.u.NombreUsuario,
                        FirmaElectronica = x.u.FirmaElectronica,
                        EstadoUsuario = x.u.EstadoUsuario,
                        FechaRegistro = x.u.FechaRegistro,
                        FechaUltimoAcceso = x.u.FechaUltimoAcceso,
                        FechaModifica = x.u.FechaModifica,
                        IntentoErradoClave = x.u.IntentoErradoClave,
                        IntentoerradoFirma = x.u.IntentoerradoFirma,
                        TerminalUsuario = x.u.TerminalUsuario,
                        UsuarioRegistro = x.u.UsuarioRegistro,
                        CodigoConexion = x.u.CodigoConexion,
                        IDPersonal = x.u.IDPersonal,
                        CodigoRol = x.u.CodigoRol,
                        CodigoTipoUsua = x.u.CodigoTipoUsua,
                        ClaseUsuario=x.u.ClaseUsuario,
                        ExpiraClave = x.u.ExpiraClave,
                        ExpiraFirma = x.u.ExpiraFirma,
                        FechaExpiraClave = x.u.FechaExpiraClave,
                        FechaExpiraFirma = x.u.FechaExpiraFirma,
                        Personal = new Personal
                        {
                            IDPersonal = x.p.IDPersonal,
                            IDEmpresa = x.p.IDEmpresa,
                            CodigoCargo = x.p.CodigoCargo,
                            CodigoArea = x.p.CodigoArea,
                            NombrePers = x.p.NombrePers,
                            ApellidoPersonal = x.p.ApellidoPersonal,
                            EmailTrabrajo = x.p.EmailTrabrajo,
                            TelefonoPersonal = x.p.TelefonoPersonal,

                        },
                        RutaFoto = "http://lorempixel.com/50/50/people?6",
                        NombreCompleto = string.Format("{0}, {1}", x.p.NombrePers, x.p.ApellidoPersonal),
                        RazoSocial =new Empresa{ RazonSocial=x.e.RazonSocial},
                        Cargo = new Concepto { DescripcionConcepto = x.Cargo.DescripcionConcepto },
                        TipoUsuario = new Concepto { DescripcionConcepto = x.Tipo.DescripcionConcepto },
                        Area = new Concepto { DescripcionConcepto = x.Area.DescripcionConcepto },
                        ClaseUsu = new Concepto { DescripcionConcepto = x.Clase.DescripcionConcepto }

                    }));
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return listUsuario;
        }
        public Usuario GrabarUsuario(Usuario usuario)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    //db.Personals.Add(usuario.Personal);
                    //db.SaveChanges();
                    //usuario.IDPersonal = usuario.Personal.IDPersonal; Faltar terminar quitar grabacion de tabla personal
                    db.Usuarios.Add(usuario);
                    db.SaveChanges();
                }
                return usuario;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Usuario EditarUsuario(Usuario usuario)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    var entidad = db.Usuarios.Find(usuario.IDUsuario);
                    entidad.TerminalUsuario = usuario.TerminalUsuario;
                    entidad.CodigoTipoUsua = usuario.CodigoTipoUsua;
                    entidad.ClaseUsuario = usuario.ClaseUsuario;
                    entidad.CodigoRol = usuario.CodigoRol;
                    entidad.ExpiraFirma = usuario.ExpiraFirma;
                    entidad.EstadoUsuario = usuario.EstadoUsuario;
                    db.SaveChanges();
                }
                return usuario;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public Usuario EliminarUsuario(Usuario usuario)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    var usu = db.Usuarios.Find(usuario.IDUsuario);
                    usu.EstadoUsuario = usuario.EstadoUsuario;
                    db.SaveChanges();
                }
                return usuario;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
