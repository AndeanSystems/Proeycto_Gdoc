using Gdoc.Entity.Extension;
using Gdoc.Negocio;
using Gdoc.Web.Util;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Gdoc.Web.Controllers
{
    public class ComboUsuarioGrupoController : Controller
    {
        #region "Variables"
        MensajeConfirmacion mensajeRespuesta = new MensajeConfirmacion();
        #endregion
        [HttpPost]
        public JsonResult ObtenerUsuarioGrupo(EUsuarioGrupo eUsuarioGrupo) {
            var retornoEUsuarioGrupo = new List<EUsuarioGrupo>();
            try
            {
                using (var oNEUsuarioGrupo = new NEUsuarioGrupo())
                {
                    retornoEUsuarioGrupo = oNEUsuarioGrupo.ObtenerUsuarioGrupo(eUsuarioGrupo);
                }
                return new JsonResult { Data = retornoEUsuarioGrupo, MaxJsonLength = Int32.MaxValue };
            }
            catch (Exception ex)
            {
                mensajeRespuesta.Exitoso = false;
                mensajeRespuesta.Mensaje = "Ocurrio un error";
                return new JsonResult { Data = mensajeRespuesta, MaxJsonLength = Int32.MaxValue };
            }
        }
    }
}