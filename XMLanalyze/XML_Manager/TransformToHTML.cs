
using System;
using System.IO;
using System.Xml;
using System.Xml.Xsl;

namespace XMLanalyze.XML_Manager
{
   
    public static class XmlTransformer
    {
        
        public static bool TransformXmlToHtml(string xmlPath, string xslPath, string outputHtmlPath)
        {
            try
            {
                var xslt = new XslCompiledTransform();

                xslt.Load(xslPath);

                using var writer = new StreamWriter(outputHtmlPath);
                xslt.Transform(xmlPath, null, writer);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка під час трансформації: {ex.Message}");
                return false;
            }
        }
    }
}
