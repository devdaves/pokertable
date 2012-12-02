using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace PokerTable.Game
{
    /// <summary>
    /// Utilities
    /// </summary>
    internal static class Util
    {
        /// <summary>
        /// Serializes the specified object to serialize.
        /// </summary>
        /// <typeparam name="T">type to be serialized</typeparam>
        /// <param name="objectToSerialize">The object to serialize.</param>
        /// <returns>string representation of object</returns>
        internal static string Serialize<T>(T objectToSerialize)
        {
            StringBuilder sb = new StringBuilder();
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            StringWriter writer = new StringWriter(sb);
            serializer.Serialize(writer, objectToSerialize);
            writer.Close();
            return sb.ToString();
        }

        /// <summary>
        /// Deserialize the object
        /// </summary>
        /// <typeparam name="T">type to be deserialized</typeparam>
        /// <param name="objectToDeSerialize">The object to de serialize.</param>
        /// <returns>returns the deserialized object</returns>
        internal static T DeSerialize<T>(string objectToDeSerialize)
            where T : new()
        {
            if (string.IsNullOrEmpty(objectToDeSerialize))
            {
                return new T();
            }

            StringReader read = new StringReader(objectToDeSerialize);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            XmlReader reader = new XmlTextReader(read);
            return (T)serializer.Deserialize(reader);            
        }
    }
}
