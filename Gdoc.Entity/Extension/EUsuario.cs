using Gdoc.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gdoc.Entity.Extension
{
    [NotMapped]
    public class EUsuario:Usuario
    {
        public Concepto Cargo { get; set; }
        public Concepto TipoUsuario { get; set; }
        public Concepto Area { get; set; }
        public Concepto ClaseUsu { get; set; }
    }
}
