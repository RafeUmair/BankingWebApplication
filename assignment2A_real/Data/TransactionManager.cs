using System.Data.SQLite;
using assignment2A_real.Models;
using System;
using System.Data.Entity;

namespace assignment2A_real.Data
{
    public class TransactionManager
    {
        private static string connectionString = "Data Source=transaction.db;Version=3;";

        public static bool CreateTransactionTable()
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"
                        CREATE TABLE IF NOT EXISTS ""Transaction"" (
                        TransactionId INTEGER PRIMARY KEY,
                        Amount DECIMAL,
                        AcctNo INTEGER,
                        Type TEXT, -- Add Type column
                        FOREIGN KEY(AcctNo) REFERENCES Account(AcctNo)
                    )";

                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }

                Console.WriteLine("Transaction table created successfully.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        public static void InsertTransaction(Transaction transaction)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"
                    INSERT OR IGNORE INTO ""Transaction"" (TransactionId, Amount, AcctNo, Type)
                    VALUES (@TransactionId, @Amount, @AcctNo, @Type)";

                        command.Parameters.AddWithValue("@TransactionId", transaction.TransactionId);
                        command.Parameters.AddWithValue("@Amount", transaction.Amount);
                        command.Parameters.AddWithValue("@AcctNo", transaction.AcctNo);
                        command.Parameters.AddWithValue("@Type", transaction.Type); // Add Type parameter

                        {
                            command.ExecuteNonQuery();
                    }

                    connection.Close();
                }
            }
                }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        public static void UpdateTransaction(Transaction transaction)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"
                            UPDATE ""Transaction""
                    SET Amount = @Amount, AcctNo = @AcctNo, Type = @Type
                    WHERE TransactionId = @TransactionId";

                        command.Parameters.AddWithValue("@TransactionId", transaction.TransactionId);
                        command.Parameters.AddWithValue("@Amount", transaction.Amount);
                        command.Parameters.AddWithValue("@AcctNo", transaction.AcctNo);
                        command.Parameters.AddWithValue("@Type", transaction.Type); // Add Type parameter

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


        public static void DeleteTransaction(int transactionId)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "DELETE FROM \"Transaction\" WHERE TransactionId = @TransactionId";
                        command.Parameters.AddWithValue("@TransactionId", transactionId);

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

        public static bool TransactionExists(int transactionId)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT COUNT(*) FROM [Transaction] WHERE TransactionId = @TransactionId";
                    command.Parameters.AddWithValue("@TransactionId", transactionId);
                    int transactionIds = Convert.ToInt32(command.ExecuteScalar());

                    connection.Close();

                    return transactionIds > 0;
                }
            }
        }

        public static void DeleteAllTransactions()
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "DELETE FROM \"Transaction\"";
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

        public static List<Transaction> GetAllTransactions()
        {
            List<Transaction> transactions = new List<Transaction>();

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM \"Transaction\"";

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Transaction transaction = new Transaction
                                {
                                    TransactionId = reader.GetInt32(reader.GetOrdinal("TransactionId")),
                                    Amount = reader.GetDecimal(reader.GetOrdinal("Amount")),
                                    AcctNo = reader.GetInt32(reader.GetOrdinal("AcctNo")),
                                    Type = reader.GetString(reader.GetOrdinal("Type")) 
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
        }

        public static List<Transaction> GetTransactionsByAcctNo(int acctNo)
        {
            List<Transaction> transactions = new List<Transaction>();

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM \"Transaction\" WHERE AcctNo = @AcctNo";
                        command.Parameters.AddWithValue("@AcctNo", acctNo);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Transaction transaction = new Transaction
                                {
                                    TransactionId = reader.GetInt32(reader.GetOrdinal("TransactionId")),
                                    Amount = reader.GetDecimal(reader.GetOrdinal("Amount")),
                                    AcctNo = reader.GetInt32(reader.GetOrdinal("AcctNo")),
                                    Type = reader.GetString(reader.GetOrdinal("Type"))
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
        }

        public static void SeedTransaction()
        {
            DeleteAllTransactions(); 

            string[] transactionDescriptions = { "withdraw", "deposit" };
            var random = new Random();
            var transactions = new List<Transaction>();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                for (int i = 0; i < 10; i++)
                {
                    var transaction = new Transaction
                    {
                        TransactionId = random.Next(1, 10000),
                        Amount = Math.Round((decimal)(random.NextDouble() * 1000), 2),
                        AcctNo = AccountManager.GetRandomAccount().AcctNo,
                        Type = transactionDescriptions[random.Next(transactionDescriptions.Length)]
                    };

                    transactions.Add(transaction);
                    InsertTransaction(transaction); 
                }

                Console.WriteLine("Sample transaction data loaded successfully.");
            }
        }

        public static void LoadSampleTransactionData()
        {
            Transaction transaction1 = new Transaction
            {
                TransactionId = 1,
                Amount = 1000,
                AcctNo = AccountManager.GetRandomAccount().AcctNo,
                Type = "Deposit"

            };

            Transaction transaction2 = new Transaction
            {
                TransactionId = 2,
                Amount = 500,
                AcctNo = AccountManager.GetRandomAccount().AcctNo,
                Type = "Deposit"

            };

            Transaction transaction3 = new Transaction
            {
                TransactionId = 3,
                Amount = 500,
                AcctNo = AccountManager.GetRandomAccount().AcctNo,
                Type = "Withdraw"

            };

            Transaction transaction4 = new Transaction
            {
                TransactionId = 4,
                Amount = 20400,
                AcctNo = AccountManager.GetRandomAccount().AcctNo,
                Type = "Deposit"

            };

            InsertTransaction(transaction1);
            InsertTransaction(transaction2);
            InsertTransaction(transaction3);
            InsertTransaction(transaction4);
        }

        public static void DBInitializeTransaction()
        {

            if (CreateTransactionTable())
            {
                LoadSampleTransactionData();
            }
        }

        public static Transaction GetRandomTransaction()
        {
            List<Transaction> allTransactions = GetAllTransactions();

            if (allTransactions.Count > 0)
            {
                var random = new Random();
                int randomIndex = random.Next(0, allTransactions.Count);
                return allTransactions[randomIndex];
            }       
            return null;
        }


    }
}

