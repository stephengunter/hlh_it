using ApplicationCore.Models.Identity;
using ApplicationCore.Models.Doc3;
using Infrastructure.Views;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace ApplicationCore.Helpers.Doc3.Old;

public interface IOldReaderHelpers
{
   Task<ICollection<Reader>> FetchReaders();
}

public class OldReaderHelpers : IOldReaderHelpers
{
   private readonly string _connectString;
   public OldReaderHelpers(string connectString)
   {
      _connectString = connectString;
   }
   public async Task<ICollection<Reader>> FetchReaders()
   {
      var readers = new List<Reader>();
      var tables = new List<string>();
      for (int i = 101; i <= 113; i++)
      {
         if (i == 112) continue;
         tables.Add($"read{i}");
      }
      tables.Add("readlist");
      foreach (var table in tables)
      {
         readers.AddRange(await GetReaders(table));
      }
      return readers;
   }
   async Task<ICollection<Reader>> GetReaders(string table)
   {
      var readers = new List<Reader>();
      string query = $"SELECT DISTINCT pername FROM {table}";
      using (SqlConnection conn = new SqlConnection(_connectString))
      {
         conn.Open();
         using (SqlCommand cmd = new SqlCommand(query, conn))
         using (SqlDataReader reader = cmd.ExecuteReader())
         {
            while (reader.Read())
            {
               readers.Add(new Reader
               {
                  Name = reader.GetString(0)
               });
            }

         }
      }
      return readers;
   }
}