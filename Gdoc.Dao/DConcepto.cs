using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gdoc.Entity.Models;

namespace Gdoc.Dao
{
    public class DConcepto
    {
        public List<Concepto> ListarConcepto()
        {
            var listConcepto = new List<Concepto>();
            try
            {
                using (var db = new DataBaseContext())
                {
                    var list = db.Conceptoes.ToList();
                    list.ForEach(x => listConcepto.Add(new Concepto {
                        CodiConcepto = x.CodiConcepto,
                        DescripcionConcepto = x.DescripcionConcepto,
                        DescripcionCorta = x.DescripcionCorta,
                        EditarRegistro = x.EditarRegistro,
                        Empresa = new Empresa { },
                        EstadoConcepto = x.EstadoConcepto,
                        FechaModifica = x.FechaModifica,
                        IDEmpresa = x.IDEmpresa,
                        TextoDos = x.TextoDos,
                        TextoUno = x.TextoUno,
                        TipoConcepto = x.TipoConcepto,
                        UsuarioModifica = x.UsuarioModifica,
                        ValorDos = x.ValorDos,
                        ValorUno = x.ValorUno
                    }));
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return listConcepto;
        }
        public Concepto GrabarConcepto(Concepto concepto)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    db.Conceptoes.Add(concepto);
                    db.SaveChanges();
                }
                return concepto;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public Concepto EditarConcepto(Concepto concepto)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    var entidad = db.Conceptoes.Find(concepto.IDEmpresa,concepto.TipoConcepto,concepto.CodiConcepto);
                    entidad.TipoConcepto = concepto.TipoConcepto;
                    entidad.CodiConcepto = concepto.CodiConcepto;
                    entidad.DescripcionConcepto = concepto.DescripcionConcepto;
                    entidad.DescripcionCorta = concepto.DescripcionCorta;
                    entidad.ValorUno = concepto.ValorUno;
                    entidad.ValorDos = concepto.ValorDos;
                    entidad.TextoUno = concepto.TextoUno;
                    entidad.TextoDos = concepto.TextoDos;
                    entidad.UsuarioModifica = concepto.UsuarioModifica;
                    entidad.FechaModifica = concepto.FechaModifica;
                    entidad.EstadoConcepto = concepto.EstadoConcepto;
                    db.SaveChanges();
                }
                return concepto;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public Concepto EliminarConcepto(Concepto concepto)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    var con = db.Conceptoes.Find(concepto.IDEmpresa);
                    con.EstadoConcepto = concepto.EstadoConcepto;
                    db.SaveChanges();
                }
                return concepto;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
