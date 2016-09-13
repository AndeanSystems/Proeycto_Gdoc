using Gdoc.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gdoc.Entity.Models;
using Gdoc.Entity.Extension;
using System.IO;
using System.Drawing;

namespace Gdoc.Negocio
{
    public class NUsuario : IDisposable
    {
        #region "Variable"
        private DUsuario dUsuario = new DUsuario();
        protected DGeneral dGeneral = new DGeneral();

        protected string ArchivoImagen = "image";
        protected string ArchivoTXT = "text/plain";
        public void Dispose()
        {
            dUsuario = null;
        }
        #endregion

        public EUsuario ValidarLogin(EUsuario usuario)
        {
            try
            {
                return dUsuario.ValidarLogin(usuario);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public EUsuario CantidadAlerta(EUsuario usuario)
        {
            try
            {
                return dUsuario.CantidadAlerta(usuario);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public EUsuario CantidadDocumentosRecibidos(EUsuario usuario)
        {
            try
            {
                return dUsuario.CantidadDocumentosRecibidos(usuario);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public EUsuario CantidadMesaVirtual(EUsuario usuario)
        {
            try
            {
                return dUsuario.CantidadMesaVirtual(usuario);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public List<EUsuario> ListarUsuario()
        {
            try
            {
                return dUsuario.ListarUsuario();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Usuario GrabarUsuario(Usuario usuario)
        {
            try
            {
                return dUsuario.GrabarUsuario(usuario);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Usuario EditarUsuario(Usuario usuario)
        {
            try
            {
                return dUsuario.EditarUsuario(usuario);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Usuario EliminarUsuario(Usuario usuario)
        {
            try
            {
                return dUsuario.EliminarUsuario(usuario);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public short MoverFirma(List<EFirma> listFirmas)
        {
            try
            {
                var eGeneral = dGeneral.CargaParametros(1001);
                if (!Directory.Exists(eGeneral.RutaGdocAdjuntos))
                {
                    Directory.CreateDirectory(eGeneral.RutaGdocAdjuntos);
                }
                foreach (var documentoOperacion in listFirmas)
                {
                    byte[] fileBytes = System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(documentoOperacion.RutaFisica);
                    documentoOperacion.RutaFisica = string.Format(@"{0}\{1}", eGeneral.RutaGdocImagenes, documentoOperacion.NombreOriginal);

                    documentoOperacion.NombreFisico = string.Empty;
                    documentoOperacion.TamanoDocto = documentoOperacion.TamanoDocto;
                    if (string.IsNullOrEmpty(documentoOperacion.TipoArchivo) || !documentoOperacion.TipoArchivo.Contains(ArchivoTXT))
                    {
                        File.WriteAllBytes(documentoOperacion.RutaFisica, fileBytes);
                    }
                    else if (documentoOperacion.TipoArchivo.Contains(ArchivoTXT))
                    {
                        File.WriteAllText(documentoOperacion.RutaFisica, Encoding.UTF8.GetString(fileBytes));
                    }
                    else
                    {
                        using (MemoryStream stream = new MemoryStream(fileBytes))
                        {
                            Image.FromStream(stream).Save(documentoOperacion.RutaFisica, System.Drawing.Imaging.ImageFormat.Jpeg);
                        }
                    }
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
