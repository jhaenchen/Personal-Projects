using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;

namespace XMLFormTesting
{
    class Program
    {
        static XmlDocument doc = new XmlDocument();
        static XDocument xmlDoc = XDocument.Load("TestXMLTemplate.xml");
        static void Main(string[] args)
        {
            
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);
            
            
            
            while (true)
            {
                Console.WriteLine("Type \"fill\" to fill out the form. \"print\" to export it.");
                string input = Console.ReadLine();
                switch (input)
                {
                    case "fill":
                        Console.Clear();
                        fillForm();
                        Console.Clear();
                        break;
                    case "print":
                        Console.Clear();
                        Print();
                        Console.Clear();
                        System.Diagnostics.Process.Start("result.html");
                        
                        break;
                }
            }
        }

        static void Print()
        {
            var myXslTrans = new XslCompiledTransform();
            myXslTrans.Load("transform.xsl");
            myXslTrans.Transform("TestXMLTemplate.xml", "result.html");
            
        }
        static void fillForm()
        {
            IEnumerable<XElement> sectionList = from section in xmlDoc.Root.Descendants("section") select section;
            foreach (XElement element in sectionList)
            {
                Console.Clear();
                Console.WriteLine("Section: " + element.Attribute("name").Value);
                IEnumerable<XElement> fieldList2 = from v in element.Descendants() select v;
                foreach (var xElement in fieldList2)
                {
                    
                    Console.WriteLine(xElement.Attribute("name").Value.Trim() + ": ");
                    xElement.Value = Console.ReadLine();
                }
            }
            
            
            
            
            
            xmlDoc.Save("TestXMLTemplate.xml");
        }
        static XmlNode addToSection(string sectionName, string fieldName, string data)
        {
            if (doc.SelectSingleNode(sectionName) == null)
            {
                Console.WriteLine("The {0} section does not yet exist. Generating...", sectionName);
                XmlNode newSectionNode = doc.CreateElement(sectionName);
                doc.AppendChild(newSectionNode);
            }
            XmlNode sectionNode = doc.SelectSingleNode(sectionName);
            XmlNode newField = doc.CreateElement(fieldName);
            Console.WriteLine("Created new field: {0}", fieldName);
            newField.InnerText = data;
            return newField;
        }
    }
}
