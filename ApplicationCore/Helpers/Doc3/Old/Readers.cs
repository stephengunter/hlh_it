using ApplicationCore.Models.Identity;
using ApplicationCore.Models.Doc3;
using Infrastructure.Views;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static OpenIddict.Client.SystemIntegration.OpenIddictClientSystemIntegrationConstants;
using Infrastructure.Helpers;
using ApplicationCore.Views.Docs;

namespace ApplicationCore.Helpers.Doc3.Old;

public interface IOldReaderHelpers
{
   Task<ICollection<ReaderViewModel>> FetchReadersAsync();
   Task<ICollection<Post>> FetchPostsAsync();
}

public class OldReaderHelpers : IOldReaderHelpers
{
   private readonly string _connectString;
   public OldReaderHelpers(string connectString)
   {
      _connectString = connectString;
   }
   public async Task<ICollection<Post>> FetchPostsAsync()
   {
      var posts = new List<Post>();
      string query = "SELECT [contentID], [contentTitle], [contentTime], [contentFile1], [contentFile2], [contentFile3]"
         + ", [contenteunit], [contenteditor], [contentMain]"
            + " FROM [HLH].[dbo].[content] WHERE [contentIcon] = 2";
      using (SqlConnection conn = new SqlConnection(_connectString))
      {
         conn.Open();
         using (SqlCommand cmd = new SqlCommand(query, conn))
         using (SqlDataReader reader = cmd.ExecuteReader())
         {
            while (reader.Read())
            {
               var files = new List<string>();
               for (int i = 1; i <= 3; i++) 
               {
                  string key = $"contentFile{i}";
                  if (reader[key] == DBNull.Value) continue;
                  string file = reader[key].ToString()!.Trim();
                  if(string.IsNullOrEmpty(file)) continue;
                  files.Add(file);
               }

               posts.Add(new Post 
               { 
                  ContentId = Convert.ToInt32(reader["contentID"]),
                  Title = reader["contentTitle"] == DBNull.Value ?  "" : reader["contentTitle"].ToString()!,
                  Content = reader["contentMain"] == DBNull.Value ? "" : reader["contentMain"].ToString()!,
                  Files = files.JoinToString(),
                  Unit = reader["contenteunit"] == DBNull.Value ? "" : reader["contenteunit"].ToString()!,
                  Author = reader["contenteditor"] == DBNull.Value ? "" : reader["contenteditor"].ToString()!,
                  CreatedAt = Convert.ToDateTime(reader["contentTime"]),
                  Order = -1
               });
            }

         }
      }
      return posts;
   }
   public async Task<ICollection<ReaderViewModel>> FetchReadersAsync()
   {
      var readers = new List<ReaderViewModel>();
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
   async Task<ICollection<ReaderViewModel>> GetReaders(string table)
   {
      var readers = new List<ReaderViewModel>();
      string query = $"SELECT [contentID], [pername], [r_time], [r_item]  FROM {table}";
      using (SqlConnection conn = new SqlConnection(_connectString))
      {
         conn.Open();
         using (SqlCommand cmd = new SqlCommand(query, conn))
         using (SqlDataReader reader = cmd.ExecuteReader())
         {
            while (reader.Read())
            {
               readers.Add(new ReaderViewModel
               {
                  Name = reader["pername"] == DBNull.Value ? "" : reader["pername"].ToString()!,
                  ContentId = Convert.ToInt32(reader["contentID"]),
                  ReadAt = reader["r_time"] == DBNull.Value ? null : Convert.ToDateTime(reader["r_time"]),
                  Ps = reader["r_item"] == DBNull.Value ? "" : reader["r_item"].ToString()!,
               });
            }

         }
      }
      return readers;
   }
}