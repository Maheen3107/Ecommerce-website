using System.Collections.Generic;
using System.Data.SqlClient;
using Final_Project.Models; // Adjust based on your project structure
using Microsoft.Data.SqlClient;
namespace Final_Project.Models.Product
{
    public class ProductRepository : IProductRepository
    {
        private const string ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Project;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var products = new List<Product>();

            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand("SELECT * FROM Products", connection);
                await connection.OpenAsync(); // Asynchronous open

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        products.Add(new Product
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Description = reader.GetString(reader.GetOrdinal("Description")),
                            Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                            ImagePath = reader.GetString(reader.GetOrdinal("ImagePath")),
                            CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId"))
                        });
                    }
                }
            }

            return products;
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            Product product = null;

            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand("SELECT * FROM Products WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                await connection.OpenAsync(); // Asynchronous open

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        product = new Product
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Description = reader.GetString(reader.GetOrdinal("Description")),
                            Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                            ImagePath = reader.GetString(reader.GetOrdinal("ImagePath")),
                            CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId"))
                        };
                    }
                }
            }

            return product;
        }

        public async Task<IEnumerable<Product>> GetProductsByCategory(int categoryId)
        {
            var products = new List<Product>();

            using (var connection = new SqlConnection(ConnectionString))
            {
                var command = new SqlCommand("SELECT * FROM Products WHERE CategoryId = @CategoryId", connection);
                command.Parameters.AddWithValue("@CategoryId", categoryId);
                await connection.OpenAsync(); // Asynchronous open

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        products.Add(new Product
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Description = reader.GetString(reader.GetOrdinal("Description")),
                            Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                            ImagePath = reader.GetString(reader.GetOrdinal("ImagePath")),
                            CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId"))
                        });
                    }
                }
            }

            return products;
        }
        public void AddProduct(Product product)
        {
            // Define the SQL query to insert the product
            string query = "INSERT INTO [Products] (Name, Description, Price, ImagePath, CategoryId) VALUES (@Name, @Description, @Price, @ImagePath, @CategoryId)";

            // Create and open a connection to the database
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                // Create a SqlCommand object with the query and connection
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Add parameters to the command to prevent SQL injection
                    command.Parameters.AddWithValue("@Name", product.Name);
                    command.Parameters.AddWithValue("@Description", product.Description);
                    command.Parameters.AddWithValue("@Price", product.Price);
                    command.Parameters.AddWithValue("@ImagePath", product.ImagePath);
                    command.Parameters.AddWithValue("@CategoryId", product.CategoryId);

                    // Execute the command
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
