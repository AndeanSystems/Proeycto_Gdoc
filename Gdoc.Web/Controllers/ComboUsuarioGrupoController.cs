﻿using Gdoc.Entity.Extension;
using Gdoc.Negocio;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using Gdoc.Common.Utilitario;

namespace Gdoc.Web.Controllers
{
    public class ComboUsuarioGrupoController : Controller
    {
        #region "Variables"
        MensajeConfirmacion mensajeRespuesta = new MensajeConfirmacion();
        #endregion
        public JsonResult ObtenerUsuarioGrupo(EUsuarioGrupo eUsuarioGrupo) {
            var retornoEUsuarioGrupo = new List<EUsuarioGrupo>();
            try
            {
                using (var oNEUsuarioGrupo = new NEUsuarioGrupo())
                {
                    if (string.IsNullOrEmpty(eUsuarioGrupo.Tipo))
                        retornoEUsuarioGrupo = oNEUsuarioGrupo.ObtenerUsuarioGrupo(eUsuarioGrupo);
                    else
                        retornoEUsuarioGrupo = oNEUsuarioGrupo.ObtenerUsuarioGrupo(eUsuarioGrupo).Where(x => x.Tipo == eUsuarioGrupo.Tipo).ToList();
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