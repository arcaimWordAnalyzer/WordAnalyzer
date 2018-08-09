using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using System.Xml.Schema;
using WordAnalyzer.Core.Domain;
using WordAnalyzer.Core.Repositories;
using WordAnalyzer.Infrastructure.Sorts;

namespace WordAnalyzer.Infrastructure.Converters
{
    public class XmlConverter : Converter
    {
        protected override async Task<string> CreateStructureAsync(IEnumerable<Sentence> sentences)
        {
            var header = new XDeclaration(version: "1.0", encoding: "utf-8", standalone: "yes");
            var doc = new XDocument( // xml -> xsd, xmlsoap -> wsdl, xslt (transformata)
                new XElement("text",
                        from sentence in sentences
                        select new XElement("sentence",
                            from word in sentence.Words
                            select new XElement("word", word)//SecurityElement.Escape(word).Replace("\'", "&apos;"))
                        )));

            var text = doc.ToString()
                          .Replace("&amp;", "&amp;amp;")
                          .Replace("&lt;", "&amp;lt;")
                          .Replace("&gt;", "&amp;gt;")
                          .Replace("\'", "&amp;apos;")
                          .Replace("\"", "&amp;quot;");

            var xml = new StringWriterUtf8();
            await xml.WriteAsync(header.ToString() + "\r\n" + text);

            var ooo = xml.ToString();
            
            return await Task.FromResult(ooo);
        }

        private class StringWriterUtf8 : StringWriter
        { 
            public override Encoding Encoding
            {
                get { return Encoding.UTF8; }
            }
        }
    }
}