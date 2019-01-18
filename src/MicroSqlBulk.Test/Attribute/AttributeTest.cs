using MicroSqlBulk.Attributes;
using MicroSqlBulk.Helper;
using NUnit.Framework;
using System.ComponentModel;
using IgnoreAttribute = MicroSqlBulk.Attributes.IgnoreAttribute;
namespace MicroSqlBulk.Test.Attribute
{

    public class AttributeTest
    {
        private PropertyDescriptorCollection props;

        [SetUp]
        public void SetUp()
        {
            props = TypeDescriptor.GetProperties(typeof(Foo));
        }

        [Test]
        public void ShouldBeTrueForColumnAttribute()
        {
            Assert.AreEqual(true, AttributeHelper.TryGetCustomAttribute(props[0], out ColumnAttribute columnAttribute));
        }

        [Test]
        public void ShouldBeTrueForIgnoreAttribute()
        {
            Assert.AreEqual(true, AttributeHelper.TryGetCustomAttribute(props[2], out IgnoreAttribute columnAttribute));
        }

        [Test]
        public void ShouldBeFalseForIgnoreAttribute()
        {
            Assert.AreEqual(false, AttributeHelper.TryGetCustomAttribute(props[1], out IgnoreAttribute columnAttribute));
        }
    }

    [Table("FOO", "BAR")]
    class Foo
    {
        [Column("Id")]
        public int Id { get; set; }

        public int Xpto { get; set; }

        [Ignore]
        public bool Ignored { get; set; }
    }
}
