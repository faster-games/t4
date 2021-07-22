using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;

namespace FasterGames.T4.Editor
{
    internal static class ReflectionUtils
    {
        public static IReadOnlyCollection<Type> ChildTypes(Type src)
        {
            List<Type> types = new List<Type>();

            AddMissingChildTypes(src, ref types);
            
            return types;
        }

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