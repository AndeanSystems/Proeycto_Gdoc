using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gdoc.Entity.Models;
using Gdoc.Entity.Extension;

namespace Gdoc.Dao
{
    public class DEmpresa
    {
        public List<EEmpresa> ListarEmpresa()
        {
            var listEmpresa = new List<EEmpresa>();
            try
            {
                using (var db = new DataBaseContext())
                {
                    var list = db.Empresas.ToList();

                    var list2 = (from empresa in db.Empresas
                                 join estado in db.Conceptoes
                                 on empresa.EstadoEmpresa.ToString() equals estado.CodiConcepto

                                 where estado.TipoConcepto.Equals("016")

                                 select new { empresa, estado }).ToList();


                    list2.ForEach(x => listEmpresa.Add(new EEmpresa
                    {
                        IDEmpresa = x.empresa.IDEmpresa,
                        RucEmpresa = x.empresa.RucEmpresa,
                        RazonSocial = x.empresa.RazonSocial,
                        DireccionEmpresa = x.empresa.DireccionEmpresa,
                        TelefonoEmpresa = x.empresa.TelefonoEmpresa,
                        CodigoUbigeo = x.empresa.CodigoUbigeo,
                        EstadoEmpresa = x.empresa.EstadoEmpresa,
                        FechaRegistro = x.empresa.FechaRegistro,
                        UsuarioRegistro = x.empresa.UsuarioRegistro,
                        Estado = new Concepto{DescripcionConcepto=x.estado.DescripcionConcepto }

                    }));
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return listEmpresa;
        }
        public Empresa GrabarEmpresa(Empresa empresa)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    db.Empresas.Add(empresa);
                    db.SaveChanges();
                }
                return empresa;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public Empresa EditarEmpresa(Empresa empresa)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    var entidad = db.Empresas.Find(empresa.IDEmpresa);
                    entidad.RazonSocial = empresa.RazonSocial;
                    entidad.RucEmpresa = empresa.RucEmpresa;
                    entidad.TelefonoEmpresa = empresa.TelefonoEmpresa;
                    entidad.EstadoEmpresa = empresa.EstadoEmpresa;
                    empresa.CodigoUbigeo = empresa.CodigoUbigeo;
                    db.SaveChanges();
                }
                return empresa;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public Empresa EliminarEmpresa(Empresa empresa)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    var emp = db.Empresas.Find(empresa.IDEmpresa);
                    emp.EstadoEmpresa = empresa.EstadoEmpresa;
                    db.SaveChanges();
                }
                return empresa;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
