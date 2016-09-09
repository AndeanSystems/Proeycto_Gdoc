using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gdoc.Entity.Models;
using Gdoc.Entity.Extension;


namespace Gdoc.Dao
{
    public class DGeneral
    {
        public List<General> ListarGeneralParametros()
        {
            var listGeneralParametros = new List<General>();
            try
            {
                using (var db = new DataBaseContext())
                {

                    var list = db.Generals.ToList();

                    list.ForEach(x => listGeneralParametros.Add(new General
                    {
                        IDCodigoParametro=x.IDCodigoParametro,
                        IDEmpresa=x.IDEmpresa,
                        IDUsuario=x.IDUsuario,
                        PlazoDoctoElectronico=x.PlazoDoctoElectronico,
                        ExtensionPlazoDoctoElectronico=x.ExtensionPlazoDoctoElectronico,
                        AlertaDoctoElectronico=x.AlertaDoctoElectronico,
                        PlazoMesaVirtual=x.PlazoMesaVirtual,
                        ExtensionPlazoMesaVirtual=x.ExtensionPlazoMesaVirtual,
                        AlertaMesaVirtual=x.AlertaMesaVirtual,
                        AlertaMailLaboral=x.AlertaMailLaboral,
                        AlertaMailPersonal=x.AlertaMailPersonal,
                        HoraActualizaEstadoOperacion=x.HoraActualizaEstadoOperacion,
                        HoraCierreLabores=x.HoraCierreLabores,
                        PlazoExpiraFirma=x.PlazoExpiraFirma,
                        RutaGdocImagenes=x.RutaGdocImagenes,
                        RutaGdocPDF=x.RutaGdocPDF,
                        RutaGdocAdjuntos = x.RutaGdocAdjuntos,
                        RutaGdocExternos=x.RutaGdocExternos,

                    }));
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return listGeneralParametros;
        }
        public General GrabarGeneralParametros(General general)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    db.Generals.Add(general);
                    db.SaveChanges();
                }
                return general;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public General EditarGeneralParametros(General general)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    var entidad = db.Generals.Find(general.IDCodigoParametro);
                    entidad.PlazoDoctoElectronico = general.PlazoDoctoElectronico;
                    entidad.ExtensionPlazoDoctoElectronico = general.ExtensionPlazoDoctoElectronico;
                    entidad.AlertaDoctoElectronico = general.AlertaDoctoElectronico;
                    general.PlazoMesaVirtual = general.PlazoMesaVirtual;
                    entidad.ExtensionPlazoMesaVirtual = general.ExtensionPlazoMesaVirtual;
                    entidad.AlertaMesaVirtual = general.AlertaMesaVirtual;
                    entidad.AlertaMailLaboral = general.AlertaMailLaboral;
                    entidad.HoraActualizaEstadoOperacion = general.HoraActualizaEstadoOperacion;
                    entidad.HoraCierreLabores = general.HoraCierreLabores;
                    entidad.PlazoExpiraFirma = general.PlazoExpiraFirma;
                    entidad.RutaGdocImagenes = general.RutaGdocImagenes;
                    entidad.RutaGdocPDF = general.RutaGdocPDF;
                    entidad.RutaGdocAdjuntos = general.RutaGdocAdjuntos;
                    entidad.RutaGdocExternos = general.RutaGdocExternos;
                    db.SaveChanges();
                }
                return general;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public General CargaParametros(int IDEmpresa)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    var usu = (from gen in db.Generals
                               where gen.IDEmpresa == IDEmpresa
                               select new { gen }).FirstOrDefault();
                    return new General()
                    {
                        IDUsuario = usu.gen.IDUsuario,
                        PlazoDoctoElectronico= usu.gen.PlazoDoctoElectronico,
                        ExtensionPlazoDoctoElectronico= usu.gen.ExtensionPlazoDoctoElectronico,
                        AlertaDoctoElectronico= usu.gen.AlertaDoctoElectronico,
                        PlazoMesaVirtual= usu.gen.PlazoMesaVirtual,
                        ExtensionPlazoMesaVirtual= usu.gen.ExtensionPlazoMesaVirtual,
                        AlertaMesaVirtual= usu.gen.AlertaMesaVirtual,
                        AlertaMailLaboral= usu.gen.AlertaMailLaboral,
                        AlertaMailPersonal= usu.gen.AlertaMailPersonal,
                        HoraActualizaEstadoOperacion= usu.gen.HoraActualizaEstadoOperacion,
                        HoraCierreLabores= usu.gen.HoraCierreLabores,
                        PlazoExpiraFirma= usu.gen.PlazoExpiraFirma,
                        RutaGdocImagenes= usu.gen.RutaGdocImagenes,
                        RutaGdocPDF= usu.gen.RutaGdocPDF,
                        RutaGdocAdjuntos = usu.gen.RutaGdocAdjuntos,
                        RutaGdocExternos= usu.gen.RutaGdocExternos,


                    };
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
