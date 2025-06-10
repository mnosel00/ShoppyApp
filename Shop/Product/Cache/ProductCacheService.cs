using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Shop.Product.Dto;

namespace Shop.Product.Cache
{
    public class ProductCacheService
    {
        private readonly string _dbPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "shop_products_cache.db");

        public ProductCacheService()
        {
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
                @"CREATE TABLE IF NOT EXISTS Products (
                    Id TEXT PRIMARY KEY,
                    Name TEXT NOT NULL,
                    Price REAL NOT NULL
                );";
            command.ExecuteNonQuery();
        }

        public async Task SaveProductsAsync(IEnumerable<ProductDto> products)
        {
            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            await connection.OpenAsync();

            using var transaction = connection.BeginTransaction();
            var deleteCmd = connection.CreateCommand();
            deleteCmd.CommandText = "DELETE FROM Products;";
            await deleteCmd.ExecuteNonQueryAsync();

            foreach (var product in products)
            {
                var insertCmd = connection.CreateCommand();
                insertCmd.CommandText =
                    "INSERT INTO Products (Id, Name, Price) VALUES ($id, $name, $price);";
                insertCmd.Parameters.AddWithValue("$id", product.Id.ToString());
                insertCmd.Parameters.AddWithValue("$name", product.Name);
                insertCmd.Parameters.AddWithValue("$price", product.Price);
                await insertCmd.ExecuteNonQueryAsync();
            }
            transaction.Commit();
        }

        public async Task<List<ProductDto>> LoadProductsAsync()
        {
            var products = new List<ProductDto>();
            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, Name, Price FROM Products;";
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                products.Add(new ProductDto
                {
                    Id = Guid.Parse(reader.GetString(0)),
                    Name = reader.GetString(1),
                    Price = reader.GetDecimal(2)
                });
            }
            return products;
        }
    }
}
