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
        public List<EPersonal> ListarPersonal()
        {
            var listPersonal = new List<EPersonal>();
            try
            {
                using (var db = new DataBaseContext())
                {
                    var list = db.Personals.ToList();

                    var list2 = (from personal in db.Personals
                                 join area in db.Conceptoes
                                 on personal.CodigoArea.ToString() equals area.CodiConcepto

                                 join cargo in db.Conceptoes
                                 on personal.CodigoCargo.ToString() equals cargo.CodiConcepto

                                 where area.TipoConcepto.Equals("013") &&
                                        cargo.TipoConcepto.Equals("007")

                                 select new { personal, area,cargo }).ToList();


                    list2.ForEach(x => listPersonal.Add(new EPersonal
                    {
                        IDPersonal = x.personal.IDPersonal,
                        IDEmpresa = x.personal.IDEmpresa,
                        IDSede = x.personal.IDSede,
                        CodigoPersonal = x.personal.CodigoPersonal,
                        NombrePers = x.personal.NombrePers,
                        ApellidoPersonal = x.personal.ApellidoPersonal,
                        SexoPersonal = x.personal.SexoPersonal,
                        EmailPersonal = x.personal.EmailPersonal,
                        EmailTrabrajo = x.personal.EmailTrabrajo,
                        FechaNacimiento = x.personal.FechaNacimiento,
                        TelefonoPersonal = x.personal.TelefonoPersonal,
                        AnexoPersonal = x.personal.AnexoPersonal,
                        EstadoPersonal = x.personal.EstadoPersonal,
                        CodigoArea = x.personal.CodigoArea,
                        CodigoCargo = x.personal.CodigoCargo,
                        ClasePersonal = x.personal.ClasePersonal,
                        NumeroIdentificacion = x.personal.NumeroIdentificacion,
                        DireccionPersonal = x.personal.DireccionPersonal,
                        CodigoUbigeo = x.personal.CodigoUbigeo,
                        CelularPersonalUno = x.personal.CelularPersonalUno,
                        CelularPersonalDos = x.personal.CelularPersonalDos,
                        TipoIdentificacion = x.personal.TipoIdentificacion,
                        Cargo = new Concepto { DescripcionConcepto = x.cargo.DescripcionConcepto },
                        Area = new Concepto { DescripcionConcepto = x.area.DescripcionConcepto },

                        //Usuario = x.personal.EmailTrabrajo.Substring(0, x.personal.EmailTrabrajo.IndexOf("@")),

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
