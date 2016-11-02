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
            string tipocomunicacion,
            List<string> listremitentes,
            string rutaFirma,
            List<string> listfirmaUsuario,
            string tipodoc)
        {
            string sFEPCMAC = ConfigurationManager.AppSettings.Get("FooterPDF1");
            string sDireccion = ConfigurationManager.AppSettings.Get("FooterPDF2");
            string sTelefono = ConfigurationManager.AppSettings.Get("FooterPDF3");
            string sWebSite = ConfigurationManager.AppSettings.Get("FooterPDF4");
            string[] sFooter = { sFEPCMAC, (sDireccion + sTelefono + sWebSite) };

            iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 90,90,50,50);

            MemoryStream ms = new MemoryStream();
            PdfWriter.GetInstance(document, ms);
            //Traer ruta de imagenes.. logos 
            document.Header = GenerarHeader(IDEmpresa, "FEPCMAC_Logo2.jpg", rutaPDF, tipodocumento, destinatario, remitente,  asunto);
            document.Open();
            if (tipodoc!="06")
                GenerarCabecera(ref document, tipodocumento, destinatario, remitente, asunto);
            GenerarBody(ref document, sBodyTexto);
            GenerarFirmas(ref document,listremitentes,rutaFirma,listfirmaUsuario);
            if (tipocomunicacion != "1")
                document.Footer = GenerarFooter(sFooter); ;
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
            
            

            //iTextSharp.text.Font fontTitulo = FontFactory.GetFont(FontFactory.HELVETICA, 14, iTextSharp.text.Font.BOLD, new iTextSharp.text.Color(System.Drawing.Color.Black));
            //iTextSharp.text.Font fonttabla = FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.NORMAL, new iTextSharp.text.Color(System.Drawing.Color.Black));
            
            ////tabla
            //float[] columnWidths = { 3, 17 };
            //PdfPTable tblPrueba = new PdfPTable(columnWidths);
            //tblPrueba.WidthPercentage = 100;

            //Chunk tipoDocumento = new Chunk(tipodocumento, fontTitulo);

            //PdfPCell cell = new PdfPCell(new Phrase(tipoDocumento));
            //cell.Colspan = 2;
            //cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            //cell.Border = 0;
            //cell.BorderWidthTop = 0.75f;
            //cell.PaddingTop = 20;
            //cell.PaddingBottom = 30;
            //tblPrueba.AddCell(cell);

            //PdfPCell space2 = new PdfPCell(new Phrase("Para", fonttabla));
            //space2.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            //space2.Border = 0;
            //tblPrueba.AddCell(space2);

            //PdfPCell destinatario = new PdfPCell(new Phrase(":" + destinatarios, fonttabla));
            //destinatario.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            //destinatario.Border = 0;
            //tblPrueba.AddCell(destinatario);

            //PdfPCell de = new PdfPCell(new Phrase("De", fonttabla));
            //de.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            //de.Border = 0;
            //tblPrueba.AddCell(de);

            //PdfPCell remitente = new PdfPCell(new Phrase(":" + remitentes, fonttabla));
            //remitente.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            //remitente.Border = 0;
            //tblPrueba.AddCell(remitente);

            //PdfPCell tres = new PdfPCell(new Phrase("Asunto", fonttabla));
            //tres.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            //tres.Border = 0;
            //tblPrueba.AddCell(tres);

            //PdfPCell asunto = new PdfPCell(new Phrase(":" + asuntos, fonttabla));
            //asunto.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            //asunto.Border = 0;
            //tblPrueba.AddCell(asunto);

            //PdfPCell fecha = new PdfPCell(new Phrase("Fecha", fonttabla));
            //fecha.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            //fecha.Border = 0;
            //fecha.PaddingBottom = 10;
            //fecha.BorderWidthBottom = 1.00f;
            //tblPrueba.AddCell(fecha);

            //PdfPCell hoy = new PdfPCell(new Phrase(":" + System.DateTime.Now.ToLongDateString(), fonttabla));
            //hoy.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            //hoy.Border = 0;
            //hoy.PaddingBottom = 10;
            //hoy.BorderWidthBottom = 1.00f;
            //tblPrueba.AddCell(hoy);

            sPhrase.Add(imagen);
            //sPhrase.Add(tblPrueba);
            sPhrase.Add("\n");

            HeaderFooter header = new HeaderFooter(sPhrase, false);
            header.Border = 0;

            return header;
        }
        protected HeaderFooter GenerarFooter(string[] sTexto)
        {
            iTextSharp.text.Font fontTexto1 = FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.BOLD, new iTextSharp.text.Color(System.Drawing.Color.Red));
            iTextSharp.text.Font fontTexto2 = FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.NORMAL, new iTextSharp.text.Color(System.Drawing.Color.Gray));
            Phrase sPhrase = new Phrase();

            Chunk chkTexto1 = new Chunk(@sTexto[0], fontTexto1);
            Chunk chkTexto2 = new Chunk(@sTexto[1], fontTexto2);

            Phrase final = new Phrase();
            final.Add(chkTexto1);
            final.Add("\n");
            final.Add(chkTexto2);

            sPhrase.Add(final); 

            HeaderFooter footer = new HeaderFooter(sPhrase, false);
            footer.SetAlignment("center");
            footer.Border = 0;
            return footer;
        }
        protected void GenerarFirmas(ref iTextSharp.text.Document document,List<string> listremitentes,string rutaFirma,List<string> listfirmaUsuario)
        {
            try
            {
                PdfPTable tabla2 = new PdfPTable(2);
                tabla2.WidthPercentage = 100;
                iTextSharp.text.Image sFirma = null;
                iTextSharp.text.Font fontdestinatario = FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.NORMAL, new iTextSharp.text.Color(System.Drawing.Color.Black));
                for (int i = 0; i <= 3; i++)
                {
                    if (listfirmaUsuario.Count() >= i + 1 && listfirmaUsuario[i] != null)
                    {
                        if (File.Exists(Path.Combine(rutaFirma, listfirmaUsuario[i])))
                            sFirma = iTextSharp.text.Image.GetInstance(Path.Combine(rutaFirma, listfirmaUsuario[i]));
                        else
                            sFirma = iTextSharp.text.Image.GetInstance(Path.Combine(rutaFirma, "sinfirma.jpg"));

                        sFirma.ScaleAbsolute(200, 100);
                        Chunk chkFirma = new Chunk(sFirma, 0, 0, true);

                        Chunk chkremitente = new Chunk(listremitentes[i], fontdestinatario);

                        Phrase firma1 = new Phrase();
                        firma1.Add(chkFirma);
                        firma1.Add("\n");
                        firma1.Add(chkremitente);

                        PdfPCell cell = new PdfPCell(firma1);
                        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        cell.Border = 0;
                        cell.PaddingTop = 5;//50

                        tabla2.AddCell(cell);
                    }
                    else
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(" "));
                        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        cell.Border = 0;
                        cell.PaddingTop = 50;
                        tabla2.AddCell(cell);
                    }

                }
                document.Add(tabla2);
            }
            catch (Exception)
            {
                
                throw;
            }
            
        }
        protected void GenerarCabecera(ref iTextSharp.text.Document document,string tipodocumento,string destinatarios,string remitentes, string asuntos)
        {
            iTextSharp.text.Font fontTitulo = FontFactory.GetFont(FontFactory.HELVETICA, 14, iTextSharp.text.Font.BOLD, new iTextSharp.text.Color(System.Drawing.Color.Black));
            iTextSharp.text.Font fonttabla = FontFactory.GetFont(FontFactory.HELVETICA, 10, iTextSharp.text.Font.NORMAL, new iTextSharp.text.Color(System.Drawing.Color.Black));

            //tabla
            float[] columnWidths = { 3, 17 };
            PdfPTable tblPrueba = new PdfPTable(columnWidths);
            tblPrueba.WidthPercentage = 100;

            Chunk tipoDocumento = new Chunk(tipodocumento, fontTitulo);

            PdfPCell cell = new PdfPCell(new Phrase(tipoDocumento));
            cell.Colspan = 2;
            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            cell.Border = 0;
            //cell.BorderWidthTop = 0.75f;
            //cell.PaddingTop = 20;
            cell.PaddingBottom = 5;//30
            tblPrueba.AddCell(cell);

            PdfPCell space2 = new PdfPCell(new Phrase("Para:", fonttabla));
            space2.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            space2.Border = 0;
            //space2.PaddingBottom = 20;
            tblPrueba.AddCell(space2);

            PdfPCell destinatario = new PdfPCell(new Phrase(destinatarios, fonttabla));
            destinatario.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            destinatario.Border = 0;
            //destinatario.PaddingBottom = 20;
            tblPrueba.AddCell(destinatario);

            PdfPCell de = new PdfPCell(new Phrase("De:", fonttabla));
            de.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            de.Border = 0;
            //de.PaddingBottom = 20;
            tblPrueba.AddCell(de);

            PdfPCell remitente = new PdfPCell(new Phrase(remitentes, fonttabla));
            remitente.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            remitente.Border = 0;
            //remitente.PaddingBottom = 20;
            tblPrueba.AddCell(remitente);

            PdfPCell tres = new PdfPCell(new Phrase("Asunto:", fonttabla));
            tres.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            tres.Border = 0;
            //tres.PaddingBottom = 20;
            tblPrueba.AddCell(tres);

            PdfPCell asunto = new PdfPCell(new Phrase(asuntos, fonttabla));
            asunto.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            asunto.Border = 0;
            //asunto.PaddingBottom = 20;
            tblPrueba.AddCell(asunto);

            PdfPCell fecha = new PdfPCell(new Phrase("Fecha:", fonttabla));
            fecha.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            fecha.Border = 0;
            //fecha.PaddingBottom = 20;
            //fecha.BorderWidthBottom = 1.00f;
            tblPrueba.AddCell(fecha);

            PdfPCell hoy = new PdfPCell(new Phrase(System.DateTime.Now.ToLongDateString(), fonttabla));
            hoy.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            hoy.Border = 0;
            //hoy.PaddingBottom = 20;
            //hoy.BorderWidthBottom = 1.00f;
            tblPrueba.AddCell(hoy);


            document.Add(tblPrueba);
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