using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using iTextSharp.text.html.simpleparser;
using Gdoc.Dao;

namespace Gdoc.Common.Utilitario
{
    public class UtilPdf
    {
        private DGeneral dGeneral = new DGeneral();
        public void GenerarArchivoPDF(string sNumeroDocumentoElectronico, string sCarpetaOrigen, string sBodyTexto, int IDEmpresa, string rutaPDF)
        {
            string sFEPCMAC = ConfigurationManager.AppSettings.Get("FooterPDF1");
            string sDireccion = ConfigurationManager.AppSettings.Get("FooterPDF2");
            string sTelefono = ConfigurationManager.AppSettings.Get("FooterPDF3");
            string sWebSite = ConfigurationManager.AppSettings.Get("FooterPDF4");
            string[] sFooter = { sFEPCMAC, (sDireccion + sTelefono + sWebSite) };


            iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4);

            MemoryStream ms = new MemoryStream();
            PdfWriter.GetInstance(document, ms);
            //Traer ruta de imagenes.. logos 
            document.Header = GenerarHeader(IDEmpresa, "FEPCMAC_Logo2.jpg", rutaPDF);
            document.Footer = GenerarFooter(sFooter); ;

            document.Open();
            GenerarBody(ref document, sBodyTexto);
            document.Close();

            byte[] byteArray = ms.ToArray();


            ms.Flush();
            ms.Close();

            SubirArchivoFTP(sCarpetaOrigen, sNumeroDocumentoElectronico, ".pdf", byteArray, rutaPDF, IDEmpresa);
        }
        protected HeaderFooter GenerarHeader(int IDEmpresa, string sNameImagen,string logoRuta)
        {
            iTextSharp.text.Image sFepcmac = iTextSharp.text.Image.GetInstance(Path.Combine(logoRuta, sNameImagen));
            sFepcmac.ScaleAbsolute(60, 70);

            Chunk chkLogoFepcmac = new Chunk(sFepcmac, -10, -10, true);

            HeaderFooter header = new HeaderFooter(new Phrase(chkLogoFepcmac), false);
            header.Alignment = 2;
            header.Border = 0;

            return header;
        }
        protected HeaderFooter GenerarFooter(string[] sTexto)
        {
            iTextSharp.text.Font fontTexto1 = FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD, new iTextSharp.text.Color(System.Drawing.Color.Red));
            iTextSharp.text.Font fontTexto2 = FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.NORMAL, new iTextSharp.text.Color(System.Drawing.Color.Gray));

            Chunk chkTexto1 = new Chunk(@sTexto[0], fontTexto1);
            Chunk chkTexto2 = new Chunk(@sTexto[1], fontTexto2);

            Phrase sPhrase = new Phrase();
            sPhrase.Add(chkTexto1);
            sPhrase.Add("\n");
            sPhrase.Add(chkTexto2);

            HeaderFooter footer = new HeaderFooter(sPhrase, false);
            footer.SetAlignment("center");
            footer.Border = 0;
            return footer;
        }
        protected void GenerarBody(ref iTextSharp.text.Document sBody, string sBodyTexto)
        {
            HTMLWorker worker = new HTMLWorker(sBody);
            worker.StartDocument();
            worker.Parse(new StringReader(sBodyTexto));
            worker.EndDocument();
            worker.Close();
        }
        protected string SubirArchivoFTP(string sNombreCarpeta, string sNombreArchivo, string sExtencionArchivo, byte[] sbyteContent, string rutaPDF, int IDEmpresa)
        {
            string _MensajeError = string.Empty;

            if (sbyteContent.Length > 0)
            {
                try
                {

                    if (!Directory.Exists(rutaPDF + sNombreCarpeta))
                        Directory.CreateDirectory(Path.Combine(rutaPDF, sNombreCarpeta));

                    if (!File.Exists(Path.Combine(rutaPDF, sNombreCarpeta, string.Format("{0}{1}", sNombreArchivo, sExtencionArchivo))))
                        File.WriteAllBytes(Path.Combine(rutaPDF, sNombreCarpeta, string.Format("{0}{1}", sNombreArchivo, sExtencionArchivo)), sbyteContent);
                }
                catch (Exception ex)
                {
                    _MensajeError = ex.Message;
                }
            }

            return _MensajeError;
        }
    }
}