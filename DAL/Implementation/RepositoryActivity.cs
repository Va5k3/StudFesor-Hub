using Core.Interface;
using Core.Constant;
using DAL.Abstraction;
using Entities;
using Microsoft.Data.SqlClient;
using DAL.Implementations;
using System.Collections.Generic;
using System.Linq;
using System;

namespace DAL.Implementations
{
    public class RepositoryActivity : IRepositoryActivity
    {
        public void Add(Activity activity)
        {
            Console.WriteLine("USAO U REPOSITORY ADD");

            try
            {
            using (SqlConnection conn = new SqlConnection(DBConstant.ConnectionString))
            {
                conn.Open();
                    Console.WriteLine("SQL CONNECTION OPEN");

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"INSERT INTO Activities (Header, Paragraph, Type, Deadline, CreatedUserId) 
                                   VALUES (@header, @paragraph, @type, @deadline, @userId)";

                cmd.Parameters.AddWithValue("@header", activity.Header);
                cmd.Parameters.AddWithValue("@paragraph", activity.Paragraph);
                cmd.Parameters.AddWithValue("@type", activity.Type);
                cmd.Parameters.AddWithValue("@deadline", activity.Deadline);
                cmd.Parameters.AddWithValue("@userId", activity.CreatedUserId);
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(DBConstant.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "DELETE FROM Activities WHERE IdActivity=@id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        public Activity? Get(int id)
        {
            using (SqlConnection conn = new SqlConnection(DBConstant.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM Activities WHERE IdActivity=@id";
                cmd.Parameters.AddWithValue("@id", id);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Activity
                        {
                            IdActivity = reader.GetInt32(0),
                            Header = reader.GetString(1),
                            Paragraph = reader.GetString(2),
                            Type = reader.GetString(3),
                            Deadline = reader.GetDateTime(4),
                            CreatedUserId = reader.GetInt32(5)
                        };
                    }
                }
            }
            return null;
        }

        public List<Activity> GetAll()
        {
            List<Activity> activities = new List<Activity>();
            using (SqlConnection conn = new SqlConnection(DBConstant.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM Activities ORDER BY Deadline ASC";

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        activities.Add(new Activity
                        {
                            IdActivity = reader.GetInt32(0),
                            Header = reader.GetString(1),
                            Paragraph = reader.GetString(2),
                            Type = reader.GetString(3),
                            Deadline = reader.GetDateTime(4),
                            CreatedUserId = reader.GetInt32(5)
                        });
                    }
                }
            }
            return activities;
        }

        public List<Activity> GetByUserId(int userId)
        {
            List<Activity> activities = new List<Activity>();
            using (SqlConnection conn = new SqlConnection(DBConstant.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM Activities WHERE CreatedUserId=@userId ORDER BY Deadline ASC";
                cmd.Parameters.AddWithValue("@userId", userId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        activities.Add(new Activity
                        {
                            IdActivity = reader.GetInt32(0),
                            Header = reader.GetString(1),
                            Paragraph = reader.GetString(2),
                            Type = reader.GetString(3),
                            Deadline = reader.GetDateTime(4),
                            CreatedUserId = reader.GetInt32(5)
                        });
                    }
                }
            }
            return activities;
        }

        public void Update(Activity activity)
        {
            using (SqlConnection conn = new SqlConnection(DBConstant.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"UPDATE Activities 
                                   SET Header=@header, Paragraph=@paragraph, Type=@type, 
                                       Deadline=@deadline, CreatedUserId=@userId 
                                   WHERE IdActivity=@id";
                cmd.Parameters.AddWithValue("@id", activity.IdActivity);
                cmd.Parameters.AddWithValue("@header", activity.Header);
                cmd.Parameters.AddWithValue("@paragraph", activity.Paragraph);
                cmd.Parameters.AddWithValue("@type", activity.Type);
                cmd.Parameters.AddWithValue("@deadline", activity.Deadline);
                cmd.Parameters.AddWithValue("@userId", activity.CreatedUserId);
                cmd.ExecuteNonQuery();
            }
        }
    }
}