using Gdoc.Entity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gdoc.Entity.Extension
{
    [NotMapped]
    public class EPersonal:Personal
    {
        public Concepto Estado { get; set; }
        public Concepto Cargo { get; set; }
        public Concepto Area { get; set; }
        public string Usuario { get; set; }
    }
}
