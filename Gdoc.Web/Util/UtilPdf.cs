using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using iTextSharp.text.html.simpleparser;

namespace Gdoc.Web.Util
{
    public class UtilPdf
    {
        public void GenerarArchivoPDF(string sNumeroDocumentoElectronico, string sCarpetaOrigen, string sBodyTexto)
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
            document.Header = GenerarHeader(@"C:\GDOCWEB\", "FEPCMAC_Logo2.jpg");
            document.Footer = GenerarFooter(sFooter); ;

            document.Open();
            GenerarBody(ref document, sBodyTexto);
            document.Close();

            byte[] byteArray = ms.ToArray();


            ms.Flush();
            ms.Close();

            SubirArchivoFTP(sCarpetaOrigen, sNumeroDocumentoElectronico, ".pdf", byteArray);
        }
        protected HeaderFooter GenerarHeader(string sURLFolder, string sNameImagen)
        {
            iTextSharp.text.Image sFepcmac = iTextSharp.text.Image.GetInstance(string.Concat(sURLFolder, sNameImagen));
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
        protected string SubirArchivoFTP(string sNombreCarpeta, string sNombreArchivo, string sExtencionArchivo, byte[] sbyteContent)
        {
            string _MensajeError = string.Empty;

            if (sbyteContent.Length > 0)
            {
                try
                {
                    //Guardar en la ruta de pdf
                    string sRutaFTP = ConfigurationManager.AppSettings.Get("CarpetaTMP");

                    if (!Directory.Exists(sRutaFTP + sNombreCarpeta))
                        Directory.CreateDirectory(sRutaFTP + "/" + sNombreCarpeta);

                    if (!File.Exists(sRutaFTP + sNombreCarpeta + sNombreArchivo + sExtencionArchivo))
                        File.WriteAllBytes(sRutaFTP + "/" + sNombreCarpeta + "/" + sNombreArchivo + sExtencionArchivo, sbyteContent);
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