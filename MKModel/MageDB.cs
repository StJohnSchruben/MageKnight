using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MKModel
{
    public class  MageDB
    {
        public static MageKnight EditMageKnight(MageData data)
        {
            try
            {
                return UpdateMageKnight(data);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"something is wrong EditMageKnight:{ex.ToString()}");
            }

            return GetMageKnight(data.Name);
        }

        private static MageKnight UpdateMageKnight(MageData data)
        {
            SqlConnection connection = MageKnightDataDB.GetConnection();
            string updateStatement =
                "UPDATE MageKnights SET " +
                "name = @name, " +
                "faction = @faction, " +
                "index = @index, " +
                "pointValue = @pointValue, " +
                "range = @range, " +
                "frontArc = @frontArc, " +
                "rearArc = @rearArc, " +
                "click = @click ";

            SqlCommand updateCommand =
                new SqlCommand(updateStatement, connection);
            updateCommand.Parameters.AddWithValue(
                "@name", data.Name);
            updateCommand.Parameters.AddWithValue(
                "@faction", data.Faction);
            updateCommand.Parameters.AddWithValue(
                "@index", data.Index);
            updateCommand.Parameters.AddWithValue(
                "@pointValue", data.PointValue);
            updateCommand.Parameters.AddWithValue(
                "@range", data.Range);
            updateCommand.Parameters.AddWithValue(
                "@frontArc", data.FrontArc);
            updateCommand.Parameters.AddWithValue(
                "@rearArc", data.RearArc);
            updateCommand.Parameters.AddWithValue(
                "@click", data.Click);

            try
            {
                connection.Open();
                int count = updateCommand.ExecuteNonQuery();
                return GetMageKnight(data.Name);
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"something is wrong UpdateMageKnight:{ex.ToString()}");
                throw;
            }
            finally
            {
                connection.Close();
            }
        }

        public static MageKnight GetMageKnight(string name)
        {
            SqlConnection connection = MageKnightDataDB.GetConnection();
            string selectStatement
                = "SELECT ID"
                + "FROM MageKnights"
                + "WHERE name = @name";
            SqlCommand selectCommand = new SqlCommand(selectStatement, connection);
            selectCommand.Parameters.AddWithValue("@name", name);

            try
            {
                connection.Open();
                MageData data = new MageData();
                SqlDataReader reader = selectCommand.ExecuteReader(System.Data.CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    data.Index = Int32.Parse(reader["index"].ToString());
                    data.Name= reader["name"].ToString();
                    data.Faction = reader["faction"].ToString();
                    data.Set = reader["set"].ToString();
                    data.PointValue = Int32.Parse(reader["pointValue"].ToString());
                    data.Range = Int32.Parse(reader["range"].ToString());
                    data.FrontArc = Int32.Parse(reader["frontArc"].ToString());
                    data.RearArc = Int32.Parse(reader["rearArc"].ToString());
                    data.Click = Int32.Parse(reader["click"].ToString());
                    //data.Stats = GetStats(name);
                    //data.Rank = GetRank(accountReader["rank"].ToString);
                    return new MageKnight(data);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"something is wrong GetMageKnight:{ex.ToString()}");
            }

            return null;
        }

        private static IRank GetRank(Func<string> toString)
        {
            throw new NotImplementedException();
        }

        private static IStats GetStats(string name)
        {
            throw new NotImplementedException();
        }
    }
}
