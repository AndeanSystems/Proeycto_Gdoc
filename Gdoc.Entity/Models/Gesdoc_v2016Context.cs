using System.Data.Entity;
using Gdoc.Entity.Models.Mapping;

namespace Gdoc.Entity.Models
{
    public partial class Gesdoc_v2016Context : DbContext
    {
        static Gesdoc_v2016Context()
        {
            Database.SetInitializer<Gesdoc_v2016Context>(null);
        }

        public Gesdoc_v2016Context()
            : base("Name=Gesdoc_v2016Context")
        {
        }

        public DbSet<AccesoSistema> AccesoSistemas { get; set; }
        public DbSet<CentroCosto> CentroCostoes { get; set; }
        public DbSet<Concepto> Conceptoes { get; set; }
        public DbSet<DocumentoAdjunto> DocumentoAdjuntoes { get; set; }
        public DbSet<DocumentoDigitalOperacion> DocumentoDigitalOperacions { get; set; }
        public DbSet<DocumentoElectronicoOperacion> DocumentoElectronicoOperacions { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Folio> Folios { get; set; }
        public DbSet<General> Generals { get; set; }
        public DbSet<Grupo> Grupoes { get; set; }
        public DbSet<IndexacionDocumento> IndexacionDocumentoes { get; set; }
        public DbSet<LogOperacion> LogOperacions { get; set; }
        public DbSet<MensajeAlerta> MensajeAlertas { get; set; }
        public DbSet<MesaVirtualComentario> MesaVirtualComentarios { get; set; }
        public DbSet<ModuloPaginaUrl> ModuloPaginaUrls { get; set; }
        public DbSet<Operacion> Operacions { get; set; }
        public DbSet<Personal> Personals { get; set; }
        public DbSet<Sede> Sedes { get; set; }
        public DbSet<sysdiagram> sysdiagrams { get; set; }
        public DbSet<Ubigeo> Ubigeos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<UsuarioAutorizador> UsuarioAutorizadors { get; set; }
        public DbSet<UsuarioGrupo> UsuarioGrupoes { get; set; }
        public DbSet<UsuarioParticipante> UsuarioParticipantes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new AccesoSistemaMap());
            modelBuilder.Configurations.Add(new CentroCostoMap());
            modelBuilder.Configurations.Add(new ConceptoMap());
            modelBuilder.Configurations.Add(new DocumentoAdjuntoMap());
            modelBuilder.Configurations.Add(new DocumentoDigitalOperacionMap());
            modelBuilder.Configurations.Add(new DocumentoElectronicoOperacionMap());
            modelBuilder.Configurations.Add(new EmpresaMap());
            modelBuilder.Configurations.Add(new FolioMap());
            modelBuilder.Configurations.Add(new GeneralMap());
            modelBuilder.Configurations.Add(new GrupoMap());
            modelBuilder.Configurations.Add(new IndexacionDocumentoMap());
            modelBuilder.Configurations.Add(new LogOperacionMap());
            modelBuilder.Configurations.Add(new MensajeAlertaMap());
            modelBuilder.Configurations.Add(new MesaVirtualComentarioMap());
            modelBuilder.Configurations.Add(new ModuloPaginaUrlMap());
            modelBuilder.Configurations.Add(new OperacionMap());
            modelBuilder.Configurations.Add(new PersonalMap());
            modelBuilder.Configurations.Add(new SedeMap());
            modelBuilder.Configurations.Add(new sysdiagramMap());
            modelBuilder.Configurations.Add(new UbigeoMap());
            modelBuilder.Configurations.Add(new UsuarioMap());
            modelBuilder.Configurations.Add(new UsuarioAutorizadorMap());
            modelBuilder.Configurations.Add(new UsuarioGrupoMap());
            modelBuilder.Configurations.Add(new UsuarioParticipanteMap());
        }
    }
}
