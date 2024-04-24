using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Xps.Packaging;
using System.Xml;

namespace EEMC.ToXPSConverteres
{
    public class TxtConverter : IXPSConvert
    {
        private string AdaptForXml(string str)
        {
            str = str.Replace("&", "&amp;");
            str = str.Replace("\"", "&quot;");
            //str = str.Replace("\'", "&apos;");
            str = str.Replace("<", "&lt;");
            str = str.Replace(">", "&gt;");
         
            return str;
        }

        private string GetTxtTemplate()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var a = assembly.GetManifestResourceNames();
            var resourceName = "EEMC.Resources.XmlTemplates.TxtTemplate.xml";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();

                return result;
            }
        }

        private string GetTxtTextBlock()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var a = assembly.GetManifestResourceNames();
            var resourceName = "EEMC.Resources.XmlTemplates.TxtTextBlock.xml";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();

                return result;
            }
        }

        private byte[] GetTmrFont()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var a = assembly.GetManifestResourceNames();
            var resourceName = "EEMC.Resources.Fonts.tmr.ttf";

            byte[] result;

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (BinaryReader br = new BinaryReader(stream))
            {
                result = br.ReadBytes((int)stream.Length);
            }

            return result;
        }

        public async Task<XpsDocument> ToXpsConvertAsync(string originFileName, string xpsFileName, CancellationToken? cancellationToken = null)
        {
            return await Task<XpsDocument>.Run(() =>
                {
                    if (File.Exists(xpsFileName))
                        File.Delete(xpsFileName);

                    XpsDocument xd = new XpsDocument(xpsFileName, FileAccess.ReadWrite);

                    IXpsFixedDocumentSequenceWriter xdSW = xd.AddFixedDocumentSequence();

                    IXpsFixedDocumentWriter xdW = xdSW.AddFixedDocument();

                    IXpsFixedPageWriter xpW = xdW.AddFixedPage();

                    XpsFont font = xpW.AddFont(false);
                    string fontUri = font.Uri.ToString();
                    using (Stream stream = font.GetStream())
                    {
                        byte[] fontBin = GetTmrFont();
                        stream.Write(fontBin, 0, fontBin.Length);
                    }
                    font.Commit();

                    string[] pageContents = File.ReadAllLines(originFileName).Select(x => AdaptForXml(x)).ToArray();
                    XmlWriter xmlWriter = xpW.XmlWriter;
                    string textBlock = GetTxtTextBlock();
                    string txtTemplate = GetTxtTemplate();

                    StringBuilder totalTextBlocks = new StringBuilder();

                    foreach (string line in pageContents)
                    {
                        string contentInXml = textBlock.Replace("TEXT_FROM_TXT", line);
                        contentInXml = contentInXml.Replace("FONT_URI", fontUri);

                        totalTextBlocks.AppendLine(contentInXml);
                    }

                    xmlWriter.WriteRaw(
                        txtTemplate.Replace("TEXT_BLOCKS", totalTextBlocks.ToString())
                    );

                    xpW.Commit();
                    xmlWriter.Close();

                    xdW.Commit();
                    xdSW.Commit();

                    return xd;
                },
                cancellationToken ?? new CancellationToken()
            );
        }
    }
}
