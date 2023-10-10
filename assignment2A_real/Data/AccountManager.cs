using assignment2A_real.Models;
using System.Data.Entity;
using System.Data.SQLite;
using System.Transactions;


namespace assignment2A_real.Data
{
    public class AccountManager
    {
        private static string connectionString = "Data Source=accountDatabase.db;Version=3;";

        public static bool CreateAccountTable()
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Enable foreign key constraints
                    using (SQLiteCommand pragmaCommand = connection.CreateCommand())
                    {
                        pragmaCommand.CommandText = "PRAGMA foreign_keys = ON;";
                        pragmaCommand.ExecuteNonQuery();
                    }

                    // Create the Account table
                    using (SQLiteCommand accountCommand = connection.CreateCommand())
                    {
                        accountCommand.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Account (
                        AcctNo INTEGER PRIMARY KEY,
                        Bal INTEGER,
                        Pin INTEGER,
                        Fname TEXT,
                        Lname TEXT
                        transactions TEXT
                    )";
                        accountCommand.ExecuteNonQuery();
                    }

                 using (SQLiteCommand transactionCommand = connection.CreateCommand())
                    {
                        transactionCommand.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Transaction (
                        TransactionId INTEGER PRIMARY KEY,
                        AcctNo INTEGER,
                        Amount DECIMAL,
                        FOREIGN KEY(AcctNo) REFERENCES Account(AcctNo)
                    )";
                        transactionCommand.ExecuteNonQuery();
                    } 
                
                    connection.Close();
                }

                Console.WriteLine("Account and Transaction tables created successfully.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }


        public static void LoadSampleAccountData()
        {
            List<Account> sampleAccounts = new List<Account>
    {
        new Account { AcctNo = 453221, Bal = 5000, Pin = 1234, Fname = "John", Lname = "Doe" },
        new Account { AcctNo = 345221, Bal = 3000, Pin = 5678, Fname = "Jane", Lname = "Smith" },
        new Account { AcctNo = 4751226, Bal = 7000, Pin = 9876, Fname = "Alice", Lname = "Johnson" }
    };

            foreach (var account in sampleAccounts)
            {
                Insert(account);

                // Insert transactions for the account
               // LoadSampleTransactionData(account.AcctNo);
            }

            Console.WriteLine("Sample account data loaded successfully.");
        }
        /*   public static void LoadSampleTransactionData(int acctNo)
           {
               List<assignment2A_real.Models.Transaction> sampleTransactions = new List<assignment2A_real.Models.Transaction>
       {
           new assignment2A_real.Models.Transaction { TransactionId = 1, Amount = 1000, AcctNo = acctNo },
           new assignment2A_real.Models.Transaction { TransactionId = 2, Amount = -500, AcctNo = acctNo },
           new assignment2A_real.Models.Transaction { TransactionId = 3, Amount = 500, AcctNo = acctNo }
       };

               foreach (var transaction in sampleTransactions)
               {
                   TransactionManager.InsertTransaction(transaction);
               }

               Console.WriteLine($"Sample transactions loaded for Account {acctNo}.");
           }*/


        public static void Insert(Account account)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        // Check if an account with the same AcctNo already exists
                        command.CommandText = "SELECT COUNT(*) FROM Account WHERE AcctNo = @AcctNo";
                        command.Parameters.AddWithValue("@AcctNo", account.AcctNo);
                        int existingAccountCount = Convert.ToInt32(command.ExecuteScalar());

                        if (existingAccountCount > 0)
                        {
                            Console.WriteLine($"An account with AcctNo {account.AcctNo} already exists.");
                            return; // Handle the duplicate account entry as needed
                        }

                        // Insert the new account
                        command.CommandText = @"
                            INSERT INTO Account (AcctNo, Bal, Pin, Fname, Lname)
                            VALUES (@AcctNo, @Bal, @Pin, @Fname, @Lname)";

                        command.Parameters.AddWithValue("@Bal", account.Bal);
                        command.Parameters.AddWithValue("@Pin", account.Pin);
                        command.Parameters.AddWithValue("@Fname", account.Fname);
                        command.Parameters.AddWithValue("@Lname", account.Lname);

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

        public static void UpdateAccount(Account account)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        // Update the account based on AcctNo
                        command.CommandText = @"
                    UPDATE Account
                    SET Bal = @Bal, Pin = @Pin, Fname = @Fname, Lname = @Lname
                    WHERE AcctNo = @AcctNo";

                        command.Parameters.AddWithValue("@Bal", account.Bal);
                        command.Parameters.AddWithValue("@Pin", account.Pin);
                        command.Parameters.AddWithValue("@Fname", account.Fname);
                        command.Parameters.AddWithValue("@Lname", account.Lname);
                        command.Parameters.AddWithValue("@AcctNo", account.AcctNo);

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

        public static void DeleteAccount(int acctNo)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        // Delete the account based on AcctNo
                        command.CommandText = "DELETE FROM Account WHERE AcctNo = @AcctNo";
                        command.Parameters.AddWithValue("@AcctNo", acctNo);

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

        public static void DeleteAllAccounts()
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        // Delete all accounts from the Account table
                        command.CommandText = "DELETE FROM Account";
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


        public static bool AccountExists(int acctNo)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = connection.CreateCommand())
                {
                    // Check if an account with the given AcctNo exists
                    command.CommandText = "SELECT COUNT(*) FROM Account WHERE AcctNo = @AcctNo";
                    command.Parameters.AddWithValue("@AcctNo", acctNo);
                    int accountCount = Convert.ToInt32(command.ExecuteScalar());

                    connection.Close();

                    return accountCount > 0;
                }
            }
        }

        public static List<Account> GetAllAccounts()
        {
            List<Account> accounts = new List<Account>();

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        // Retrieve all accounts from the Account table
                        command.CommandText = "SELECT * FROM Account";
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Account account = new Account
                                {
                                    AcctNo = reader.GetInt32(reader.GetOrdinal("AcctNo")),
                                    Bal = reader.GetInt32(reader.GetOrdinal("Bal")),
                                    Pin = reader.GetInt32(reader.GetOrdinal("Pin")),
                                    Fname = reader.GetString(reader.GetOrdinal("Fname")),
                                    Lname = reader.GetString(reader.GetOrdinal("Lname"))

                                };
                             //   account.Transactions = GetTransactionsForAccount(account.AcctNo);
                                accounts.Add(account);
                            }
                        }
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return accounts;
        }

       
     /*   public static List<assignment2A_real.Models.Transaction> GetTransactionsForAccount(int acctNo)
        {
            List<assignment2A_real.Models.Transaction> transactions = new List<assignment2A_real.Models.Transaction>();

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        // Retrieve transactions for the specified account
                        command.CommandText = "SELECT * FROM \"Transaction\" WHERE AcctNo = @AcctNo";
                        command.Parameters.AddWithValue("@AcctNo", acctNo);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                assignment2A_real.Models.Transaction transaction = new assignment2A_real.Models.Transaction
                                {
                                    TransactionId = reader.GetInt32(reader.GetOrdinal("TransactionId")),
                                    Amount = reader.GetDecimal(reader.GetOrdinal("Amount")),
                                    AcctNo = reader.GetInt32(reader.GetOrdinal("AcctNo"))
                                };

                                transactions.Add(transaction);
                            }
                        }
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return transactions;
        }*/

        public static Account GetAccountByAcctNo(int acctNo)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = connection.CreateCommand())
                {
                    // Retrieve an account by AcctNo
                    command.CommandText = "SELECT * FROM Account WHERE AcctNo = @AcctNo";
                    command.Parameters.AddWithValue("@AcctNo", acctNo);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Account
                            {
                                AcctNo = Convert.ToInt32(reader["AcctNo"]),
                                Bal = Convert.ToInt32(reader["Bal"]),
                                Pin = Convert.ToInt32(reader["Pin"]),
                                Fname = reader["Fname"].ToString(),
                                Lname = reader["Lname"].ToString()
                            };
                        }
                        else
                        {
                            return null; // Account not found
                        }
                    }
                }
            }
        }
        public static void SeedAccount()
        {
            try
            {
                DeleteAllAccounts();

                string[] firstNames = { "Rafe", "Aziz", "James", "Alsion", "Ella", "Timothy", "Emily", "Jeffrey", "Scott", "Patrick" };
                string[] lastNames = { "Wright", "Scott", "Brown", "Davis", "Morris", "Hayes", "Lee", "Phillips", "Cox", "ClJenkinsark" };

                var random = new Random();
                var accounts = new List<Account>();
                var usedAccountNumbers = new HashSet<int>();

                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    for (int i = 1; i <= 10; i++)
                    {
                        int uniqueAccountNumber;
                        do
                        {
                            uniqueAccountNumber = random.Next(100000, 999999);
                        }
                        while (usedAccountNumbers.Contains(uniqueAccountNumber));

                        usedAccountNumbers.Add(uniqueAccountNumber);
                        string randomFirstName = firstNames[random.Next(firstNames.Length)];
                        string randomLastName = lastNames[random.Next(lastNames.Length)];

                        var account = new Account
                        {
                            Bal = random.Next(-5000, 5000),
                            Pin = random.Next(10000, 999998),
                            Fname = randomFirstName,
                            Lname = randomLastName,
                            AcctNo = uniqueAccountNumber
                        };

                        Insert(account);
                    }

                    connection.Close();
                }

                Console.WriteLine("Sample account data loaded successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }


        public static void DBInitializeAccount()
        {
            if (CreateAccountTable())
            {
                DeleteAllAccounts(); // Delete all previous accounts
                LoadSampleAccountData();
            }
        }

    }

}
