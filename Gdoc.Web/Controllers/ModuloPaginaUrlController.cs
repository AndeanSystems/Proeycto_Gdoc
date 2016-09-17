using Gdoc.Entity.Extension;
using Gdoc.Entity.Models;
using Gdoc.Negocio;
using Gdoc.Web.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gdoc.Web.Controllers
{
    public class ModuloPaginaUrlController : Controller
    {
        #region "Variables"
        MensajeConfirmacion mensajeRespuesta = new MensajeConfirmacion();
        #endregion
        [HttpPost]
        public JsonResult ListarModuloPaginaUrl(Usuario eUsuario)
        {
            var listModuloPaginaUrl = new List<EModuloPaginaUrl>();
            using (var nModuloPaginaUrl = new NModuloPaginaUrl())
            {
                listModuloPaginaUrl = nModuloPaginaUrl.ObtenerPaginaModuloUrl(eUsuario);
            }
            return new JsonResult { Data = listModuloPaginaUrl, MaxJsonLength = int.MaxValue };
        }
	}
}