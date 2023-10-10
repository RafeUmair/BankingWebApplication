using assignment2A_real.Models;
using System.Data.Entity;
using System.Data.SQLite;
using System.Text;
using System.Xml.Linq;

namespace assignment2A_real.Data
{
    public class UserProfileManager
    {
        private static string connectionString = "Data Source=userProfile.db;Version=3;";


        public static bool CreateUserProfileTable()
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Create a new SQLite command to execute SQL
                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        // SQL command to create the UserProfile table
                        command.CommandText = @"
                        CREATE TABLE IF NOT EXISTS UserProfile (
                        Name TEXT PRIMARY KEY,
                        Email TEXT,
                        Address TEXT,
                        Phone INTEGER,
                        Picture TEXT,
                        Password TEXT
                    )";

                        // Execute the SQL command to create the table
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                Console.WriteLine("UserProfile table created successfully.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false; // Create table failed
            }
        }
        public static void LoadSampleUserProfileData()
            {
                List<UserProfile> sampleUserProfiles = new List<UserProfile>
        {
            new UserProfile
            {
                Name = "john_doee",
                Email = "john@example.com",
                Address = "123 Main St",
                Phone = 5551234567,
                Picture = "john.jpg",
                Password = "password123"
            },
            new UserProfile
            {
                Name = "jane_smiths",
                Email = "jane@example.com",
                Address = "456 Elm St",
                Phone = 5559876543,
                Picture = "jane.jpg",
                Password = "password456"
            },
            new UserProfile
            {
                Name = "alice_johnsons",
                Email = "alice@example.com",
                Address = "789 Oak St",
                Phone = 5555555555,
                Picture = "alice.jpg",
                Password = "password789"
            }
        };

            foreach (var userProfile in sampleUserProfiles)
            {
                InsertUserProfile(userProfile);
            }

            Console.WriteLine("Sample user profile data loaded successfully.");
        }

        public static void InsertUserProfile(UserProfile userProfile)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT COUNT(*) FROM UserProfile WHERE Name = @Name";
                        command.Parameters.AddWithValue("@Name", userProfile.Name);
                        int existingProfileCount = Convert.ToInt32(command.ExecuteScalar());

                        if (existingProfileCount > 0)
                        {
                            Console.WriteLine($"A user profile with Name {userProfile.Name} already exists.");
                            return; 
                        }

                        command.CommandText = @"
                            INSERT INTO UserProfile (Name, Email, Address, Phone, Picture, Password)
                            VALUES (@Name, @Email, @Address, @Phone, @Picture, @Password)";

                        command.Parameters.AddWithValue("@Email", userProfile.Email);
                        command.Parameters.AddWithValue("@Address", userProfile.Address);
                        command.Parameters.AddWithValue("@Phone", userProfile.Phone);
                        command.Parameters.AddWithValue("@Picture", userProfile.Picture);
                        command.Parameters.AddWithValue("@Password", userProfile.Password);

                        command.ExecuteNonQuery();
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        public static IEnumerable<UserProfile> GetAllUserProfiles()
        {
            List<UserProfile> userProfiles = new List<UserProfile>();

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        // Select all user profiles
                        command.CommandText = "SELECT * FROM UserProfile";

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                UserProfile userProfile = new UserProfile
                                {
                                    Name = reader["Name"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    Address = reader["Address"].ToString(),
                                    Phone = (long)(reader["Phone"]),
                                    Picture = reader["Picture"].ToString(),
                                    Password = reader["Password"].ToString()
                                };

                                userProfiles.Add(userProfile);
                                Console.WriteLine(userProfile.Name);

                            }
                        }
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in GetAllUserProfiles: " + ex.Message);
            }

            return userProfiles;
        }


        public static void UpdateUserProfile(string oldname, UserProfile userProfile)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        DeleteUserProfile(oldname);

                        command.CommandText = @"
                        INSERT INTO UserProfile (Name, Email, Address, Phone, Picture, Password)
                        VALUES (@Name, @Email, @Address, @Phone, @Picture, @Password)";

                        command.Parameters.AddWithValue("@Email", userProfile.Email);
                        command.Parameters.AddWithValue("@Address", userProfile.Address);
                        command.Parameters.AddWithValue("@Phone", userProfile.Phone);
                        command.Parameters.AddWithValue("@Picture", userProfile.Picture);
                        command.Parameters.AddWithValue("@Password", userProfile.Password);
                        command.Parameters.AddWithValue("@Name", userProfile.Name);

                        command.ExecuteNonQuery();
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        public static void DeleteUserProfile(string name)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        // Delete the user profile based on Name
                        command.CommandText = "DELETE FROM UserProfile WHERE Name = @Name";
                        command.Parameters.AddWithValue("@Name", name);

                        command.ExecuteNonQuery();
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        public static void DeleteAllUserProfiles()
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        // Delete all user profiles from the "UserProfile" table
                        command.CommandText = "DELETE FROM UserProfile";
                        command.ExecuteNonQuery();
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        public static UserProfile GetUserProfileByUsername(string username)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        // Retrieve user profile by username
                        command.CommandText = "SELECT * FROM UserProfile WHERE Name = @Username";
                        command.Parameters.AddWithValue("@Username", username);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new UserProfile
                                {
                                    Name = reader["Name"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    Address = reader["Address"].ToString(),
                                    Phone = (long)(reader["Phone"]),
                                    Picture = reader["Picture"].ToString(),
                                    Password = reader["Password"].ToString()
                                };
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return null;
            }
        }

        public static UserProfile GetUserProfileByEmail(string email)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        // Retrieve user profile by email
                        command.CommandText = "SELECT * FROM UserProfile WHERE Email = @Email";
                        command.Parameters.AddWithValue("@Email", email);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new UserProfile
                                {
                                    Name = reader["Name"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    Address = reader["Address"].ToString(),
                                    Phone = (long)(reader["Phone"]),
                                    Picture = reader["Picture"].ToString(),
                                    Password = reader["Password"].ToString()
                                };
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return null;
            }
        }

        public static bool UserProfileExists(string username)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        // Check if the user profile exists by username
                        command.CommandText = "SELECT COUNT(*) FROM UserProfile WHERE Name = @Username";
                        command.Parameters.AddWithValue("@Username", username);

                        int profileCount = Convert.ToInt32(command.ExecuteScalar());

                        return profileCount > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        public static void SeedUserProfile()
        {
            DeleteAllUserProfiles(); // Delete all previous transactions

            string[] userprofilenames = { "shaquille.oatmeal", "CelebrityCaio", "hoosier-daddy", "BadKarma", "GRAFETY", "AllGoodNamesRGone", "anonymouse", "YESIMFUNNY", "BenAfleckIsAnOkActor", "JJAYZ" };

            var random = new Random();
            var userProfiles = new List<UserProfile>();

            for (int i = 0; i < 10; i++)
            {
                string name = userprofilenames[random.Next(userprofilenames.Length)];

                var userProfile = new UserProfile
                {
                    Name = name,
                    Email = $"user{i}@example.com",
                    Address = $"Address {i}",
                    Phone = (long)random.Next(100000000, 1000000000),
                    Picture = $"Picture {i}",
                    Password = GenerateRandomPassword()
                };

                userProfiles.Add(userProfile);
                InsertUserProfile(userProfile);
            }

        }
        public static String GenerateRandomPassword()
        {
            string allowedChars = "1234567890qweasdzxcrtyfghbnmuiopjkl";
            Random random = new Random();
            StringBuilder password = new StringBuilder();

            for (int i = 0; i < 8; i++)
            {
                int index = random.Next(0, allowedChars.Length);
                password.Append(allowedChars[index]);
            }

            return password.ToString();
        }


        public static void DBInitializeUserProfile()
        {
            if (CreateUserProfileTable())
            {
               DeleteAllUserProfiles();
                LoadSampleUserProfileData();
            }
        }
    }
}
