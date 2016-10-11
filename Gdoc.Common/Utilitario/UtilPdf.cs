using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using iTextSharp.text.html.simpleparser;

namespace Gdoc.Common.Utilitario
{
    public class UtilPdf
    {

        public void GenerarArchivoPDF(string sNumeroDocumentoElectronico, 
            string sBodyTexto, 
            int IDEmpresa, 
            string rutaPDF,
            string tipodocumento,
            string destinatario,
            string remitente, 
            string asunto,
            string acceso,
            List<string> listremitentes,
            string rutaFirma,
            List<string> listfirmaUsuario)
        {
            string sFEPCMAC = ConfigurationManager.AppSettings.Get("FooterPDF1");
            string sDireccion = ConfigurationManager.AppSettings.Get("FooterPDF2");
            string sTelefono = ConfigurationManager.AppSettings.Get("FooterPDF3");
            string sWebSite = ConfigurationManager.AppSettings.Get("FooterPDF4");
            string[] sFooter = { sFEPCMAC, (sDireccion + sTelefono + sWebSite) };


            iTextSharp.text.Document descripcion = new iTextSharp.text.Document();
            //iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4);

            iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4);

            MemoryStream ms = new MemoryStream();
            PdfWriter.GetInstance(document, ms);
            //Traer ruta de imagenes.. logos 
            document.Header = GenerarHeader(IDEmpresa, "FEPCMAC_Logo2.jpg", rutaPDF, tipodocumento, destinatario, remitente,  asunto);
            if (acceso !="1")
            {
                document.Footer = GenerarFooter(sFooter, rutaFirma, listfirmaUsuario, listremitentes); ;
            }

            document.Open();
            GenerarBody(ref document, sBodyTexto);
            document.Close();

            byte[] byteArray = ms.ToArray();


            ms.Flush();
            ms.Close();

            SubirArchivoFTP(sNumeroDocumentoElectronico, ".pdf", byteArray, rutaPDF, IDEmpresa);
        }
        protected HeaderFooter GenerarHeader(int IDEmpresa, string sNameImagen,string logoRuta,string tipodocumento,string destinatarios,string remitentes, string asuntos)
        {
            iTextSharp.text.Image sFepcmac = iTextSharp.text.Image.GetInstance(Path.Combine(logoRuta, sNameImagen));
            sFepcmac.ScaleAbsolute(60, 70);

            //imagen
            Chunk chkLogoFepcmac = new Chunk(sFepcmac, -10, -10, true);
            
            Phrase sPhrase = new Phrase();

            Paragraph imagen = new Paragraph();
            imagen.Add(chkLogoFepcmac);
            imagen.Alignment = Element.ALIGN_LEFT;
            imagen.Add("\n");
            
            //Paragraph Titulo = new Paragraph();

            iTextSharp.text.Font fontTitulo = FontFactory.GetFont(FontFactory.HELVETICA, 14, iTextSharp.text.Font.BOLD, new iTextSharp.text.Color(System.Drawing.Color.Black));

            iTextSharp.text.Font fontTitulo2 = FontFactory.GetFont(FontFactory.HELVETICA, 14, iTextSharp.text.Font.UNDERLINE, new iTextSharp.text.Color(System.Drawing.Color.Black));
            //Chunk chkTexto1 = new Chunk("TITULO", fontTitulo);
            //Titulo.Add(chkTexto1);
            //Titulo.Alignment = Element.ALIGN_CENTER;

            //tabla
            PdfPTable tblPrueba = new PdfPTable(2);
            tblPrueba.WidthPercentage = 100;



            Chunk tipoDocumento = new Chunk(tipodocumento, fontTitulo);

            PdfPCell cell = new PdfPCell(new Phrase(tipoDocumento));
            cell.Colspan = 2;
            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            cell.Border = 0;
            cell.BorderWidthTop = 0.75f;
            cell.PaddingTop = 20;
            cell.PaddingBottom = 30;
            tblPrueba.AddCell(cell);

            PdfPCell space2 = new PdfPCell(new Phrase("Para"));
            space2.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            space2.Border = 0;
            //space2.Width = 20;
            tblPrueba.AddCell(space2);

            PdfPCell destinatario = new PdfPCell(new Phrase(":" + destinatarios));
            destinatario.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            destinatario.Border = 0;
            tblPrueba.AddCell(destinatario);

            PdfPCell de = new PdfPCell(new Phrase("De"));
            de.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            de.Border = 0;
            tblPrueba.AddCell(de);

            PdfPCell remitente = new PdfPCell(new Phrase(":" + remitentes));
            remitente.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            remitente.Border = 0;
            tblPrueba.AddCell(remitente);

            PdfPCell tres = new PdfPCell(new Phrase("Asunto"));
            tres.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            tres.Border = 0;
            tblPrueba.AddCell(tres);

            PdfPCell asunto = new PdfPCell(new Phrase(":" + asuntos));
            asunto.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            asunto.Border = 0;
            tblPrueba.AddCell(asunto);

            PdfPCell fecha = new PdfPCell(new Phrase("Fecha"));
            fecha.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            fecha.Border = 0;
            fecha.PaddingBottom = 10;
            fecha.BorderWidthBottom = 1.00f;
            tblPrueba.AddCell(fecha);

            PdfPCell hoy = new PdfPCell(new Phrase(":"+System.DateTime.Now.ToLongDateString()));
            hoy.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            hoy.Border = 0;
            hoy.PaddingBottom = 10;
            hoy.BorderWidthBottom = 1.00f;
            tblPrueba.AddCell(hoy);

            sPhrase.Add(imagen);
            //sPhrase.Add(Titulo);
            sPhrase.Add(tblPrueba);


            sPhrase.Add("\n");

            //HeaderFooter header = new HeaderFooter(new Phrase(chkLogoFepcmac), false);
            HeaderFooter header = new HeaderFooter(sPhrase, false);
            //header.Alignment = 2;
            //header.Alignment = Element.ALIGN_LEFT;
            header.Border = 0;
            //header.BorderWidthBottom = 0.75f;

            return header;
        }
        protected HeaderFooter GenerarFooter(string[] sTexto, string rutaFirma, List<string> nombreFirma, List<string> remitente)
        {
            iTextSharp.text.Font fontTexto1 = FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD, new iTextSharp.text.Color(System.Drawing.Color.Red));
            iTextSharp.text.Font fontTexto2 = FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.NORMAL, new iTextSharp.text.Color(System.Drawing.Color.Gray));
            iTextSharp.text.Font fontdestinatario = FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.NORMAL, new iTextSharp.text.Color(System.Drawing.Color.Black));

            Phrase sPhrase = new Phrase();
            foreach (var item in nombreFirma)
            {
                foreach (var rem in remitente)
                {
                    //firmas
                    iTextSharp.text.Image sFirma = iTextSharp.text.Image.GetInstance(Path.Combine(rutaFirma, item));
                    sFirma.ScaleAbsolute(100, 100);

                    //remitentes
                    Chunk chkFirma = new Chunk(sFirma, 0, 0, true);
                    Chunk chkremitente = new Chunk(rem, fontdestinatario);


                    Paragraph firma = new Paragraph();
                    firma.Add(chkFirma);
                    firma.Add("\n");
                    firma.Add(chkremitente);
                    //firma.Alignment = Element.ALIGN_LEFT;

                    

                    sPhrase.Add(firma);
                    
                }
                
            }

            Chunk chkTexto1 = new Chunk(@sTexto[0], fontTexto1);
            Chunk chkTexto2 = new Chunk(@sTexto[1], fontTexto2);

            Phrase final = new Phrase();
            final.Add(chkTexto1);
            final.Add("\n");
            final.Add(chkTexto2);

            sPhrase.Add("\n");
            sPhrase.Add(final); 

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
        protected string SubirArchivoFTP(string sNombreArchivo, string sExtencionArchivo, byte[] sbyteContent, string rutaPDF, int IDEmpresa)
        {
            string _MensajeError = string.Empty;

            if (sbyteContent.Length > 0)
            {
                try
                {

                    //if (!Directory.Exists(rutaPDF + sNombreCarpeta))
                    //    Directory.CreateDirectory(Path.Combine(rutaPDF, sNombreCarpeta));

                    //if (!File.Exists(Path.Combine(rutaPDF, sNombreCarpeta, string.Format("{0}{1}", sNombreArchivo, sExtencionArchivo))))
                    //    File.WriteAllBytes(Path.Combine(rutaPDF, sNombreCarpeta, string.Format("{0}{1}", sNombreArchivo, sExtencionArchivo)), sbyteContent);

                    if (!Directory.Exists(rutaPDF))
                        Directory.CreateDirectory(Path.Combine(rutaPDF));

                    if (!File.Exists(Path.Combine(rutaPDF, string.Format("{0}{1}", sNombreArchivo, sExtencionArchivo))))
                        File.WriteAllBytes(Path.Combine(rutaPDF, string.Format("{0}{1}", sNombreArchivo, sExtencionArchivo)), sbyteContent);
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