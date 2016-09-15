using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gdoc.Entity.Models;
using Gdoc.Entity.Extension;

namespace Gdoc.Dao
{
    public class DLogOperacion
    {
        public LogOperacion GrabarLogOperacion(LogOperacion logoperacion)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    db.LogOperacions.Add(logoperacion);
                    db.SaveChanges();
                }
                return logoperacion;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
