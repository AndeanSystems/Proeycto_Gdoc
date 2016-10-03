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
    public class EAdjunto:Adjunto
    {
        public DocumentoAdjunto DocumentoAdjunto { get; set; }
    }
}
