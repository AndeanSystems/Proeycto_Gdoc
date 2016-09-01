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
    }
}
