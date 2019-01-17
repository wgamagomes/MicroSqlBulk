using System;

namespace MicroSqlBulk.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute : Attribute
    {
        public TableAttribute(string name, string schema = "")
        {
            Name = name;
            Schema = schema;
        }

        public string Name { get; }
        public string Schema { get; }
    }
}
