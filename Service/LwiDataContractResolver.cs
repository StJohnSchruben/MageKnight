

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Xml;

namespace Service
{
    /// <summary>
    /// A custom data contract resolver that scans all the declared types in one or more assemblies and adds them to a list of
    /// known types for the service.
    /// </summary>
    public class LwiDataContractResolver : DataContractResolver
    {
        /// <summary>
        /// The default namespace for types without a namespace.
        /// </summary>
        private const string DefaultNamespace = "global";

        /// <summary>
        /// The names to type map.
        /// </summary>
        private readonly Dictionary<string, Dictionary<string, Type>> namesToTypeMap =
            new Dictionary<string, Dictionary<string, Type>>();

        /// <summary>
        /// The type to names map.
        /// </summary>
        private readonly Dictionary<Type, Tuple<string, string>> typeToNamesMap =
            new Dictionary<Type, Tuple<string, string>>();

        /// <summary>
        /// The dictionary
        /// </summary>
        private readonly XmlDictionary dictionary;

        /// <summary>
        /// Initializes a new instance of the <see cref="LwiDataContractResolver" /> class.
        /// </summary>
        /// <param name="assemblyNames">The names of the assemblies to scan for known types.</param>
        public LwiDataContractResolver(params string[] assemblyNames)
        {
            this.dictionary = new XmlDictionary();
            var knownTypes = GetAssemblyKnownTypes(assemblyNames);

            foreach (var knownType in knownTypes)
            {
                this.AddKnownType(knownType.Namespace ?? DefaultNamespace, knownType.Name, knownType);
            }
        }

        /// <summary>
        /// Override this method to map the specified xsi:type name and namespace to a data contract type during deserialization.
        /// </summary>
        /// <param name="typeName">The xsi:type name to map.</param>
        /// <param name="typeNamespace">The xsi:type namespace to map.</param>
        /// <param name="declaredType">The type declared in the data contract.</param>
        /// <param name="knownTypeResolver">The known type resolver.</param>
        /// <returns>
        /// The type the xsi:type name and namespace is mapped to.
        /// </returns>
        public override Type ResolveName(
            string typeName,
            string typeNamespace,
            Type declaredType,
            DataContractResolver knownTypeResolver)
        {
            if (this.namesToTypeMap.ContainsKey(typeNamespace))
            {
                if (this.namesToTypeMap[typeNamespace].ContainsKey(typeName))
                {
                    return this.namesToTypeMap[typeNamespace][typeName];
                }
            }

            return knownTypeResolver.ResolveName(typeName, typeNamespace, declaredType, null);
        }

        /// <summary>
        /// Override this method to map a data contract type to an xsi:type name and namespace during serialization.
        /// </summary>
        /// <param name="type">The type to map.</param>
        /// <param name="declaredType">The type declared in the data contract.</param>
        /// <param name="knownTypeResolver">The known type resolver.</param>
        /// <param name="typeName">The xsi:type name.</param>
        /// <param name="typeNamespace">The xsi:type namespace.</param>
        /// <returns>
        /// <c>true</c>, if mapping succeeded; otherwise, <c>false</c>.
        /// </returns>
        public override bool TryResolveType(
            Type type,
            Type declaredType,
            DataContractResolver knownTypeResolver,
            out XmlDictionaryString typeName,
            out XmlDictionaryString typeNamespace)
        {
            if (this.typeToNamesMap.ContainsKey(type))
            {
                typeNamespace = this.dictionary.Add(this.typeToNamesMap[type].Item1);
                typeName = this.dictionary.Add(this.typeToNamesMap[type].Item2);

                return true;
            }

            return knownTypeResolver.TryResolveType(type, declaredType, null, out typeName, out typeNamespace);
        }

        /// <summary>
        /// Gets the known types from the specified assembly names.
        /// </summary>
        /// <param name="assemblyNames">The names of the assemblies to scan for known types.</param>
        /// <returns>
        /// A collection of known types.
        /// </returns>
        private static IEnumerable<Type> GetAssemblyKnownTypes(IEnumerable<string> assemblyNames)
        {
            var result = new List<Type>();

            foreach (var assemblyName in assemblyNames)
            {
                var assembly = Assembly.Load(new AssemblyName(assemblyName));

                result.AddRange(GetAssemblyKnownTypes(assembly));
            }

            return result;
        }

        /// <summary>
        /// Gets the known types from the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>
        /// A collection of known types.
        /// </returns>
        private static IEnumerable<Type> GetAssemblyKnownTypes(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (!type.IsPublic)
                {
                    // Filter out non-public types
                    continue;
                }

                if (!type.IsClass && (!type.IsValueType || type.IsEnum || type.IsPrimitive))
                {
                    // Filter out everything except classes and structs
                    continue;
                }

                if (type.IsAbstract && type.IsSealed)
                {
                    // Filter out static classes
                    continue;
                }

                if (type.Namespace == null ||
                    Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute), false) ||
                    type.Name.Contains("AnonymousType") ||
                    type.Name.StartsWith("<>"))
                {
                    // Filter out anonymous types
                    continue;
                }

                yield return type;
            }
        }

        /// <summary>
        /// Adds a known type to the collection of known types.
        /// </summary>
        /// <param name="typeNamespace">The xsi:type namespace to map.</param>
        /// <param name="typeName">The xsi:type name to map.</param>
        /// <param name="type">The type to map.</param>
        private void AddKnownType(string typeNamespace, string typeName, Type type)
        {
            if (!this.typeToNamesMap.ContainsKey(type))
            {
                this.typeToNamesMap.Add(type, new Tuple<string, string>(typeNamespace, typeName));
            }

            if (!this.namesToTypeMap.ContainsKey(typeNamespace))
            {
                this.namesToTypeMap.Add(
                    typeNamespace,
                    new Dictionary<string, Type>
                    {
                        { typeName, type }
                    });
            }
            else if (!this.namesToTypeMap[typeNamespace].ContainsKey(typeName))
            {
                this.namesToTypeMap[typeNamespace].Add(typeName, type);
            }
        }
    }
}