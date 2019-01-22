using FluentAssertions;
using MicroSqlBulk.Attributes;
using MicroSqlBulk.Helper;
using NUnit.Framework;
using System;

namespace MicroSqlBulk.Test.Script
{
    public class ScriptTest
    {

        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        public void ShouldBeReturnedTheTempTableCreationScript()
        {
            TableHelper
                 .GenerateLocalTempTableScript<TableDummy>()
                 .Should()
                 .Be(
@"CREATE TABLE #TABLE_DUMMY_TEMP
(
	 ID BIGINT,
	 DESCRIPTION NVARCHAR(MAX),
	 DATE DATETIME,
	 FLAG BIT,
	 MONEY DECIMAL(18,0)
)
");
        }
    }

    [Table("TABLE_DUMMY")]
    public class TableDummy
    {
        [Column("ID", true)]
        public int Id { get; set; }

        [Column("DESCRIPTION")]
        public string Description { get; set; }

        [Column("DATE")]
        public DateTime Date { get; set; }

        [Column("FLAG")]
        public bool Flag { get; set; }

        [Column("MONEY")]
        public decimal Money { get; set; }
    }
}
