using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace PersonalOffice.Backend.Application.Common.Global
{
    internal class XmlHelper
    {
        public static byte[] SerializeObject(object Obj)
        {
            var xmlSerializer = new XmlSerializer(Obj.GetType());
            XmlSerializerNamespaces EmptyNameSpaces = new XmlSerializerNamespaces([XmlQualifiedName.Empty]);
            byte[] Content;
            using var MS = new MemoryStream();
            using var XmlWrite = XmlWriter.Create(MS, new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                ConformanceLevel = ConformanceLevel.Auto,
                Indent = true
            });

            xmlSerializer.Serialize(XmlWrite, Obj, EmptyNameSpaces);
            Content = MS.ToArray();
            XmlWrite.Close();

            MS.Close();
            return Content;
        }

        public static T DeserializeObject<T>(string XmlContent)
        {
            byte[] bytes = Encoding.UTF32.GetBytes(XmlContent);
            return DeserializeObject<T>(bytes);
        }

        public static T DeserializeObject<T>(byte[] XmlContent)
        {
            var a1 = Encoding.UTF8.GetString(XmlContent);
            T Obj;
            var xmlSerializer = new XmlSerializer(typeof(T));
            using (var MS = new MemoryStream(XmlContent))
            {
                using (XmlReader XmlRead = XmlReader.Create(MS))
                {
                    Obj = (T)xmlSerializer.Deserialize(XmlRead);
                    XmlRead.Close();
                }
                MS.Close();
            }
            return Obj;
        }
    }
}
