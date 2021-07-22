using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;

namespace FasterGames.T4.Editor
{
    /// <summary>
    /// Internal reflection utils
    /// </summary>
    internal static class ReflectionUtils
    {
        /// <summary>
        /// Get the child types from a src type
        /// </summary>
        /// <param name="src">src type</param>
        /// <returns>child types</returns>
        public static IReadOnlyCollection<Type> ChildTypes(Type src)
        {
            List<Type> types = new List<Type>();

            AddMissingChildTypes(src, ref types);
            
            return types;
        }

        /// <summary>
        /// Internal helper to add missing child types to a list
        /// </summary>
        /// <param name="src">src type</param>
        /// <param name="types">existing types</param>
        private static void AddMissingChildTypes(Type src, ref List<Type> types)
        {
            var fieldTypes = src.GetFields().Select(f => f.FieldType);
            var propTypes = src.GetProperties().Select(p => p.PropertyType);
            
            var baseTypes = new List<Type>();
            if (src.BaseType != null)
            {
                baseTypes.Add(src.BaseType);
            }
            
            var coreTypes = fieldTypes.Union(propTypes)/*.Union(attrTypes)*/.Union(baseTypes);

            foreach (var type in coreTypes)
            {
                if (!types.Contains(type) && type != null)
                {
                    types.Add(type);
                    AddMissingChildTypes(type, ref types);
                }
            }
        }
    }
}