﻿using MKViewModel;
using Org.BouncyCastle.Asn1;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MKModel
{
    public class  MageDB
    {
        public static void ResetDB()
        {
            DeleteTable("AllMageKnights");
            DeleteTable("ClickAbilities");
            DeleteTable("ClickValues");
        }
        public static void DeleteTable(string table)
        {
            SqlConnection connection = MageKnightDataDB.GetConnection();
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
                MessageBox.Show(ex.ToString());
                throw;
            }
            finally
            {
                connection.Close();
            }
        }


        public static MageKnight GenerateMageKnight(MageData data)
        {
            SqlConnection connection = MageKnightDataDB.GetConnection();
            string insertStatement =
                    "INSERT Into AllMageKnights " +
                    "(Id, [Index], [Set], Name, PointValue, Faction, FrontArc, Targets, Range, Rank, Rarity, ModelImage, PriceValue)" +
                    "VALUES(@Id, @Index, @Set, @Name, @PointValue, @Faction, @FrontArc, @Targets, @Range, @Rank, @Rarity, @ModelImage, @PriceValue)";

            SqlCommand insertCommand = new SqlCommand(insertStatement, connection);
            insertCommand.Parameters.AddWithValue(
                "@Id", data.Id);

            insertCommand.Parameters.AddWithValue(
                "@Index", data.Index);
            insertCommand.Parameters.AddWithValue(
                "@Set", data.Set);
            insertCommand.Parameters.AddWithValue(
                "@Name", data.Name);
            insertCommand.Parameters.AddWithValue(
                "@PointValue", data.PointValue);
            insertCommand.Parameters.AddWithValue(
                "@Faction", data.Faction);
            insertCommand.Parameters.AddWithValue(
                "@FrontArc", data.FrontArc);
            insertCommand.Parameters.AddWithValue(
                "@Targets", data.Targets);
            insertCommand.Parameters.AddWithValue(
                "@Range", data.Range);
            insertCommand.Parameters.AddWithValue(
                "@Rank", data.Rank);
            insertCommand.Parameters.AddWithValue(
                "@Rarity", data.Rarity);

            //FileStream fs = new FileStream(data.ModelImage, FileMode.Open, FileAccess.Read); //Path is image location 
            //Byte[] bindata = new byte[Convert.ToInt32(fs.Length)];
            //fs.Read(bindata, 0, Convert.ToInt32(fs.Length));

            insertCommand.Parameters.AddWithValue(
                "@ModelImage", data.ModelImage);
            insertCommand.Parameters.AddWithValue(
                "@PriceValue", data.PriceValue);

            try
            {
                connection.Open();
                int count = insertCommand.ExecuteNonQuery();
                if (count < 0)
                {
                    return null;
                }
                else
                {
                    return GetMageKnight(data.Id);
                }
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

        public static void update()
        {
            SqlConnection connection = MageKnightDataDB.GetConnection();
            string selectStatement1
                   = "SELECT Speed, Attack, Defense, Damage, Id "
                   + "FROM ClickValues ";
            SqlCommand selectCommand1 = new SqlCommand(selectStatement1, connection);
            string selectStatement2
                 = "SELECT Speed, Attack, Defense, Damage, Id "
                 + "FROM ClickValues ";
            SqlCommand selectCommand2= new SqlCommand(selectStatement2, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = selectCommand1.ExecuteReader();


                List<int> speeds = new List<int>();
                List<int> attacks = new List<int>();
                List<Guid> guids = new List<Guid>();
                while (reader.Read())
                {
                    speeds.Add(Int32.Parse(reader["Speed"].ToString()));
                    attacks.Add(Int32.Parse(reader["Speed"].ToString()));
                    guids.Add(Guid.Parse(reader["Id"].ToString()));
                }

                connection.Close();

                foreach (Guid g in guids)
                {
                    foreach (int s in speeds)
                    {
                        foreach (int a in attacks)
                        {
                            UpdateClickValues(a, s, g);

                            break;
                        }

                        break;
                    }
                }

                connection.Open();
                SqlDataReader reader2 = selectCommand2.ExecuteReader();
                List<string> speedAbilities = new List<string>();
                List<string> attackAbilities = new List<string>();
                while (reader2.Read())
                {

                    speedAbilities.Add(reader2["Speed"].ToString());
                    attackAbilities.Add(reader2["Attack"].ToString());
                }

                connection.Close();
                foreach (Guid g in guids)
                {
                    foreach (string s in speedAbilities)
                    {
                        foreach (string a in attackAbilities)
                        {

                            UpdateClickAbilities(a, s, g);
                            break;
                        }

                        break;
                    }
                }
               

            }
            catch (Exception ex)
            {
                MessageBox.Show($"something is wrong GetMageKnight:{ex.ToString()}");
            }
        }

        private static void UpdateClickValues(int speed, int attack, Guid id)
        {
            SqlConnection connection = MageKnightDataDB.GetConnection();
            string updateStatement =
                           "UPDATE ClickValues SET " +
                           "Speed = @Speed, " +
                           "Attack = @Attack " +
                           "WHERE Id = @Id";

            SqlCommand insertCommand = new SqlCommand(updateStatement, connection);
            insertCommand.Parameters.AddWithValue(
                "@Id", id);
            insertCommand.Parameters.AddWithValue(
                "@Speed", speed);
            insertCommand.Parameters.AddWithValue(
                "@Attack", attack);


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
        private static void UpdateClickAbilities(string speedAblity, string attackAbility, Guid id)
        {
            SqlConnection connection = MageKnightDataDB.GetConnection();

            string updateStatement =
                           "UPDATE ClickAbilities SET " +
                           "Speed = @Speed, " +
                           "Attack = @Attack " +
                           "WHERE Id = @Id";

            SqlCommand insertCommand = new SqlCommand(updateStatement, connection);
            insertCommand.Parameters.AddWithValue(
                "@Id", id);
            insertCommand.Parameters.AddWithValue(
                "@Speed", speedAblity);
            insertCommand.Parameters.AddWithValue(
                "@Attack", attackAbility);

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

        public static List<IMageKnightModel> GetMageKnights()
        {
            List<IMageKnightModel> mageKnights = new List<IMageKnightModel>();

              SqlConnection connection = MageKnightDataDB.GetConnection();
            string selectStatement
                = "SELECT Id, [Index], [Set], Name, PointValue, Faction, FrontArc, Targets, Range, Rank, Rarity, ModelImage "
                + "FROM AllMageKnights ";
            SqlCommand selectCommand = new SqlCommand(selectStatement, connection);

            try
            {
                connection.Open();
               
                SqlDataReader reader = selectCommand.ExecuteReader();

                
                while (reader.Read())
                {
                    MageData data = new MageData();
                    data.Id = Guid.Parse(reader["Id"].ToString());
                    data.Index = Int32.Parse(reader["Index"].ToString());
                    data.Name = reader["Name"].ToString();
                    data.Faction = reader["Faction"].ToString();
                    data.Set = reader["Set"].ToString();
                    data.PointValue = Int32.Parse(reader["PointValue"].ToString());
                    data.Range = Int32.Parse(reader["Range"].ToString());
                    data.FrontArc = Int32.Parse(reader["FrontArc"].ToString());
                    data.Targets = Int32.Parse(reader["Targets"].ToString());
                    data.Rank = reader["Rank"].ToString(); 
                    data.Faction = reader["Faction"].ToString();
                    data.ModelImage = reader["ModelImage"] as byte[];
                    data.Dial = GetDialStats(data);
                    IMageKnightModel mage = new MageKnight(data);
                    mageKnights.Add(mage);
                }

                

                connection.Close();

                return mageKnights;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"something is wrong GetMageKnight:{ex.ToString()}");
            }

            return null;
        }

        private static IDial GetDialStats(MageData data)
        {
            SqlConnection connection = MageKnightDataDB.GetConnection();
            string selectStatement
                = "SELECT Speed, Attack, Defense, Damage, [Index] "
                + "FROM ClickValues "
                + "WHERE Id = @Id";
            SqlCommand selectCommand = new SqlCommand(selectStatement, connection);
            selectCommand.Parameters.AddWithValue("@Id", data.Id);
            IDial dial = new Dial(data);
            try
            {
                connection.Open();
                SqlDataReader reader = selectCommand.ExecuteReader();
                while (reader.Read())
                {
                    IStat speed = new Stat(StatType.Speed);
                    IStat attack = new Stat(StatType.Attack);
                    IStat defense = new Stat(StatType.Defense);
                    IStat damage = new Stat(StatType.Damage);

                    speed.Value = Int32.Parse(reader["Speed"].ToString());
                    attack.Value = Int32.Parse(reader["Attack"].ToString());
                    defense.Value = Int32.Parse(reader["Defense"].ToString());
                    damage.Value = Int32.Parse(reader["Damage"].ToString());
                    int index = Int32.Parse(reader["Index"].ToString());
                    IClick click = new Click(speed, attack, defense, damage, index);
                    dial.Clicks.Add(click);
                }
                
                connection.Close();
                return FillDialSpecialAbilities(dial, data.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"something is wrong GetMageKnight:{ex.ToString()}");
            }

            return null;
        }

        private static IDial FillDialSpecialAbilities(IDial dial, Guid id)
        {
            SqlConnection connection = MageKnightDataDB.GetConnection();
            string selectStatement
                = "SELECT Speed, Attack, Defense, Damage, [Index] "
                + "FROM ClickAbilities "
                + "WHERE Id = @Id";
            SqlCommand selectCommand = new SqlCommand(selectStatement, connection);
            selectCommand.Parameters.AddWithValue("@Id", id);
            try
            {
                connection.Open();
                SqlDataReader reader = selectCommand.ExecuteReader();
                while (reader.Read())
                {
                    int index = Int32.Parse(reader["Index"].ToString());
                    IClick click = dial.Clicks.First(x => x.Index == index);
                    click.Speed.Ability = reader["Speed"].ToString();
                    click.Attack.Ability = reader["Attack"].ToString();
                    click.Defense.Ability = reader["Defense"].ToString();
                    click.Damage.Ability = reader["Damage"].ToString();
                }

                return dial;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"something is wrong GetMageKnight:{ex.ToString()}");
            }

            return null;
        }

        public static MageKnight GetMageKnight(Guid Id)
        {
            SqlConnection connection = MageKnightDataDB.GetConnection();
            string selectStatement
                = "SELECT Id, [Index], [Set], Name, PointValue, Faction, FrontArc, Targets, Range, Rank, Rarity "
                + "FROM AllMageKnights "
                + "WHERE Id = @Id";
            SqlCommand selectCommand = new SqlCommand(selectStatement, connection);
            selectCommand.Parameters.AddWithValue("@Id", Id);

            try
            {
                connection.Open();
                MageData data = new MageData();
                SqlDataReader reader = selectCommand.ExecuteReader(System.Data.CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    data.Index = Int32.Parse(reader["Index"].ToString());
                    data.Name= reader["Name"].ToString();
                    data.Faction = reader["Faction"].ToString();
                    data.Set = reader["Set"].ToString();
                    data.PointValue = Int32.Parse(reader["PointValue"].ToString());
                    data.Range = Int32.Parse(reader["Range"].ToString());
                    data.FrontArc = Int32.Parse(reader["FrontArc"].ToString());
                    data.Targets = Int32.Parse(reader["Targets"].ToString());
                    data.Rank = reader["Rank"].ToString();
                    data.Id = Guid.Parse(reader["Id"].ToString());
                }

                connection.Close();

                return new MageKnight(data);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"something is wrong GetMageKnight:{ex.ToString()}");
            }
            finally
            {
                connection.Close();
            }

            return null;
        }

        public static void CreateDialStats(List<IClick> clicks, int number)
        {
            SqlConnection connection = MageKnightDataDB.GetConnection();
            string selectStatement
                = "SELECT Id "
                + "FROM AllMageKnights "
                + "WHERE [Index] = @number";
            SqlCommand selectCommand = new SqlCommand(selectStatement, connection);
            selectCommand.Parameters.AddWithValue("@number", number);

            try
            {
                connection.Open();
                MageData data = new MageData();
                SqlDataReader reader = selectCommand.ExecuteReader(System.Data.CommandBehavior.SingleRow);
                if (reader.Read())
                {
                    Guid id = Guid.Parse(reader["Id"].ToString());
                    InsertClickValues(clicks, id);
                    InsertClickAbilites(clicks, id);
                }

                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"something is wrong GetMageKnight:{ex.ToString()}");
            }
        }

        private static void InsertClickAbilites(List<IClick> clicks, Guid id)
        {
            SqlConnection connection = MageKnightDataDB.GetConnection();
            foreach (IClick click in clicks)
            {
                string insertStatement =
                    "INSERT Into ClickAbilities " +
                    "(Id, Speed, Attack, Defense, Damage, [Index])" +
                    "VALUES(@Id, @Speed, @Attack, @Defense, @Damage, @Index)";

                SqlCommand insertCommand = new SqlCommand(insertStatement, connection);
                insertCommand.Parameters.AddWithValue(
                    "@Id", id);
                insertCommand.Parameters.AddWithValue(
                    "@Speed", click.Speed.Ability);
                insertCommand.Parameters.AddWithValue(
                    "@Attack", click.Attack.Ability);
                insertCommand.Parameters.AddWithValue(
                    "@Defense", click.Defense.Ability);
                insertCommand.Parameters.AddWithValue(
                    "@Damage", click.Damage.Ability);
                insertCommand.Parameters.AddWithValue(
                    "@Index", clicks.IndexOf(click));

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
        }

        private static void InsertClickValues(List<IClick> clicks, Guid id)
        {
            SqlConnection connection = MageKnightDataDB.GetConnection();
            foreach (IClick click in clicks)
            {
                string insertStatement =
                    "INSERT Into ClickValues " +
                    "(Id, Speed, Attack, Defense, Damage, [Index])" +
                    "VALUES(@Id, @Speed, @Attack, @Defense, @Damage, @Index)";

                SqlCommand insertCommand = new SqlCommand(insertStatement, connection);
                insertCommand.Parameters.AddWithValue(
                    "@Id", id);
                insertCommand.Parameters.AddWithValue(
                    "@Speed", click.Speed.Value);
                insertCommand.Parameters.AddWithValue(
                    "@Attack", click.Attack.Value);
                insertCommand.Parameters.AddWithValue(
                    "@Defense", click.Defense.Value);
                insertCommand.Parameters.AddWithValue(
                    "@Damage", click.Damage.Value);
                insertCommand.Parameters.AddWithValue(
                    "@Index", clicks.IndexOf(click));

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
        }
    }
}
