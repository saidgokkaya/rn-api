using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Helper
{
    public static class Sql
    {
        private static string _connectionString;

        public static void Initialize(string connectionString)
        {
            _connectionString = connectionString;
        }

        public static DataTable GetQueryResult(string query)
        {
            try
            {
                if (string.IsNullOrEmpty(query) || string.IsNullOrEmpty(_connectionString))
                {
                    return null;
                }

                using (var connection = new SqlConnection(_connectionString))
                {
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.CommandTimeout = 600;
                        connection.Open();
                        using (var dataReader = command.ExecuteReader())
                        {
                            var dataTable = new DataTable();
                            dataTable.Load(dataReader);
                            return dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
                return null;
            }
        }

        public static DataTable GetStoredProcedure(string procedureName, string[] parameters, object[] values)
        {
            try
            {
                if (parameters.Length != values.Length)
                    return null;

                using (var connection = new SqlConnection(_connectionString))
                {
                    using (var command = new SqlCommand(procedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandTimeout = 600;

                        for (int i = 0; i < parameters.Length; i++)
                        {
                            command.Parameters.AddWithValue(parameters[i], values[i]);
                        }

                        var dataTable = new DataTable();
                        using (var dataAdapter = new SqlDataAdapter(command))
                        {
                            dataAdapter.Fill(dataTable);
                        }
                        return dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
                return null;
            }
        }
    }
}
