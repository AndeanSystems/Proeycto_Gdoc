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
    public class NMesaVirtualComentario : IDisposable
    {
        protected DGeneral dGeneral = new DGeneral();
        protected DLogOperacion dLogOperacion = new DLogOperacion();
        protected DAdjunto dAdjunto = new DAdjunto();
        protected DDocumentoAdjunto dDocumentoAdjunto = new DDocumentoAdjunto();
        protected DMesaVirtualComentario dMesaVirtualComentario = new DMesaVirtualComentario();

        protected string ArchivoImagen = "image";
        protected string ArchivoTXT = "text/plain";
        public void Dispose()
        {
            dMesaVirtualComentario = null;
        }
        public List<EMesaVirtualComentario> ListarMesaVirtualComentario()
        {
            try
            {
                return dMesaVirtualComentario.ListarMesaVirtualComentario();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public short GrabarMesaVirtualComentario(Operacion operacion, List<Adjunto> listAdjuntos, MesaVirtualComentario mesaVirtualComentario, Int64 IDUsuario)
        {
            try
            {
                var listDocumentoAdjunto = new List<DocumentoAdjunto>();
                var eDocumentoAdjunto = new DocumentoAdjunto();

                //GRABAR COMENTARIOMESAVIRTUAL
                dMesaVirtualComentario.GrabarMesaVirtualComentario(mesaVirtualComentario);

                var eGeneral = dGeneral.CargaParametros(operacion.IDEmpresa);
                if (!Directory.Exists(eGeneral.RutaGdocAdjuntos))
                {
                    Directory.CreateDirectory(eGeneral.RutaGdocAdjuntos);
                }
                //COPIAR ADJUNTO Y GRABAR
                if (listAdjuntos != null)
                {
                    foreach (var adjunto in listAdjuntos)
                    {
                        byte[] fileBytes = System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(adjunto.RutaArchivo);
                        adjunto.IDUsuario = IDUsuario;
                        adjunto.NombreOriginal = adjunto.NombreOriginal;
                        //documentoAdjunto.RutaArchivo = string.Format(@"{0}\{1}", eGeneral.RutaGdocAdjuntos, documentoAdjunto.NombreOriginal);
                        adjunto.RutaArchivo = string.Format(@"{0}\{1}_{2}", eGeneral.RutaGdocAdjuntos, operacion.NumeroOperacion, adjunto.NombreOriginal);
                        adjunto.TamanoArchivo = adjunto.TamanoArchivo;
                        adjunto.FechaRegistro = System.DateTime.Now;
                        adjunto.EstadoAdjunto = 1;
                        if (string.IsNullOrEmpty(adjunto.TipoArchivo) || !adjunto.TipoArchivo.Contains(ArchivoTXT))
                        {
                            File.WriteAllBytes(adjunto.RutaArchivo, fileBytes);
                        }
                        else if (adjunto.TipoArchivo.Contains(ArchivoTXT))
                        {
                            File.WriteAllText(adjunto.RutaArchivo, Encoding.UTF8.GetString(fileBytes));
                        }
                        else
                        {
                            using (MemoryStream stream = new MemoryStream(fileBytes))
                            {
                                Image.FromStream(stream).Save(adjunto.RutaArchivo, System.Drawing.Imaging.ImageFormat.Jpeg);
                            }
                        }

                        dAdjunto.GrabarAdjunto(adjunto);

                        //GRABAR DOCUMENTO ADJUNTO
                        eDocumentoAdjunto = new DocumentoAdjunto();
                        eDocumentoAdjunto.IDOperacion = operacion.IDOperacion;
                        eDocumentoAdjunto.IDAdjunto = adjunto.IDAdjunto;
                        eDocumentoAdjunto.IDComentarioMesaVirtual = mesaVirtualComentario.IDComentarioMesaVirtual;
                        eDocumentoAdjunto.EstadoDoctoAdjunto = 1;
                        dDocumentoAdjunto.GrabarDocumentoAdjunto(eDocumentoAdjunto);
                    }
                }
                return 1;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
