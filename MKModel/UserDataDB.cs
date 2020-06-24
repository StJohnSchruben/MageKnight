using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MKModel
{
    public static class UserDataDB
    {
        public static void ResetDB()
        {
            DeleteTable("UserAccounts");
            DeleteTable("UserInvetory");
        }
        public static ObservableCollection<MageKnightData> GetUserInventory(Guid userId)
        {
            SqlConnection connection = MKUserDataDB.GetConnection();
            string selectStatement
                = "SELECT UserId, MageId, Quantity " +
                "FROM UserInventory " +
                "WHERE UserId = @UserId";

            SqlCommand selectCommand = new SqlCommand(selectStatement, connection);
            selectCommand.Parameters.AddWithValue("@UserId", userId);

            try
            {
                bool isDuplicate = false;
                connection.Open();
                UserData user = new UserData();
                ObservableCollection<MageKnightData> models = new ObservableCollection<MageKnightData>();
                SqlDataReader reader = selectCommand.ExecuteReader(System.Data.CommandBehavior.SingleResult);
                while (reader.Read())
                {
                    int quantity = Int32.Parse(reader["Quantity"].ToString());
                    Guid mageId = Guid.Parse(reader["MageId"].ToString());
                    for (int i = 0; i < quantity; i++)
                    {
                        models.Add(MageDB.GetMageKnight(mageId));
                    }
                }

                connection.Close();

                return models;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                connection.Close();
            }

            return null;
        }

        public static UserModel SignUp(UserData user)
        {
            SqlConnection connection = MKUserDataDB.GetConnection();
            string insertStatment =
                "INSERT Into UserAccounts " +
                "(Id, UserName, Password, RebellionBoosterPacks, WhirlWindBoosterPacks, LancersBoosterPacks, UnlimitedBoosterPacks, SinisterBoosterPacks, MinionsBoosterPacks, UprisingBoosterPacks) " +
                "VALUES(@Id, @UserName, @Password, @RebellionBoosterPacks, @WhirlWindBoosterPacks, @LancersBoosterPacks, @UnlimitedBoosterPacks, @SinisterBoosterPacks, @MinionsBoosterPacks, @UprisingBoosterPacks)";

            SqlCommand insertCommand = new SqlCommand(insertStatment, connection);
            insertCommand.Parameters.AddWithValue(
                 "@Id", user.Id);
            insertCommand.Parameters.AddWithValue(
                "@RebellionBoosterPacks", 5);
            insertCommand.Parameters.AddWithValue(
                "@WhirlWindBoosterPacks", 0);
            insertCommand.Parameters.AddWithValue(
                "@LancersBoosterPacks", 0);
            insertCommand.Parameters.AddWithValue(
                "@UnlimitedBoosterPacks", 0);
            insertCommand.Parameters.AddWithValue(
                "@SinisterBoosterPacks", 0);
            insertCommand.Parameters.AddWithValue(
                "@MinionsBoosterPacks", 0);
            insertCommand.Parameters.AddWithValue(
                "@UprisingBoosterPacks", 0);
            insertCommand.Parameters.AddWithValue(
                "@UserName", user.UserName);
            insertCommand.Parameters.AddWithValue(
                "@Password", user.Password);

            try
            {
                connection.Open();
                int count = insertCommand.ExecuteNonQuery();
                if (count < 0)
                {
                    return SignIn(user.UserName, user.Password);
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                connection.Close();
            }

            return null;
        }

        public static UserModel SignIn(string userName, string password)
        {
            SqlConnection connection = MKUserDataDB.GetConnection();
            string selectStatement
                = "SELECT Id, UserName, Password, RebellionBoosterPacks, WhirlWindBoosterPacks, LancersBoosterPacks, UnlimitedBoosterPacks, SinisterBoosterPacks, MinionsBoosterPacks, UprisingBoosterPacks " +
                "FROM UserAccounts " +
                "WHERE UserName = @UserName ";

            SqlCommand selectCommand = new SqlCommand(selectStatement, connection);
            selectCommand.Parameters.AddWithValue("@UserName", userName);

            try
            {
                connection.Open();
                UserData user = new UserData();
                SqlDataReader reader = selectCommand.ExecuteReader(System.Data.CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    user.Id = Guid.Parse(reader["Id"].ToString());
                    user.UserName = reader["UserName"].ToString();
                    user.Password = reader["Password"].ToString();
                    user.RebellionBoosterPacks = Int32.Parse(reader["RebellionBoosterPacks"].ToString());
                    //user.WhirlWindBoosterPacks = Int32.Parse(reader["WhirlWindBoosterPacks"].ToString());
                    //user.LancersBoosterPacks = Int32.Parse(reader["LancersBoosterPacks"].ToString());
                    //user.UnlimitedBoosterPacks = Int32.Parse(reader["UnlimitedBoosterPacks"].ToString());
                    //user.SinisterBoosterPacks = Int32.Parse(reader["SinisterBoosterPacks"].ToString());
                    //user.MinionsBoosterPacks = Int32.Parse(reader["MinionsBoosterPacks"].ToString());
                    //user.UprisingBoosterPacks = Int32.Parse(reader["UprisingBoosterPacks"].ToString());
                }

                connection.Close();

                return new UserModel(user);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                connection.Close();
            }

            return null;
        }

        public static void AddMageToInventory(Guid mageId, Guid userId)
        {
            SqlConnection connection = MKUserDataDB.GetConnection();
            string selectStatement
                = "SELECT UserId, MageId, Quantity " +
                "FROM UserInventory " +
                "WHERE UserId = @UserId AND MageId = @MageId ";

            SqlCommand selectCommand = new SqlCommand(selectStatement, connection);
            selectCommand.Parameters.AddWithValue("@UserId", userId);
            selectCommand.Parameters.AddWithValue("@MageId", mageId);

            try
            {
                bool isDuplicate = false;
                connection.Open();
                UserData user = new UserData();
                SqlDataReader reader = selectCommand.ExecuteReader();
                List<MageData> duplicates = new List<MageData>();
                while (reader.Read())
                {
                    MageData data = new MageData();
                    data.Id = Guid.Parse(reader["MageId"].ToString());
                    duplicates.Add(data);
                }

                connection.Close();

                if (duplicates.Count() == 0)
                {
                    InsertNewMage(mageId, userId);
                }
                else
                {
                    UpdateMageQuantity(mageId, userId, duplicates.Count());
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            finally
            {
                connection.Close();
            }
        }

        private static void InsertNewMage(Guid mageId, Guid userId)
        {
            SqlConnection connection = MKUserDataDB.GetConnection();
            string insertStatement =
                "INSERT Into UserInventory " +
                "(UserId, MageId, Quantity)" +
                "VALUES (@UserId, @MageId, @Quantity)";

            SqlCommand insertCommand = new SqlCommand(insertStatement, connection);
            insertCommand.Parameters.AddWithValue(
                "@UserId", userId);
            insertCommand.Parameters.AddWithValue(
                "@MageId", mageId);
            insertCommand.Parameters.AddWithValue(
                "@Quantity", 1);

            try
            {
                connection.Open();
                int count = insertCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString());
                throw;
            }
            finally
            {
                connection.Close();
            }
        }

        public static void UpdateUsersBoosterPackCount(BoosterPack set, Guid id, int value)
        {
            SqlConnection connection = MKUserDataDB.GetConnection();
            string updateStatement =
                           "UPDATE UserAccounts SET " +
                           "RebellionBoosterPacks = @RebellionBoosterPacks " +
                           "WHERE Id = @Id";

            SqlCommand insertCommand = new SqlCommand(updateStatement, connection);
            insertCommand.Parameters.AddWithValue(
                "@Id", id);
            insertCommand.Parameters.AddWithValue(
                "@RebellionBoosterPacks", value);

            try
            {
                connection.Open();
                int count = insertCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString());
                throw;
            }
            finally
            {
                connection.Close();
            }
        }

        private static void UpdateMageQuantity(Guid mageId, Guid userId, int quantity)
        {
            SqlConnection connection = MKUserDataDB.GetConnection();
            string updateStatement =
                           "UPDATE UserInventory SET " +
                           "Quantity = @Quantity " +
                           "WHERE MageId = @MageId AND UserId = @UserId ";

            SqlCommand insertCommand = new SqlCommand(updateStatement, connection);
            insertCommand.Parameters.AddWithValue("@Quantity", quantity + 1);
            insertCommand.Parameters.AddWithValue("@MageId", mageId);
            insertCommand.Parameters.AddWithValue("@UserId", userId);

            try
            {
                connection.Open();
                int count = insertCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString());
                throw;
            }
            finally
            {
                connection.Close();
            }

        }

        public static void DeleteTable(string table)
        {
            SqlConnection connection = MKUserDataDB.GetConnection();
            string deletStatment =
               $" DELETE FROM {table}";

            SqlCommand deletCommand = new SqlCommand(deletStatment, connection);
            try
            {
                connection.Open();
                int count = deletCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
