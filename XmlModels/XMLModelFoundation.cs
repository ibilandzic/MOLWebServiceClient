using Microline.WS.Core;
using Microline.WS.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Microline.WS.XMLModel
{
    public abstract class XMLModelFoundation
    {
        /// <summary>
        /// Serializes xml model to string
        /// </summary>
        /// <returns></returns>
        public virtual string SerializeToString()
        {
            string xml = null;
            XmlSerializer xmlSerializer = new XmlSerializer(this.GetType());

            //System.Text.StringBuilder sb = new System.Text.StringBuilder();
            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, this);
                xml = textWriter.ToString();
            }

            return xml;
        }

        public virtual string SerializeToStringNoDeclaration()
        {
            string xml = null;
            XmlSerializer xmlSerializer = new XmlSerializer(this.GetType());
            var emptyNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            using (var stream = new StringWriter())
            {
                using (var writer = XmlWriter.Create(stream, settings))
                {
                    xmlSerializer.Serialize(writer, this, emptyNamespaces);
                    xml = stream.ToString();
                }
            }

            return xml;
        }

        /// <summary>
        /// Serilaizes xml model to byte array
        /// </summary>
        /// <returns></returns>
        public virtual byte[] SerializeToByteArray()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(this.GetType());
            MemoryStream memoryStream = new MemoryStream();
            xmlSerializer.Serialize(memoryStream, this);
            return memoryStream.ToArray();
        }

        /// <summary>
        /// serializes xml model to byte array with specific name space
        /// </summary>
        /// <param name="xmlSerializerNamespaces"></param>
        /// <returns></returns>
        public virtual byte[] SerializeToByteArray(XmlSerializerNamespaces xmlSerializerNamespaces)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(this.GetType());
            MemoryStream memoryStream = new MemoryStream();
            xmlSerializer.Serialize(memoryStream, this, xmlSerializerNamespaces);
            return memoryStream.ToArray();
        }

        /// <summary>
        /// Serialize object to xml file. Output file encoding is UTF-8
        /// </summary>
        /// <param name="fullFilePath"></param>
        public virtual void SerializeToFile(string fullFilePath, System.Text.Encoding encoding)
        {
            if (String.IsNullOrEmpty(fullFilePath)) throw new StringNotSetException("File path is not defined");
            XmlSerializer xmlSerializer = new XmlSerializer(this.GetType());
            StreamWriter file = null;
            try
            {
                file = new StreamWriter(fullFilePath, false, encoding);
                xmlSerializer.Serialize(file, this);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (file != null) file.Close();
            }
        }

        public virtual void SerializeToFile(string fullFilePath)
        {
            SerializeToFile(fullFilePath, new UTF8Encoding(false));//Encoding.UTF8
        }

        /// <summary>
        /// Serializes in DOM structure
        /// </summary>
        /// <returns></returns>
        public virtual XmlDocument SerializeToXmlDocument()
        {
            var document = new XmlDocument();
            var navigator = document.CreateNavigator();

            using (var writer = navigator.AppendChild())
            {
                var serializer = new XmlSerializer(this.GetType());
                serializer.Serialize(writer, this);
            }
            return document;
        }

        public virtual XmlDocument SerializeToXmlDocument(XmlSerializerNamespaces namespaces)
        {
            var document = new XmlDocument();
            var navigator = document.CreateNavigator();

            using (var writer = navigator.AppendChild())
            {
                var serializer = new XmlSerializer(this.GetType());
                serializer.Serialize(writer, this, namespaces);
            }
            return document;
        }

        public virtual XmlElement SerializeToXmlElement()
        {
            return this.SerializeToXmlDocument().DocumentElement;
        }

        #region Deserialiyation methods
        /// <summary>
        /// Deserializes from file. Full file path required
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <param name="languageKey"></param>
        /// <returns></returns>
        public static T DeserializeFromFile<T>(string fileName)
        {
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException(String.Format("File {0} not found", fileName));
            }
            else
            {
                string content = File.ReadAllText(fileName);
                if (String.IsNullOrEmpty(content)) throw new Exception("File has no content");
                else
                {
                    return DeserializeFromString<T>(content);
                }
            }
        }

        /// <summary>
        /// Deserialize from input string/content
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectData"></param>
        /// <returns></returns>
        public static T DeserializeFromString<T>(string objectData)
        {
            return (T)DeserializeFromString(objectData, typeof(T));
        }

        public static object DeserializeFromString(string objectData, System.Type type)
        {
            if (String.IsNullOrEmpty(objectData)) throw new Exception("Nothing to deserialize. Unsuccessfull attempt!");
            var serializer = new XmlSerializer(type);
            object result;

            using (TextReader reader = new StringReader(objectData))
            {
                result = serializer.Deserialize(reader);
            }

            return result;
        }


        public static T DeserializeFromXmlNode<T>(XmlNode element)
        {
            return (T)DeserializeFromXmlNode(element, typeof(T));
        }

        public static object DeserializeFromXmlNode(XmlNode element, System.Type type)
        {
            if (element != null)
            {
                var serializer = new XmlSerializer(type);
                object result;

                using (XmlReader reader = new XmlNodeReader(element))
                {
                    result = serializer.Deserialize(reader);
                }

                return result;
            }
            else throw new Exception("Nothing to deserialize");
        }

        #endregion
        public virtual string SerializeToXMLString(XmlSerializerNamespaces namespaces)
        {
            string xml = null;
            XmlSerializer xmlSerializer = new XmlSerializer(this.GetType());

            //System.Text.StringBuilder sb = new System.Text.StringBuilder();
            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, this, namespaces);
                xml = textWriter.ToString();
            }

            return xml;
        }


    }
}
