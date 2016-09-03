using System;
using Gdoc.Entity.Models;

namespace Gdoc.Dao
{
    public class DOperacion
    {
        public short Grabar(Operacion operacion)
        {
            try
            {

                using (var db = new DataBaseContext())
                {
                    db.Operacions.Add(operacion);
                    db.SaveChanges();
                }
                return 1;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
