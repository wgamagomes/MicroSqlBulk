
namespace MicroSqlBulk
{
   public class Row
    {
        public Row(params object[] values)
        {
            Values = values;
        }

        public object[] Values { get; }
    }
}
