
using MicroSqlBulk.Attributes;
using MicroSqlBulk.Helper;

namespace Console.Dev
{
    class Program
    {
        static void Main(string[] args)
        {
            var columns = ColumnHelper.GetFildesInfo<Table>();
        }
    }

    [Table("TABLE")]
    public class Table
    {
        [Column("ID")]
        public int Id { get; set; }

        [Column("DESCRIPTION")]
        public string Description { get; set; }
    }
}
