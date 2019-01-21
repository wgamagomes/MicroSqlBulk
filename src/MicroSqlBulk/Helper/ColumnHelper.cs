using MicroSqlBulk.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace MicroSqlBulk.Helper
{
    public static class ColumnHelper
    {
        public static IList<Column> GetFildesInfo<TEntity>()
        {
            IList<Column> columns;
            bool containsPrimaryKey = false;

            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(TEntity));

            columns = new List<Column>();

            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];

                if (AttributeHelper.TryGetCustomAttribute(prop, out ColumnAttribute columnAttribute))
                {
                    if(columnAttribute.IsPrimaryKey && containsPrimaryKey)
                        throw new InvalidOperationException($"This '{prop.Name}' can't set as primary key, because the primary key already field exists in the table configuration manager.");
                    containsPrimaryKey = columnAttribute.IsPrimaryKey;
                    columns.Add(new Column((columnAttribute).Name, prop));
                }
                else
                {
                    if (!AttributeHelper.TryGetCustomAttribute(prop, out IgnoreAttribute ignoreAttribute))
                    {
                        throw new InvalidOperationException($"The '{prop.Name}' property should be configured through the '{nameof(ColumnAttribute)}' or it should be ignored on the POCO through the '{nameof(IgnoreAttribute)}'.");
                    }
                }
            }

            return columns;
        }
    }
}
