## MicroSqlBulk 

MicroSqlBulk  or **µ**SqlBulk is an IDbConnection extension for bulk operations.

### Download
>**µ**SqlBulk is available [here](https://www.nuget.org/packages/MicroSqlBulk/) as a Nuget Package. You can install it using the Nuget Package Console window:

```PM> Install-Package MicroSqlBulk -Version 1.0.2```

### usage
*1. Attributes*
 
Attributes are powerful feature in the .NET that can add metadata information to your assemblies.

The current version of **µ**SqlBulk only supports metadata attributes. But don't worry we're working to build the fluent version.


The attributes can be applied on your entity class or properties like the example bellow:


    [Table("TABLE_NAME", "SCHEMA")]
    public class Foo
    {
        [Column("ID")]
        public int Id { get; set; }

        [Column("XPTO")]
        public int Xpto { get; set; }

        [Ignore]
        public bool Ignored { get; set; }
    }  

*2. Extending the IDbConnection*

Extension methods are another powerful feature in the .NET and allow you to extend the functionality of existing types without the need of modify them or create sub types from them. 
Here's how simple it is to extend IDbConnection in the example below:

    List<Entity> data = DataBuilder();
    
    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.BulkInsert(data);                
    }

*Default settings*

* Open Connection
* Close connection
* 30 seconds of timeout

```void BulkInsert<TEntity>(this IDbConnection dbConnection, List<TEntity> data, int timeout = 30, bool openConnection = true, bool closeConnection = true)```

### License
You can read the terms of use [here](https://github.com/wgamagomes/MicroSqlBulk/blob/master/LICENSE).
