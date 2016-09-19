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
        public EUsuario ValidarLogin(EUsuario usuario)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    var usu = (from usua in db.Usuarios
                               join persona in db.Personals
                               on usua.IDPersonal equals persona.IDPersonal

                               join tipousu in db.Conceptoes
                               on usua.CodigoTipoUsua equals tipousu.CodiConcepto

                               where usua.NombreUsuario == usuario.NombreUsuario &&
                                       usua.ClaveUsuario == usuario.ClaveUsuario &&
                                       tipousu.TipoConcepto.Equals("010")
                               select new { usua, persona, tipousu }).FirstOrDefault();
                    return new EUsuario()
                    {
                         IDUsuario = usu.usua.IDUsuario,
                         NombreUsuario = usu.usua.NombreUsuario,
                         ClaveUsuario=usu.usua.ClaveUsuario,
                         RutaAvatar = usu.usua.RutaAvatar,
                         Personal = usu.persona,
                         TipoUsuario = new Concepto { DescripcionConcepto = usu.tipousu.DescripcionConcepto }
                         
                    };
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public EUsuario CantidadAlerta(EUsuario usuario)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    var usu = (from usua in db.Usuarios
                               join alerta in db.MensajeAlertas
                               on usua.IDUsuario equals alerta.IDUsuario

                               where usua.NombreUsuario == usuario.NombreUsuario &&
                                        alerta.FechaAlerta.Value.Day == System.DateTime.Now.Day

                               group new{usua,alerta} by new{
                                usua.IDUsuario,
                                usua.NombreUsuario
                               }
                               into grp select new {
                                   Count =grp.Count(),
                                   grp.Key.IDUsuario,
                                   grp.Key.NombreUsuario
                               }

                               //select new { usua, alerta }
                               ).FirstOrDefault();

                    if (usu != null)
                    {
                        return new EUsuario()
                        {
                            IDUsuario = usu.IDUsuario,
                            NombreUsuario = usu.NombreUsuario,
                            CantidadAlerta = usu.Count,

                        };
                    }
                    else return new EUsuario()
                    {
                        CantidadAlerta=0,
                    };
                    
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public EUsuario CantidadDocumentosRecibidos(EUsuario usuario)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    var usu = (from usuparti in db.UsuarioParticipantes

                               where (usuparti.TipoParticipante =="03" ||
                                     usuparti.TipoParticipante == "08") &&
                                     usuparti.IDUsuario==usuario.IDUsuario
                               && usuparti.FechaNotificacion.Value.Day == System.DateTime.Now.Day
                               group new { usuparti } by new
                               {
                                   usuparti.IDUsuario,
                               }
                                   into grp
                                   select new
                                   {
                                       Count = grp.Count(),
                                       grp.Key.IDUsuario,
                                   }

                               //select new { usua, alerta }
                               ).FirstOrDefault();

                    if (usu != null)
                    {
                        return new EUsuario()
                        {
                            IDUsuario = usu.IDUsuario,
                            CantidadDocumentosRecibidos = usu.Count,

                        };
                    }
                    else return new EUsuario()
                    {
                        CantidadDocumentosRecibidos = 0,
                    };

                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public EUsuario CantidadMesaVirtual(EUsuario usuario)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    var usu = (from usuparti in db.UsuarioParticipantes

                               where usuparti.TipoParticipante == "02" &&
                                     usuparti.IDUsuario == usuario.IDUsuario &&

                                      usuparti.FechaNotificacion.Value.Day == System.DateTime.Now.Day

                               group new { usuparti } by new
                               {
                                   usuparti.IDUsuario,
                               }
                                   into grp
                                   select new
                                   {
                                       Count = grp.Count(),
                                       grp.Key.IDUsuario,
                                   }

                               //select new { usua, alerta }
                               ).FirstOrDefault();

                    if (usu != null)
                    {
                        return new EUsuario()
                        {
                            IDUsuario = usu.IDUsuario,
                            CantidadMesasVirtual = usu.Count,

                        };
                    }
                    else return new EUsuario()
                    {
                        CantidadMesasVirtual = 0,
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

                                 join Estado in db.Conceptoes
                                 on u.EstadoUsuario.ToString() equals  Estado.CodiConcepto

                                 join e in db.Empresas
                                 on p.IDEmpresa equals e.IDEmpresa

                                 where Cargo.TipoConcepto.Equals("007") &&
                                         Tipo.TipoConcepto.Equals("010") &&
                                         Area.TipoConcepto.Equals("013") &&
                                         Clase.TipoConcepto.Equals("021") &&
                                         Estado.TipoConcepto.Equals("017")


                                 select new { u, p, Cargo, Tipo, Area, Clase ,Estado, e}).ToList();


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
                        ClaveUsuario=x.u.ClaveUsuario,
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
                            TipoIdentificacion=x.p.TipoIdentificacion,
                            NumeroIdentificacion=x.p.NumeroIdentificacion,

                        },
                        NombreCompleto = string.Format("{0}, {1}", x.p.NombrePers, x.p.ApellidoPersonal),
                        RazoSocial =new Empresa{ RazonSocial=x.e.RazonSocial},
                        Cargo = new Concepto { DescripcionConcepto = x.Cargo.DescripcionConcepto },
                        TipoUsuario = new Concepto { DescripcionConcepto = x.Tipo.DescripcionConcepto },
                        Area = new Concepto { DescripcionConcepto = x.Area.DescripcionConcepto },
                        ClaseUsu = new Concepto { DescripcionConcepto = x.Clase.DescripcionConcepto },
                        Estado = new Concepto { DescripcionConcepto = x.Estado.DescripcionConcepto }

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
                    entidad.ClaveUsuario = usuario.ClaveUsuario;
                    entidad.FirmaElectronica = usuario.FirmaElectronica;
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

        public short GrabarUsuarioAvatar(Usuario eUsuario)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    var query = db.Usuarios.Find(eUsuario.IDUsuario);
                    query.RutaAvatar = eUsuario.RutaAvatar;
                    db.SaveChanges();
                }
                return 1;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
    }
}
