

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;
using log4net;

namespace ServerHost
{
    /// <summary>
    /// A custom serializer that logs all serialization errors. It is a simple wrapper around the default WCF
    /// <see cref="DataContractSerializer" />.
    /// </summary>
    public class LoggingSerializer : XmlObjectSerializer
    {
        /// <summary>
        /// The log.
        /// </summary>
        private readonly ILog log = LogManager.GetLogger(typeof(LoggingSerializer));

        /// <summary>
        /// The underlying serializer.
        /// </summary>
        private readonly DataContractSerializer serializer;

        /// <summary>
        /// The type that will be serialized or deserialized.
        /// </summary>
        private readonly Type type;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingSerializer" /> class.
        /// </summary>
        /// <param name="type">The type that will be serialized or deserialized.</param>
        /// <param name="rootName">
        /// The name of the XML element that encloses the content to serialize or
        /// deserialize.
        /// </param>
        /// <param name="rootNamespace">The XML namespace of <paramref name="rootName" />.</param>
        /// <param name="knownTypes">The collection of types that may be present in the object graph.</param>
        /// <param name="maxItemsInObjectGraph">The maximum number of items in the object graph to serialize or deserialize.</param>
        /// <param name="ignoreExtensionDataObject">
        /// <c>true</c>, to ignore the data supplied by an extension of the
        /// <paramref name="type" /> upon serialization or deserialization; otherwise, <c>false</c>.
        /// </param>
        /// <param name="preserveObjectReferences">
        /// <c>true</c>, to use non-standard XML constructs to preserve object reference
        /// data; otherwise, <c>false</c>.
        /// </param>
        /// <param name="dataContractSurrogate">The data contract surrogate used to customize the serialization process.</param>
        /// <param name="dataContractResolver">The data contract resolver used to map xsi:type declarations to data contract types.</param>
        public LoggingSerializer(
            Type type,
            string rootName,
            string rootNamespace,
            IEnumerable<Type> knownTypes,
            int maxItemsInObjectGraph,
            bool ignoreExtensionDataObject,
            bool preserveObjectReferences,
            IDataContractSurrogate dataContractSurrogate,
            DataContractResolver dataContractResolver)
        {
            this.log.DebugFormat("Creating new serializer for type '{0}'.", type.FullName);

            this.type = type;
            this.serializer = new DataContractSerializer(
                type,
                rootName,
                rootNamespace,
                knownTypes,
                maxItemsInObjectGraph,
                ignoreExtensionDataObject,
                preserveObjectReferences,
                dataContractSurrogate,
                dataContractResolver);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingSerializer" /> class.
        /// </summary>
        /// <param name="type">The type that will be serialized or deserialized.</param>
        /// <param name="rootName"> The name of the XML element that encloses the content to serialize or deserialize.</param>
        /// <param name="rootNamespace">The XML namespace of <paramref name="rootName" />.</param>
        /// <param name="knownTypes">The collection of types that may be present in the object graph.</param>
        /// <param name="maxItemsInObjectGraph">The maximum number of items in the object graph to serialize or deserialize.</param>
        /// <param name="ignoreExtensionDataObject">
        /// <c>true</c>, to ignore the data supplied by an extension of the
        /// <paramref name="type" /> upon serialization or deserialization; otherwise, <c>false</c>.
        /// </param>
        /// <param name="preserveObjectReferences">
        /// <c>true</c>, to use non-standard XML constructs to preserve object reference
        /// data; otherwise, <c>false</c>.
        /// </param>
        /// <param name="dataContractSurrogate">The data contract surrogate used to customize the serialization process.</param>
        /// <param name="dataContractResolver">The data contract resolver used to map xsi:type declarations to data contract types.</param>
        public LoggingSerializer(
            Type type,
            XmlDictionaryString rootName,
            XmlDictionaryString rootNamespace,
            IEnumerable<Type> knownTypes,
            int maxItemsInObjectGraph,
            bool ignoreExtensionDataObject,
            bool preserveObjectReferences,
            IDataContractSurrogate dataContractSurrogate,
            DataContractResolver dataContractResolver)
        {
            this.log.DebugFormat("Creating new serializer for type '{0}'.", type.FullName);

            this.type = type;
            this.serializer = new DataContractSerializer(
                type,
                rootName,
                rootNamespace,
                knownTypes,
                maxItemsInObjectGraph,
                ignoreExtensionDataObject,
                preserveObjectReferences,
                dataContractSurrogate,
                dataContractResolver);
        }

        /// <summary>
        /// Gets a value that specifies whether the specified reader is positioned over an XML element that can be read.
        /// </summary>
        /// <param name="reader"> An <see cref="T:System.Xml.XmlDictionaryReader" /> used to read the XML stream or document.</param>
        /// <returns>
        /// <c>true</c>, if the specified reader can read the data; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsStartObject(XmlDictionaryReader reader)
        {
            return this.serializer.IsStartObject(reader);
        }

        /// <summary>
        /// Reads the XML stream or document with the specified reader and returns the deserialized object.
        /// </summary>
        /// <param name="reader">An <see cref="T:System.Xml.XmlDictionaryReader" /> used to read the XML document.</param>
        /// <param name="verifyObjectName">
        /// <c>true</c>, if the enclosing XML element name and namespace should be checked to see if they correspond to the root
        /// name and root namespace; otherwise, <c>false</c>.
        /// </param>
        /// <returns>
        /// The deserialized object.
        /// </returns>
        public override object ReadObject(XmlDictionaryReader reader, bool verifyObjectName)
        {
            try
            {
                return this.serializer.ReadObject(reader, verifyObjectName);
            }
            catch (Exception e)
            {
                this.log.Error($"Reading object for type, '{this.type.FullName}', threw an exception.", e);

                throw;
            }
        }

        /// <summary>
        /// Writes the end of the object data as a closing XML element to the XML document or stream with the specified writer.
        /// </summary>
        /// <param name="writer">An <see cref="T:System.Xml.XmlDictionaryWriter" /> used to write the XML document or stream.</param>
        public override void WriteEndObject(XmlDictionaryWriter writer)
        {
            try
            {
                this.serializer.WriteEndObject(writer);
            }
            catch (Exception e)
            {
                this.log.Error($"Writing end object for type, '{this.type.FullName}', threw an exception.", e);

                throw;
            }
        }

        /// <summary>
        /// Writes only the content of the object to the XML document or stream using the specified writer.
        /// </summary>
        /// <param name="writer">An <see cref="T:System.Xml.XmlDictionaryWriter" /> used to write the XML document or stream.</param>
        /// <param name="graph">The object that contains the content to write.</param>
        public override void WriteObjectContent(XmlDictionaryWriter writer, object graph)
        {
            try
            {
                this.serializer.WriteObjectContent(writer, graph);
            }
            catch (Exception e)
            {
                this.log.Error($"Writing object content for type, '{this.type.FullName}', threw an exception.", e);

                throw;
            }
        }

        /// <summary>
        /// Writes the start of the object's data as an opening XML element using the specified writer.
        /// </summary>
        /// <param name="writer">An <see cref="T:System.Xml.XmlDictionaryWriter" /> used to write the XML document.</param>
        /// <param name="graph">The object to serialize.</param>
        public override void WriteStartObject(XmlDictionaryWriter writer, object graph)
        {
            try
            {
                this.serializer.WriteStartObject(writer, graph);
            }
            catch (Exception e)
            {
                this.log.Error($"Writing start object for type, '{this.type.FullName}', threw an exception.", e);

                throw;
            }
        }
    }
}