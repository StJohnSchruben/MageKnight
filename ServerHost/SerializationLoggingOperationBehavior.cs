

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Xml;

namespace ServerHost
{
    /// <summary>
    /// An operation behavior that replaces the default WCF serializer with the <see cref="LoggingSerializer" />.
    /// </summary>
    public class SerializationLoggingOperationBehavior : DataContractSerializerOperationBehavior
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SerializationLoggingOperationBehavior" /> class.
        /// </summary>
        /// <param name="operation">The operation being examined.</param>
        public SerializationLoggingOperationBehavior(OperationDescription operation)
            : base(operation)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializationLoggingOperationBehavior" /> class.
        /// </summary>
        /// <param name="operation">The operation being examined.</param>
        /// <param name="dataContractFormatAttribute">
        /// The run-time object that exposes customization properties for the operation described by <paramref name="operation" />.
        /// </param>
        public SerializationLoggingOperationBehavior(
            OperationDescription operation,
            DataContractFormatAttribute dataContractFormatAttribute)
            : base(operation, dataContractFormatAttribute)
        {
        }

        /// <summary>
        /// Creates an XML serializer for the specified type.
        /// </summary>
        /// <param name="type">The type to serialize or deserialize.</param>
        /// <param name="name">The name of the serialized type.</param>
        /// <param name="ns">The namespace of the serialized type.</param>
        /// <param name="knownTypes">The collection of known types.</param>
        /// <returns>
        /// The XML serializer for the specified type.
        /// </returns>
        public override XmlObjectSerializer CreateSerializer(
            Type type,
            XmlDictionaryString name,
            XmlDictionaryString ns,
            IList<Type> knownTypes)
        {
            return new LoggingSerializer(
                type,
                name,
                ns,
                knownTypes,
                this.MaxItemsInObjectGraph,
                this.IgnoreExtensionDataObject,
                false,
                this.DataContractSurrogate,
                this.DataContractResolver);
        }

        /// <summary>
        /// Creates an XML serializer for the specified type.
        /// </summary>
        /// <param name="type">The type to serialize or deserialize.</param>
        /// <param name="name">The name of the serialized type.</param>
        /// <param name="ns">The namespace of the serialized type.</param>
        /// <param name="knownTypes">The collection of known types.</param>
        /// <returns>
        /// The XML serializer for the specified type.
        /// </returns>
        public override XmlObjectSerializer CreateSerializer(Type type, string name, string ns, IList<Type> knownTypes)
        {
            return new LoggingSerializer(
                type,
                name,
                ns,
                knownTypes,
                this.MaxItemsInObjectGraph,
                this.IgnoreExtensionDataObject,
                false,
                this.DataContractSurrogate,
                this.DataContractResolver);
        }
    }
}