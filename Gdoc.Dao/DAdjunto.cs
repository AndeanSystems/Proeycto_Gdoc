﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gdoc.Entity.Models;
using Gdoc.Entity.Extension;


namespace Gdoc.Dao
{
    public class DAdjunto
    {
        public Int32 GrabarAdjunto(Adjunto Adjunto)
        {
            try
            {
                using (var db = new DataBaseContext())
                {
                    db.Adjuntoes.Add(Adjunto);
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
