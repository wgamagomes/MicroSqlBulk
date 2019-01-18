using System;
using System.ComponentModel;

namespace MicroSqlBulk.Helper
{
    public static class AttributeHelper
    {
        public static bool TryGetCustomAttribute<TAtt>(PropertyDescriptor prop, out TAtt attribute)
        where TAtt : Attribute
        {
            attribute = (TAtt)prop.Attributes[typeof(TAtt)];

            return attribute != null;
        }
    }
}
