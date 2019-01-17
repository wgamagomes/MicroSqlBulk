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

        public string Name { get; }
        public PropertyDescriptor PropertyDescriptor { get; }
    }
}
