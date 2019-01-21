using System;

namespace MicroSqlBulk.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnAttribute : Attribute
    {
        public ColumnAttribute(string name, bool isPrimaryKey = false)
        {
            Name = name;
            IsPrimaryKey = isPrimaryKey;
        }

        public string Name { get; }
        public bool IsPrimaryKey { get; set; }
    }
}
