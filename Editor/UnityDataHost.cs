using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FasterGames.T4.Runtime;
using Mono.TextTemplating;
using UnityEditor;
using Object = UnityEngine.Object;

namespace FasterGames.T4.Editor
{
    /// <summary>
    /// Template generator with data hosting support
    /// </summary>
    /// <typeparam name="TData">user data type</typeparam>
    public class UnityDataHost<TData> : TemplateGenerator, IDataHost<TData> where TData : Object
    {
        /// <summary>
        /// Creates an instance using a runtime type
        /// </summary>
        /// <param name="data">data to host</param>
        /// <param name="additionalRuntimeTypes">additional types to bring in</param>
        /// <returns>Instance of host</returns>
        public static TemplateGenerator CreateInstance(object data, List<string> additionalRuntimeTypes)
        {
            var dataHostType = typeof(UnityDataHost<>).MakeGenericType(new Type[] {data.GetType()});

            return (TemplateGenerator) Activator.CreateInstance(dataHostType, new object[]{ data, additionalRuntimeTypes });
        }

        /// <summary>
        /// Default ctor
        /// </summary>
        /// <remarks>
        /// If your <see cref="data"/> type pulls in non default-referenced types, you'll need to add them to <see cref="additionalRuntimeTypes"/>
        /// </remarks>
        /// <param name="data">data to host</param>
        /// <param name="additionalRuntimeTypes">additional types to bring in</param>
        public UnityDataHost(TData data, IEnumerable<string> additionalRuntimeTypes)
        {
            var dataType = data.GetType();
            
            Data = data;

            SafeAddRangeRef(new []{typeof(Object).Assembly.Location, typeof(IDataHost<>).Assembly.Location, dataType.Assembly.Location});
            SafeAddRangeImport(new []{typeof(Object).Namespace, typeof(IDataHost<>).Namespace, dataType.Namespace});

            foreach (var childType in ReflectionUtils.ChildTypes(dataType))
            {
                SafeAddRef(childType.Assembly.Location);
                SafeAddImport(childType.Namespace);
            }

            foreach (var additionalType in additionalRuntimeTypes)
            {
                var typ = Type.GetType(additionalType);
                SafeAddRef(typ?.Assembly.Location);
                SafeAddImport(typ?.Namespace);
            }
            
            var apiCompatLevel = PlayerSettings.GetApiCompatibilityLevel(EditorUserBuildSettings.selectedBuildTargetGroup);
            if (apiCompatLevel == ApiCompatibilityLevel.NET_Standard_2_0)
            {
                // always add netstandard - if you're in an editor only assembly it's a no-op, and you need it for non-editor dlls
                var netStandardPath = Assembly.Load(new AssemblyName("netstandard")).Location;
                SafeAddRef(netStandardPath);
            }
        }

        /// <inheritdoc />
        public override Type SpecificHostType => typeof(UnityDataHost<TData>);
        
        /// <inheritdoc />
        public TData Data { get; private set; }

        /// <summary>
        /// Adds a ref, only if it won't cause any issues to the template generator
        /// </summary>
        /// <param name="re">ref to add</param>
        private void SafeAddRef(string re)
        {
            if (string.IsNullOrWhiteSpace(re))
            {
                return;
            }
            
            Refs.Add(re);
        }

        /// <summary>
        /// Adds refs, only if it won't cause any issues to the template generator
        /// </summary>
        /// <param name="res">refs to add</param>
        private void SafeAddRangeRef(IEnumerable<string> res)
        {
            Refs.AddRange(res.SkipWhile(string.IsNullOrWhiteSpace));
        }
        
        /// <summary>
        /// Adds an import, only if it won't cause any issues to the template generator
        /// </summary>
        /// <param name="import">import to add</param>
        private void SafeAddImport(string import)
        {
            if (string.IsNullOrWhiteSpace(import))
            {
                return;
            }
            
            Imports.Add(import);
        }

        /// <summary>
        /// Adds imports, only if it won't cause any issues to the template generator
        /// </summary>
        /// <param name="imports">imports to add</param>
        private void SafeAddRangeImport(IEnumerable<string> imports)
        {
            Imports.AddRange(imports.SkipWhile(string.IsNullOrWhiteSpace));
        }
    }
}