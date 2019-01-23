﻿using MicroSqlBulk.Attributes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MicroSqlBulk.Helper
{
    public static class TableHelper
    {
       

        public static string GetTableName<TEntity>()
        {
            TableAttribute customAttribute = (TableAttribute)(typeof(TEntity).GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault());

            if (customAttribute == null)
                throw new InvalidOperationException($"The '{typeof(TEntity)}' entity should be configured through the '{nameof(TableAttribute)}'");

            var schema = !string.IsNullOrWhiteSpace(customAttribute.Schema) ? $"{customAttribute.Schema}." : string.Empty;
            var tableName = customAttribute.Name;

            return $"{schema}{tableName}";
        }

        private static Dictionary<Type, String> _sqlDataMapper
        {
            get
            {               
                Dictionary<Type, String> dataMapper = new Dictionary<Type, string>();
                dataMapper.Add(typeof(int), "BIGINT NOT NULL");
                dataMapper.Add(typeof(int?), "BIGINT");
                dataMapper.Add(typeof(long), "BIGINT NOT NULL");
                dataMapper.Add(typeof(long?), "BIGINT");
                dataMapper.Add(typeof(string), "NVARCHAR(MAX)");
                dataMapper.Add(typeof(bool), "BIT NOT NULL");
                dataMapper.Add(typeof(bool?), "BIT");
                dataMapper.Add(typeof(DateTime), "DATETIME NOT NULL");
                dataMapper.Add(typeof(DateTime?), "DATETIME");
                dataMapper.Add(typeof(float), "FLOAT NOT NULL");
                dataMapper.Add(typeof(float?), "FLOAT");
                dataMapper.Add(typeof(decimal), "DECIMAL(18,0) NOT NULL");
                dataMapper.Add(typeof(decimal?), "DECIMAL(18,0)");
                dataMapper.Add(typeof(Guid), "UNIQUEIDENTIFIER NOT NULL");
                dataMapper.Add(typeof(Guid?), "UNIQUEIDENTIFIER");

                return dataMapper;
            }
        }

        public static string GetSQLDataType(this Type type)
        {
            if (_sqlDataMapper.TryGetValue(type, out string dataType))
            {
                return dataType;
            }

            throw new KeyNotFoundException($"The element {type.Name} doesn't  match any key in the collection.");
        }

        public static string GenerateLocalTempTableScript<TEntity>()
        {
            var config = CacheHelper.GetConfiguration<TEntity>();

            StringBuilder script = new StringBuilder();

            script.AppendLine($"CREATE TABLE #{config.TableName}_TEMP");
            script.AppendLine("(");

            for (int i = 0; i < config.Columns.Count; i++)
            {
                Column column = config.Columns[i];

                script.Append($"\t {column.Name} {GetSQLDataType(column.PropertyDescriptor.PropertyType)}");

                if (i != config.Columns.Count - 1)
                {
                    script.Append(",");
                }

                script.Append(Environment.NewLine);
            }

            script.AppendLine(")");

            return script.ToString();
        }

        public static string GenerateSetUpdate<TEntity>()
        {
            var config = CacheHelper.GetConfiguration<TEntity>();

            StringBuilder script = new StringBuilder();

            for (int i = 0; i < config.Columns.Count; i++)
            {
                Column column = config.Columns[i];

                if (column.IsPrimaryKey)
                    continue;

                script.Append($"{config.TableName}.{column.Name} = #{config.TableName}_TEMP.{column.Name}");

                if (i != config.Columns.Count - 1)
                {
                    script.Append(",");
                }
            }

            return script.ToString();
        }

        public static string GenerateOnJoin<TEntity>()
        {
            var config = CacheHelper.GetConfiguration<TEntity>();

            Column columnPrimaryKey = config.Columns.FirstOrDefault(column => column.IsPrimaryKey);

            if (columnPrimaryKey == null)
                throw new MissingFieldException($"Unable to proceed with the operation, because the primary key of the {config.TableName} table was not found.");

            return $"ON {config.TableName}.{columnPrimaryKey.Name} = #{config.TableName}_TEMP.{columnPrimaryKey.Name}";
        }

        public static string GenerateValuesUpdate<TEntity>()
        {
            var config = CacheHelper.GetConfiguration<TEntity>();

            StringBuilder script = new StringBuilder();

            for (int i = 0; i < config.Columns.Count; i++)
            {
                Column column = config.Columns[i];

                if (column.IsPrimaryKey)
                    continue;

                script.Append($"#{config.TableName}_TEMP.{column.Name}");

                if (i != config.Columns.Count - 1)
                {
                    script.Append(",");
                }
            }

            return script.ToString();
        }

        public static string GenerateColumnsInsert<TEntity>()
        {
            var config = CacheHelper.GetConfiguration<TEntity>();

            StringBuilder script = new StringBuilder();

            for (int i = 0; i < config.Columns.Count; i++)
            {
                Column column = config.Columns[i];

                if (column.IsPrimaryKey)
                    continue;

                script.Append($"{column.Name}");

                if (i != config.Columns.Count - 1)
                {
                    script.Append(",");
                }
            }

            return script.ToString();
        }
    }
}
