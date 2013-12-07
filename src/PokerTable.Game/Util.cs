using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace PokerTable.Game
{
    internal static class Util
    {
        internal static string Serialize<T>(T objectToSerialize)
        {
            var sb = new StringBuilder();
            var serializer = new XmlSerializer(typeof(T));
            var writer = new StringWriter(sb);
            serializer.Serialize(writer, objectToSerialize);
            writer.Close();
            return sb.ToString();
        }

        internal static T DeSerialize<T>(string objectToDeSerialize)
            where T : new()
        {
            if (string.IsNullOrEmpty(objectToDeSerialize))
            {
                return new T();
            }

            var read = new StringReader(objectToDeSerialize);
            var serializer = new XmlSerializer(typeof(T));
            XmlReader reader = new XmlTextReader(read);
            return (T)serializer.Deserialize(reader);
        }
    }
}
