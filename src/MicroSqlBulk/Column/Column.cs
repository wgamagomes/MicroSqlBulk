using System.ComponentModel;

namespace MicroSqlBulk
{
    public class Column
    {
        public Column(string name, PropertyDescriptor propertyDescriptor)
        {
            Name = name;
            PropertyDescriptor = propertyDescriptor;
        }

        public Column(string name, PropertyDescriptor propertyDescriptor, bool isPrimaryKey)
            : this(name, propertyDescriptor)
        {
            IsPrimaryKey = isPrimaryKey;
        }

        public string Name { get; }
        public PropertyDescriptor PropertyDescriptor { get; }
        public bool IsPrimaryKey { get; }
    }
}
