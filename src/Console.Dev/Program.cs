
using MicroSqlBulk;
using MicroSqlBulk.Attributes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Console.Dev
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "data source=localhost;initial catalog=HBMDM_1ST_SERVICES;persist security info=True;user id=sa;password=Mar@9627;MultipleActiveResultSets=True;App=EntityFramework";
            var list = new List<TesteEntidade>();

            for (int i = 3000; i < 503000; i++)
            {
                list.Add(new TesteEntidade
                {
                    CdCodigo = i + 1,
                    Idade = 22,
                    Nome = Guid.NewGuid().ToString()
                });
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.BulkUpdate(list, 300);
            }
        }
    }

    [Table("TB_TESTE")]
    public class TesteEntidade
    {
        [Column("CdCodigo", true)]
        public int CdCodigo { get; set; }
        [Column("Idade")]
        public int Idade { get; set; }
        [Column("Nome")]
        public string Nome { get; set; }
    }
}

