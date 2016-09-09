using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gdoc.Entity.Models;
using Gdoc.Entity.Extension;

namespace Gdoc.Dao
{
    public class DPersonal 
    {
        public List<Personal> ListarPersonal()
        {
            var listPersonal = new List<Personal>();
            try
            {
                using (var db = new DataBaseContext())
                {
                    var list = db.Personals.ToList();

                    var list2 = (from empresa in db.Empresas
                                 join estado in db.Conceptoes
                                 on empresa.EstadoEmpresa.ToString() equals estado.CodiConcepto

                                 where estado.TipoConcepto.Equals("016")

                                 select new { empresa, estado }).ToList();


                    list.ForEach(x => listPersonal.Add(new Personal
                    {
                        IDPersonal=x.IDPersonal,
                        IDEmpresa = x.IDEmpresa,
                        IDSede=x.IDSede,
                        CodigoPersonal=x.CodigoPersonal,
                        NombrePers=x.NombrePers,
                        ApellidoPersonal=x.ApellidoPersonal,
                        SexoPersonal=x.SexoPersonal,
                        EmailPersonal=x.EmailPersonal,
                        EmailTrabrajo=x.EmailTrabrajo,
                        FechaNacimiento=x.FechaNacimiento,
                        TelefonoPersonal=x.TelefonoPersonal,
                        AnexoPersonal=x.AnexoPersonal,
                        EstadoPersonal=x.EstadoPersonal,
                        CodigoArea=x.CodigoArea,
                        CodigoCargo=x.CodigoCargo,
                        ClasePersonal=x.ClasePersonal,
                        NumeroIdentificacion=x.NumeroIdentificacion,
                        DireccionPersonal=x.DireccionPersonal,
                        CodigoUbigeo=x.CodigoUbigeo,
                        CelularPersonalUno=x.CelularPersonalUno,
                        CelularPersonalDos = x.CelularPersonalDos,
                        TipoIdentificacion=x.TipoIdentificacion,

                    }));
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return listPersonal;
        }
        public Personal GrabarPersonal(Personal personal)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    db.Personals.Add(personal);
                    db.SaveChanges();
                }
                return personal;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public Personal EditarPersonal(Personal personal)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    var entidad = db.Personals.Find(personal.IDPersonal);
                    entidad.NombrePers= personal.NombrePers;
                    entidad.ApellidoPersonal= personal.ApellidoPersonal;
                    entidad.SexoPersonal= personal.SexoPersonal;
                    entidad.EmailPersonal= personal.EmailPersonal;
                    entidad.EmailTrabrajo= personal.EmailTrabrajo;
                    entidad.FechaNacimiento= personal.FechaNacimiento;
                    entidad.TelefonoPersonal= personal.TelefonoPersonal;
                    entidad.AnexoPersonal= personal.AnexoPersonal;
                    entidad.EstadoPersonal= personal.EstadoPersonal;
                    entidad.CodigoArea= personal.CodigoArea;
                    entidad.CodigoCargo= personal.CodigoCargo;
                    entidad.ClasePersonal= personal.ClasePersonal;
                    entidad.NumeroIdentificacion= personal.NumeroIdentificacion;
                    entidad.DireccionPersonal= personal.DireccionPersonal;
                    entidad.CodigoUbigeo= personal.CodigoUbigeo;
                    entidad.CelularPersonalUno= personal.CelularPersonalUno;
                    entidad.CelularPersonalDos= personal.CelularPersonalDos;
                    entidad.TipoIdentificacion = personal.TipoIdentificacion;
                    db.SaveChanges();
                }
                return personal;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
}
