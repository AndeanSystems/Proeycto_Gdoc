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
    }
}
