using Microsoft.Data.SqlClient;
 namespace Final_Project.Models.User
{
    public class UserRepository:IUserRepository
    {
        string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Project;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";


        public bool RegisterUser(User user)
        {
            try
            {
                using (SqlConnection connect = new SqlConnection(_connectionString))
                {
                    connect.Open();

                    // Check if the email already exists
                    string select = "SELECT COUNT(*) FROM Users WHERE Email=@Email";
                    using (SqlCommand selectCommand = new SqlCommand(select, connect))
                    {
                        selectCommand.Parameters.AddWithValue("@Email", user.Email);

                        int existingUserCount = (int)selectCommand.ExecuteScalar();
                        if (existingUserCount > 0)
                        {
                            return false; // Email is already registered
                        }
                    }

                    // Insert user data into the database
                    string query = @"
                INSERT INTO Users (Name, Email, Password, Address, PhoneNumber, City, State, PostalCode, Country)
                VALUES (@Name, @Email, @Password, @Address, @PhoneNumber, @City, @State, @PostalCode, @Country)";

                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        cmd.Parameters.AddWithValue("@Name", user.Name);
                        cmd.Parameters.AddWithValue("@Email", user.Email);
                        cmd.Parameters.AddWithValue("@Password", user.Password); // Ensure password is hashed before passing here
                        cmd.Parameters.AddWithValue("@Address", user.Address ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@City", user.City ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@State", user.State ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@PostalCode", user.PostalCode ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Country", user.Country ?? (object)DBNull.Value);

                        int count = cmd.ExecuteNonQuery();
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error properly using a logging framework
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }

        public User GetUserByEmailAndPassword(string email, string hashedPassword)
        {
            User user = null;

            try
            {
                using (SqlConnection connect = new SqlConnection(_connectionString))
                {
                    connect.Open();
                    string query = "SELECT * FROM Users WHERE Email=@Email AND Password=@Password";
                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Password", hashedPassword); // Ensure hashed password

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    user = new User
                                    {
                                        Id = reader.GetInt32(0),
                                        Name = reader.GetString(1),
                                        Email = reader.GetString(2),
                                        Password = reader.GetString(3),
                                        Address = reader.IsDBNull(4) ? null : reader.GetString(4),
                                        PhoneNumber = reader.IsDBNull(5) ? null : reader.GetString(5),
                                        City = reader.IsDBNull(6) ? null : reader.GetString(6),
                                        State = reader.IsDBNull(7) ? null : reader.GetString(7),
                                        PostalCode = reader.IsDBNull(8) ? null : reader.GetString(8),
                                        Country = reader.IsDBNull(9) ? null : reader.GetString(9)
                                    };
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return user;
        }






        public User GetUserByEmail(string email)
        {
            User user = null;

            try
            {
                using (SqlConnection connect = new SqlConnection(_connectionString))
                {
                    connect.Open();
                    string query = "SELECT * FROM Users WHERE Email=@Email";
                    using (SqlCommand cmd = new SqlCommand(query, connect))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    user = new User
                                    {
                                        Id = reader.GetInt32(0),
                                        Name = reader.GetString(1),
                                        Email = reader.GetString(2),
                                        Password = reader.GetString(3),
                                        Address = reader.IsDBNull(4) ? null : reader.GetString(4),
                                        PhoneNumber = reader.IsDBNull(5) ? null : reader.GetString(5),
                                        City = reader.IsDBNull(6) ? null : reader.GetString(6),
                                        State = reader.IsDBNull(7) ? null : reader.GetString(7),
                                        PostalCode = reader.IsDBNull(8) ? null : reader.GetString(8),
                                        Country = reader.IsDBNull(9) ? null : reader.GetString(9)
                                    };
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return user;
        }
        public List<User> GetAllUsers()
        {
            var users = new List<User>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM Users", connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var user = new User
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            Password = reader.GetString(reader.GetOrdinal("Password")),
                            Address = reader.GetString(reader.GetOrdinal("Address")),
                            PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                            City = reader.GetString(reader.GetOrdinal("City")),
                            State = reader.GetString(reader.GetOrdinal("State")),
                            PostalCode = reader.GetString(reader.GetOrdinal("PostalCode")),
                            Country = reader.GetString(reader.GetOrdinal("Country")),
                        };
                        users.Add(user);
                    }
                }
            }

            return users;
        }
    }
}

