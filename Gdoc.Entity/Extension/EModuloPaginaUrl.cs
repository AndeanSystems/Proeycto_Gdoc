using Gdoc.Entity.Models;
using System.ComponentModel.DataAnnotations.Schema;


namespace Gdoc.Entity.Extension
{
    [NotMapped]
    public class EModuloPaginaUrl : ModuloPaginaUrl
    {
        public string Asignacion { get; set; }
    }
}
