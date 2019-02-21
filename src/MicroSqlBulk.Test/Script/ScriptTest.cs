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
        public void FromSourceColumnsToTargetColumnsTest()
        {
            TableHelper
                .FromSourceColumnsToTargetColumns<TableDummy>()
                .Should()
                .Be("TABLE_DUMMY.DESCRIPTION = #TABLE_DUMMY_TEMP.DESCRIPTION,TABLE_DUMMY.DATE = #TABLE_DUMMY_TEMP.DATE,TABLE_DUMMY.FLAG = #TABLE_DUMMY_TEMP.FLAG,TABLE_DUMMY.MONEY = #TABLE_DUMMY_TEMP.MONEY,TABLE_DUMMY.STATUS = #TABLE_DUMMY_TEMP.STATUS");
        }

        [Test]
        public void ShouldSetAPrefixForeachColumnWithTheTableName()
        {
            TableHelper
                .SetThePrefixInTheColumns<TableDummy>()
                .Should()
                .Be("TABLE_DUMMY.DESCRIPTION,TABLE_DUMMY.DATE,TABLE_DUMMY.FLAG,TABLE_DUMMY.MONEY,TABLE_DUMMY.STATUS");
        }

        [Test]
        public void ShouldSetAPrefixForeachColumnWithTheTempTableName()
        {
            TableHelper
                .SetThePrefixInTheColumns<TableDummy>(true)
                .Should()
                .Be("#TABLE_DUMMY_TEMP.DESCRIPTION,#TABLE_DUMMY_TEMP.DATE,#TABLE_DUMMY_TEMP.FLAG,#TABLE_DUMMY_TEMP.MONEY,#TABLE_DUMMY_TEMP.STATUS");
        }

        [Test]
        public void ShouldReturnConcatenatedColumnsSeparatedByCommas()
        {
            TableHelper
                .ConcatenateColumns<TableDummy>()
                .Should()
                .Be("DESCRIPTION,DATE,FLAG,MONEY,STATUS");
        }

        [Test]
        public void TheTemporaryTableCreationScriptShouldBeReturnedFromTheEntity()
        {
            TableHelper
                .GetCreateTableScript<TableDummy>(true)
                .Should()
                .Be("CREATE TABLE #TABLE_DUMMY_TEMP(ID BIGINT NOT NULL,DESCRIPTION NVARCHAR(MAX),DATE DATETIME NOT NULL,FLAG BIT NOT NULL,MONEY DECIMAL(18,0) NOT NULL,STATUS INT NOT NULL)");
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

        [Column("STATUS")]
        public Status Status { get; set; }
    }

    public enum Status
    {
        Pending = 0,
        Active = 1,
        Inactive = 2,
        Deleted = 3
    }
}
