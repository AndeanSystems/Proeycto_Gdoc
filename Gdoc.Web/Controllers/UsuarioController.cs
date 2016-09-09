using Gdoc.Entity.Models;
using Gdoc.Negocio;
using Gdoc.Web.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gdoc.Entity.Extension;

namespace Gdoc.Web.Controllers
{
    public class UsuarioController : Controller
    {
        #region "Variables"
        MensajeConfirmacion mensajeRespuesta = new MensajeConfirmacion();
        #endregion
        // GET: Usuario
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
            using (var NUsuario = new NUsuario())
            {
                var UsuarioEncontrado = NUsuario.ValidarLogin(usuario);
                var CantidadAlerta = NUsuario.CantidadAlerta(UsuarioEncontrado);
                var CantidadDocumentosRecibidos = NUsuario.CantidadDocumentosRecibidos(UsuarioEncontrado);
                var CantidadMesaVirtual = NUsuario.CantidadMesaVirtual(UsuarioEncontrado);

                

                if (UsuarioEncontrado != null)
                {
                    Session["IDEmpresa"] = UsuarioEncontrado.Personal.IDEmpresa; //Pendiente falta terminar
                    Session["NombreUsuario"] = UsuarioEncontrado.NombreUsuario;
                    Session["NombreCompleto"] = string.Format("{0} {1}", FormatoNombre(UsuarioEncontrado.Personal.NombrePers), FormatoNombre(UsuarioEncontrado.Personal.ApellidoPersonal));
                    Session["CargoUsuario"] = FormatoNombre(UsuarioEncontrado.TipoUsuario.DescripcionConcepto);
                      
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
                    using (var NGeneral = new NGeneral())
                    {
                        var CargarParametros = NGeneral.CargaParametros(Convert.ToInt32(Session["IDEmpresa"]));

                        Session["PlazoDoctoElectronico"] = CargarParametros.PlazoDoctoElectronico;
                        Session["ExtensionPlazoDoctoElectronico"] = CargarParametros.ExtensionPlazoDoctoElectronico;
                        Session["AlertaDoctoElectronico"] = CargarParametros.AlertaDoctoElectronico;
                        Session["PlazoMesaVirtual"] = CargarParametros.PlazoMesaVirtual;
                        Session["ExtensionPlazoMesaVirtual"] = CargarParametros.ExtensionPlazoMesaVirtual;
                        Session["AlertaMesaVirtual"] = CargarParametros.AlertaMesaVirtual;
                        Session["AlertaMailLaboral"] = CargarParametros.AlertaMailLaboral;
                        Session["AlertaMailPersonal"] = CargarParametros.AlertaMailPersonal;
                        Session["HoraActualizaEstadoOperacion"] = CargarParametros.HoraActualizaEstadoOperacion;
                        Session["HoraCierreLabores"] = CargarParametros.HoraCierreLabores;
                        Session["PlazoExpiraFirma"] = CargarParametros.PlazoExpiraFirma;
                        Session["RutaGdocImagenes"] = CargarParametros.RutaGdocImagenes;
                        Session["RutaGdocPDF"] = CargarParametros.RutaGdocPDF;
                        Session["RutaGdocAdjuntos"] = CargarParametros.RutaGdocAdjuntos;
                        Session["RutaGdocExternos"] = CargarParametros.RutaGdocExternos;

                    }

                    return RedirectToAction("Index", "Alertas");
                }
                else
                    return RedirectToAction("Index", "Home");
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
            //CopiarFirma("","","");
            using (var oUsuario = new NUsuario())
            {
                Usuario respuesta = null;
                if (usuario.IDUsuario > 0){
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
        public JsonResult EliminarUsuario(Usuario usuario)
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
        [HttpPost]
        public JsonResult BuscarUsuarioNombre(Usuario usuario)
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
