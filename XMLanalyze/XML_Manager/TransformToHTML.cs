// LabWork2/XML_Manager/XmlTransformer.cs

using System;
using System.IO;
using System.Xml;
using System.Xml.Xsl;

namespace XMLanalyze.XML_Manager
{
    /// <summary>
    /// Клас для трансформації XML в HTML за допомогою XSL
    /// </summary>
    public static class XmlTransformer
    {
        /// <summary>
        /// Виконує трансформацію XML-файлу в HTML з використанням XSL-шаблону
        /// </summary>
        /// <param name="xmlPath">Шлях до вхідного XML-файлу</param>
        /// <param name="xslPath">Шлях до XSL-шаблону</param>
        /// <param name="outputHtmlPath">Шлях для збереження згенерованого HTML-файлу</param>
        /// <returns>True, якщо трансформація успішна, інакше False</returns>
        public static bool TransformXmlToHtml(string xmlPath, string xslPath, string outputHtmlPath)
        {
            try
            {
                var xslt = new XslCompiledTransform();

                // Завантажуємо XSL-шаблон
                xslt.Load(xslPath);

                // Виконуємо трансформацію XML -> HTML і записуємо в outputHtmlPath
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
