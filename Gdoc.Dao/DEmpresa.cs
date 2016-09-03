using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gdoc.Entity.Models;

namespace Gdoc.Dao
{
    public class DEmpresa
    {
        public List<Empresa> ListarEmpresa()
        {
            var listEmpresa = new List<Empresa>();
            try
            {
                using (var db = new DataBaseContext())
                {
                    var list = db.Empresas.ToList();
                    list.ForEach(x => listEmpresa.Add(new Empresa
                    {
                        IDEmpresa = x.IDEmpresa,
                        RucEmpresa = x.RucEmpresa,
                        RazonSocial = x.RazonSocial,
                        DireccionEmpresa  = x.DireccionEmpresa,
                        TelefonoEmpresa = x.TelefonoEmpresa,
                        CodigoUbigeo = x.CodigoUbigeo,
                        EstadoEmpresa = x.EstadoEmpresa,
                        FechaRegistro=x.FechaRegistro,
                        UsuarioRegistro=x.UsuarioRegistro
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
                    //empresa.EstadoEmpresa = empresa.EstadoEmpresa;
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
