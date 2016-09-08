namespace Gdoc.Entity.Models
{
    public partial class DocumentoElectronicoOperacion
    {
        public long IDDoctoElectronicoOperacion { get; set; }
        public long IDOperacion { get; set; }
        public string Memo { get; set; }
        public virtual Operacion Operacion { get; set; }
    }
}
