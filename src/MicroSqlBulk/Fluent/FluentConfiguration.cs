using System.Collections.Concurrent;
using System.Collections.Generic;

namespace MicroSqlBulk.Fluent
{
    public class FluentConfiguration 
    {
        private static ConcurrentDictionary<string, FluentConfiguration> _mapperCache = new ConcurrentDictionary<string, FluentConfiguration>();

        public IList<Column> Columns { get; }
        public string TableName { get; private set; }
        public int Timeout { get; private set; }
        public string SchemaName { get; private set; }

        public FluentConfiguration Entity<TEntity>()
        {
            return this;
        }

        public FluentConfiguration Table(string name)
        {
            TableName = name;
            return this;
        }

        public FluentConfiguration SetSchema(string name)
        {
            SchemaName = name;
            return this;
        }

        public FluentConfiguration SetTimeout(int timeout)
        {
            Timeout = timeout;
            return this;
        }

        public void Builder()
        {
            var key = "";

            if (!_mapperCache.ContainsKey(key))
            {
                //Do de configuration here
                _mapperCache.TryAdd(key, this);

            }
        }
    }
}
