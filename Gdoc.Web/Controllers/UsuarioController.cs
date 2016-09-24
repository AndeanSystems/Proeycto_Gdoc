using Gdoc.Entity.Models;
using Gdoc.Negocio;
using Gdoc.Web.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gdoc.Entity.Extension;
using System.Configuration;
using System.Text;
using System.Drawing;
using System.IO;
using ActiveDirectoryValidation;
using Seguridadv2;
namespace Gdoc.Web.Controllers
{
    public class UsuarioController : Controller
    {
        #region "Variables"
        List<eUsuarioActiveDirec> LstUsuAD = new List<eUsuarioActiveDirec>();
        ADFunctions oADFunctions = new ADFunctions();
        EUsuarioFEPCMAC _eUsuario = new EUsuarioFEPCMAC();
        MensajeConfirmacion mensajeRespuesta = new MensajeConfirmacion();
        #endregion
        // GET: Usuario

        public bool ValidarUsuarioEnActiveDirectory(EUsuario usuario)
        {
            //txtUsuario.Text.Equals("reaseguros"))//
            if (oADFunctions.FnValidarUsuario(ConfigurationManager.AppSettings.Get("Dominio"),usuario.NombreUsuario, usuario.ClaveUsuario, ConfigurationManager.AppSettings.Get("UrlLDAP")))
            {
                //Guardamos el usuario y la clave AD en una sesión:
                _eUsuario._Usuario = usuario.NombreUsuario;
                _eUsuario._Contrasena = FNSeguridad.EncriptarConClave(usuario.ClaveUsuario, "11254125852587458124587485215895");
                //eUsuario._Aceso_Pagina = "99";
                Session["CredencialesAD"] = _eUsuario;

                return true;
            }
            else
            {
                return false;
            }
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CambiarContraseña()
        {
            return View();
        }
        public ActionResult CambiarFirmaElectronica()
        {
            return View();
        }
        public ActionResult Login(EUsuario usuario)
        {
            try
            {
                using (var NUsuario = new NUsuario())
                {
                    //false)//
                    if (ValidarUsuarioEnActiveDirectory(usuario) == false)//true)
                    {
                        var UsuarioEncontrado = NUsuario.ValidarLogin(usuario);
                        var CantidadAlerta = NUsuario.CantidadAlerta(UsuarioEncontrado);
                        var CantidadDocumentosRecibidos = NUsuario.CantidadDocumentosRecibidos(UsuarioEncontrado);
                        var CantidadMesaVirtual = NUsuario.CantidadMesaVirtual(UsuarioEncontrado);

                        if (UsuarioEncontrado != null)
                        {
                            Session["IDEmpresa"] = UsuarioEncontrado.Personal.IDEmpresa; //Pendiente falta terminar
                            Session["NombreUsuario"] = UsuarioEncontrado.NombreUsuario;
                            Session["IDUsuario"] = UsuarioEncontrado.IDUsuario;
                            Session["ClaveUsuario"] = UsuarioEncontrado.ClaveUsuario;
                            Session["NombreCompleto"] = string.Format("{0} {1}", FormatoNombre(UsuarioEncontrado.Personal.NombrePers), FormatoNombre(UsuarioEncontrado.Personal.ApellidoPersonal));
                            Session["CargoUsuario"] = FormatoNombre(UsuarioEncontrado.TipoUsuario.DescripcionConcepto);
                            Session["RutaAvatar"] = string.IsNullOrEmpty(UsuarioEncontrado.RutaAvatar) ? "/resources/img/incognito.png" : UsuarioEncontrado.RutaAvatar.Replace("~", string.Empty);

                            //CONTADORES
                            if (CantidadAlerta != null) Session["CantidadAlerta"] = CantidadAlerta.CantidadAlerta;
                            else Session["CantidadAlerta"] = 0;
                            //---
                            if (CantidadDocumentosRecibidos != null) Session["CantidadDocumentosRecibidos"] = CantidadDocumentosRecibidos.CantidadDocumentosRecibidos;
                            else Session["CantidadDocumentosRecibidos"] = 0;
                            //---
                            if (CantidadMesaVirtual != null) Session["CantidadMesaVirtual"] = CantidadMesaVirtual.CantidadMesasVirtual;
                            else Session["CantidadMesaVirtual"] = 0;
                            //--

                            //PARAMETROS GENERALES
                            //using (var NGeneral = new NGeneral())
                            //{
                            //    var CargarParametros = NGeneral.CargaParametros(Convert.ToInt32(Session["IDEmpresa"]));

                            //    Session["PlazoDoctoElectronico"] = CargarParametros.PlazoDoctoElectronico;
                            //    Session["ExtensionPlazoDoctoElectronico"] = CargarParametros.ExtensionPlazoDoctoElectronico;
                            //    Session["AlertaDoctoElectronico"] = CargarParametros.AlertaDoctoElectronico;
                            //    Session["PlazoMesaVirtual"] = CargarParametros.PlazoMesaVirtual;
                            //    Session["ExtensionPlazoMesaVirtual"] = CargarParametros.ExtensionPlazoMesaVirtual;
                            //    Session["AlertaMesaVirtual"] = CargarParametros.AlertaMesaVirtual;
                            //    Session["AlertaMailLaboral"] = CargarParametros.AlertaMailLaboral;
                            //    Session["AlertaMailPersonal"] = CargarParametros.AlertaMailPersonal;
                            //    Session["HoraActualizaEstadoOperacion"] = CargarParametros.HoraActualizaEstadoOperacion;
                            //    Session["HoraCierreLabores"] = CargarParametros.HoraCierreLabores;
                            //    Session["PlazoExpiraFirma"] = CargarParametros.PlazoExpiraFirma;
                            //    Session["RutaGdocImagenes"] = CargarParametros.RutaGdocImagenes;
                            //    Session["RutaGdocPDF"] = CargarParametros.RutaGdocPDF;
                            //    Session["RutaGdocAdjuntos"] = CargarParametros.RutaGdocAdjuntos;
                            //    Session["RutaGdocExternos"] = CargarParametros.RutaGdocExternos;

                            //}

                            return RedirectToAction("Index", "Alertas");
                        }
                        else
                        {
                            TempData["Message"] = "Usuario Incorrecto";
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        TempData["Message2"] = "Usted no pertenece al dominio";
                        return RedirectToAction("Index", "Home");
                    }

                }
            }
            catch (Exception)
            {
                
                throw;
            }
            
        }
        [HttpGet]
        public JsonResult ListarUsuario()
        {
            var listUsuario = new List<EUsuario>();
            using (var oUsuario = new NUsuario())
            {
                listUsuario = oUsuario.ListarUsuario();
            }
            return new JsonResult { Data = listUsuario, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        [HttpPost]
        public JsonResult ListarUsuario(Usuario usuario)
        {
            var listUsuario = new List<EUsuario>();
            using (var oUsuario = new NUsuario())
            {
                listUsuario = oUsuario.ListarUsuario();
            }
            return new JsonResult { Data = listUsuario, MaxJsonLength = Int32.MaxValue };
        }
        [HttpPost]
        public JsonResult ListarUsuarioGrupo(Usuario usuario)
        {
            var listUsuarioGrupo = new List<EUsuario>();
            using (var oUsuario = new NUsuario())
            {
                listUsuarioGrupo = oUsuario.ListarUsuario().Where(x => x.IDUsuario == usuario.IDUsuario).ToList();
            }
            return new JsonResult { Data = listUsuarioGrupo, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public JsonResult GrabarUsuario(Usuario usuario)
        {
            try
            {
                //CopiarFirma("","","");
                using (var oUsuario = new NUsuario())
                {
                    Usuario respuesta = null;
                    if (usuario.IDUsuario > 0)
                    {
                        usuario.FechaModifica = System.DateTime.Now;
                        respuesta = oUsuario.EditarUsuario(usuario);
                    }
                    else
                    {
                        //POR TERMINAR
                        usuario.ClaveUsuario = "";
                        usuario.FechaRegistro = System.DateTime.Now;
                        usuario.FirmaElectronica = "";
                        usuario.FechaModifica = System.DateTime.Now;
                        usuario.IntentoErradoClave = 3;
                        usuario.IntentoerradoFirma = 2;
                        usuario.CodigoConexion = "";

                        usuario.UsuarioRegistro = Session["NombreUsuario"].ToString();
                        usuario.ExpiraClave = "1";//0=EXPIRA || 1=NO EXPIRA
                        //usuario.ExpiraFirma = "90";
                        usuario.FechaExpiraClave = System.DateTime.Now;
                        usuario.FechaExpiraFirma = System.DateTime.Now.AddDays(90);//falta terminar traer el valor de dias de experacion de firmas de la tabla general

                        respuesta = oUsuario.GrabarUsuario(usuario);
                    }
                    mensajeRespuesta.Exitoso = true;
                    mensajeRespuesta.Mensaje = "Grabación Exitosa";
                }
                return new JsonResult { Data = mensajeRespuesta };
            }
            catch (Exception)
            {
                
                throw;
            }
            
        }
        public JsonResult EliminarUsuario(Usuario usuario)
        {
            try
            {
                using (var oUsuario = new NUsuario())
                {
                    usuario.EstadoUsuario = Gdoc.Web.Util.Estados.EstadoEmpresa.Inactivo;
                    var respuesta = oUsuario.EliminarUsuario(usuario);
                    mensajeRespuesta.Exitoso = true;
                    mensajeRespuesta.Mensaje = "Grabación Exitoso";
                }
                return new JsonResult { Data = mensajeRespuesta };
            }
            catch (Exception)
            {
                
                throw;
            }
            
        }
        [HttpPost]
        public JsonResult BuscarUsuarioNombre(Usuario usuario)
        {
            try
            {
                var listUsuario = new List<EUsuario>();
                using (var oUsuario = new NUsuario())
                {
                    if (!string.IsNullOrEmpty(usuario.NombreUsuario))
                        listUsuario = oUsuario.ListarUsuario().Where(x => x.NombreUsuario.Contains(usuario.NombreUsuario.ToUpper())).ToList();
                    else
                        listUsuario = oUsuario.ListarUsuario();
                }

                return new JsonResult { Data = listUsuario, MaxJsonLength = Int32.MaxValue };
            }
            catch (Exception)
            {
                
                throw;
            }
            
        }
        [HttpPost]
        public JsonResult BuscarUsuarioPersonal(Usuario usuario)
        {
            var listUsuario = new List<EUsuario>();
            try
            {
                if (usuario.NombreUsuario != null)
                {
                    listUsuario = new List<EUsuario>();
                    _eUsuario = (EUsuarioFEPCMAC)Session["CredencialesAD"];
                    LstUsuAD = new List<eUsuarioActiveDirec>();
                    var usuarioAD = new EUsuario();
                    try
                    {
                        LstUsuAD = oADFunctions.FnRecuperarDatos("sAMAccountName", usuario.NombreUsuario, ConfigurationManager.AppSettings.Get("UrlLDAP"), _eUsuario._Usuario.ToString(), FNSeguridad.DesencriptarConClave(_eUsuario._Contrasena.ToString(), "11254125852587458124587485215895"));
                    }
                    catch
                    {
                        LstUsuAD = null;
                    }

                    if (LstUsuAD != null)
                    {
                        if (LstUsuAD.Count > 0)
                        {
                            if (LstUsuAD[0]._sAMAccountName != null)
                            {
                                usuarioAD = new EUsuario();

                                usuarioAD.NombreUsuario = LstUsuAD[0]._sAMAccountName;
                                //usuarioAD.Personal.NumeroIdentificacion = "1";
                                //usuarioAD.Personal.TipoIdentificacion = "1";
                                usuarioAD.NombreCompleto = LstUsuAD[0]._cn;
                                usuarioAD.Personal.NombrePers = LstUsuAD[0]._name;
                                //usuarioAD.Personal.CodigoCargo = LstUsuAD[0]._title;
                                //usuarioAD.Personal.CodigoArea = "1";
                                usuarioAD.ClaseUsuario = "0";
                                listUsuario.Add(usuarioAD);
                            }
                        }
                    }

                    return new JsonResult { Data = listUsuario, MaxJsonLength = Int32.MaxValue };
                }
                else if (usuario.Personal.NumeroIdentificacion != null)
                {
                    listUsuario = new List<EUsuario>();
                    using (var oUsuario = new NUsuario())
                    {
                        if (!string.IsNullOrEmpty(usuario.Personal.NumeroIdentificacion))
                            listUsuario = oUsuario.ListarUsuario().Where(x => x.Personal.NumeroIdentificacion == usuario.Personal.NumeroIdentificacion
                                //                                        x.Personal.TipoIdentificacion == usuario.Personal.TipoIdentificacion ||
                                //                                        x.Personal.EstadoPersonal==1
                                                                        ).ToList();
                        else
                            listUsuario = oUsuario.ListarUsuario();
                    }

                    return new JsonResult { Data = listUsuario, MaxJsonLength = Int32.MaxValue };
                }
                else
                    return new JsonResult { Data = "MAL", MaxJsonLength = Int32.MaxValue };
                
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        [HttpGet]
        public JsonResult BuscarUsuarioNombreClave()
        {
            var listUsuario = new List<EUsuario>();
            using (var oUsuario = new NUsuario())
            {
                listUsuario = oUsuario.ListarUsuario().Where(x => x.NombreUsuario == Session["NombreUsuario"].ToString()).ToList();
            }

            return new JsonResult { Data = listUsuario, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public JsonResult MoverFirma(List<EFirma> listFirmas)
        {
            try
            {
                using (var oUsuario = new NUsuario())
                {
                    var respuesta = oUsuario.MoverFirma(listFirmas);
                }
                mensajeRespuesta.Exitoso = true;
                mensajeRespuesta.Mensaje = "Grabación Exitosa";
                return new JsonResult { Data = mensajeRespuesta };
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public JsonResult GrabarUsuarioAvatar(Usuario eUsuario) {
            using (var nUsuario = new NUsuario())
            {
                byte[] fileBytes = System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(eUsuario.RutaAvatar);
                using (MemoryStream stream = new MemoryStream(fileBytes))
                {
                    eUsuario.RutaAvatar = string.Format("{0}_{1}.png", "~/resources/img/", eUsuario.IDUsuario);
                    if (System.IO.File.Exists(eUsuario.RutaAvatar))
                        System.IO.File.Delete(eUsuario.RutaAvatar);
                    Image.FromStream(stream).Save(@Server.MapPath(eUsuario.RutaAvatar), System.Drawing.Imaging.ImageFormat.Png);
                }
                nUsuario.GrabarUsuarioAvatar(eUsuario);
            }
            mensajeRespuesta.Exitoso = true;
            mensajeRespuesta.Mensaje = "OK";
            return new JsonResult { Data = mensajeRespuesta };
        }

        #region "Metodos"
        public string FormatoNombre(string nombre)
        {
            string nombre_orig = nombre;
            string nombre_may = nombre.ToUpper();

            nombre_orig = nombre_orig.ToLower();

            nombre_orig = nombre_orig.Replace(nombre_orig[0], nombre_may[0]);
            return nombre_orig;
        }

        public void CopiarFirma(string file,string ruta,string rutafin)
        {
            file = "firma.jpg";
            ruta = @"C:\Users\ANDEAN\Desktop\firmas";
            rutafin = @"C:\Users\ANDEAN\Desktop\firmas\destino";

            string sourceFile = System.IO.Path.Combine(ruta, file);
            string destFile = System.IO.Path.Combine(rutafin, file);

            if (!System.IO.Directory.Exists(rutafin))
            {
                System.IO.Directory.CreateDirectory(rutafin);
            }

            System.IO.File.Copy(sourceFile, destFile, true);

            
        }
        #endregion
    }
}
