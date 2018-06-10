using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Zabrodin.SavePass.Extensions
{
    public static class XmlSerialization
    {
        public static T DeserializeObject<T>(string xml) where T : class => DeserializeObject<T>(xml, Encoding.UTF8);

        public static string SerializeObject(object obj, bool withFormatting = true)
            => SerializeObject(obj, Encoding.UTF8, withFormatting);

        public static T DeserializeObject<T>(string xml, Encoding encoding)
            where T : class
        {
            if (String.IsNullOrWhiteSpace(xml))
                return default(T);

            try
            {
                using (MemoryStream memoryStream = new MemoryStream(encoding.GetBytes(xml)))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                    return xmlSerializer.Deserialize(memoryStream) as T;
                }
            }

            catch
            {
                return default(T);
            }
        }

        public static string SerializeObject(object obj, Encoding encoding, bool withFormatting = false)
        {
            try
            {
                StringBuilder result = new StringBuilder();
                XmlWriterSettings settings = new XmlWriterSettings
                {
                    OmitXmlDeclaration = true,
                    NewLineHandling = NewLineHandling.Entitize,
                    Indent = withFormatting,
                    Encoding = encoding
                };
                XmlSerializer xmlSerializer = new XmlSerializer(obj.GetType());

                XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                namespaces.Add(String.Empty, String.Empty);

                using (XmlWriter writer = XmlWriter.Create(result, settings))
                {
                    xmlSerializer.Serialize(writer, obj, namespaces);
                    return result.ToString();
                }
            }
            catch
            {
                return String.Empty;
            }
        }
    }
}