using Core.Constant;
using DAL.Abstraction;
using Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace DAL.Implementation
{
    public class RepositorySchedule : IRepositorySchedule
    {
        public bool Add(Schedule item)
        {
            using (SqlConnection sqlConnection = new SqlConnection(DBConstant.ConnectionString))
            {
                sqlConnection.Open();
                SqlCommand cmd = sqlConnection.CreateCommand();
                cmd.CommandText = @"INSERT INTO Schedules (IdSubject, IdStudent, IdProfesor, Day, Time, Type, Cabinet) 
                                    VALUES (@IdSubject, @IdStudent, @IdProfesor, @Day, @Time, @Type, @Cabinet)";

                cmd.Parameters.AddWithValue("@IdSubject", (object)item.IdSubject ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IdStudent", (object)item.IdStudent ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IdProfesor", (object)item.IdProfesor ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Day", (object)item.Day ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Time", item.Time);
                cmd.Parameters.AddWithValue("@Type", (object)item.Type ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Cabinet", (object)item.Cabinet ?? DBNull.Value);

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public bool Delete(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(DBConstant.ConnectionString))
            {
                sqlConnection.Open();
                SqlCommand cmd = sqlConnection.CreateCommand();
                cmd.CommandText = "DELETE FROM Schedules WHERE IdSchedule=@id";
                cmd.Parameters.AddWithValue("@id", id);

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public Schedule Get(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(DBConstant.ConnectionString))
            {
                sqlConnection.Open();
                SqlCommand cmd = sqlConnection.CreateCommand();
                cmd.CommandText = "SELECT * FROM Schedules WHERE IdSchedule=@id";
                cmd.Parameters.AddWithValue("@id", id);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Schedule
                        {
                            IdSchedule = reader.GetInt32(0),
                            IdSubject = reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
                            IdStudent = reader.IsDBNull(2) ? 0 : reader.GetInt32(2),
                            IdProfesor = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                            Day = reader.IsDBNull(4) ? null : reader.GetString(4),
                            Time = reader.GetTimeSpan(5),
                            Type = reader.IsDBNull(6) ? null : reader.GetString(6),
                            Cabinet = reader.IsDBNull(7) ? null : reader.GetInt32(7)
                        };
                    }
                }
                return null;
            }
        }

        public List<Schedule> GetAll()
        {
            List<Schedule> list = new List<Schedule>();
            using (SqlConnection sqlConnection = new SqlConnection(DBConstant.ConnectionString))
            {
                sqlConnection.Open();
                SqlCommand cmd = sqlConnection.CreateCommand();
                cmd.CommandText = "SELECT * FROM Schedules";

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Schedule
                        {
                            IdSchedule = reader.GetInt32(0),
                            IdSubject = reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
                            IdStudent = reader.IsDBNull(2) ? 0 : reader.GetInt32(2),
                            IdProfesor = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                            Day = reader.IsDBNull(4) ? null : reader.GetString(4),
                            Time = reader.GetTimeSpan(5),
                            Type = reader.IsDBNull(6) ? null : reader.GetString(6),
                            Cabinet = reader.IsDBNull(7) ? null : reader.GetInt32(7),
                        });
                    }
                }
            }
            return list;
        }

        public bool Update(Schedule item)
        {
            using (SqlConnection sqlConnection = new SqlConnection(DBConstant.ConnectionString))
            {
                sqlConnection.Open();
                SqlCommand cmd = sqlConnection.CreateCommand();
                cmd.CommandText = @"UPDATE Schedules SET 
                                    IdSubject=@IdSubject, IdStudent=@IdStudent, IdProfesor=@IdProfesor, 
                                    Day=@Day, Time=@Time, Type=@Type, Cabinet=@Cabinet 
                                    WHERE IdSchedule=@Id";

                cmd.Parameters.AddWithValue("@Id", item.IdSchedule);
                cmd.Parameters.AddWithValue("@IdSubject", (object)item.IdSubject ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IdStudent", (object)item.IdStudent ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IdProfesor", (object)item.IdProfesor ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Day", (object)item.Day ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Time", item.Time);
                cmd.Parameters.AddWithValue("@Type", (object)item.Type ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Cabinet", (object)item.Cabinet ?? DBNull.Value);

                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public void InsertSchedule(Schedule schedule)
        {
            Add(schedule);
        }
    }
}