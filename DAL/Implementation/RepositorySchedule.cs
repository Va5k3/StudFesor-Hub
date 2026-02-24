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
            using (SqlConnection conn = new SqlConnection(DBConstant.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"INSERT INTO Schedules 
                    (IdSubject, IdStudent, IdProfesor, Day, Time, Type, Cabinet)
                    VALUES (@IdSubject, @IdStudent, @IdProfesor, @Day, @Time, @Type, @Cabinet)";
                cmd.Parameters.AddWithValue("@IdSubject",  (object)item.IdSubject  ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IdStudent",  (object)item.IdStudent  ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IdProfesor", (object)item.IdProfesor ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Day",        (object)item.Day        ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Time",       (object)item.Time       ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Type",       (object)item.Type       ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Cabinet",    (object)item.Cabinet    ?? DBNull.Value);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(DBConstant.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "DELETE FROM Schedules WHERE IdShedule=@id";
                cmd.Parameters.AddWithValue("@id", id);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool DeleteAll()
        {
            using (SqlConnection conn = new SqlConnection(DBConstant.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "DELETE FROM Schedules";
                cmd.ExecuteNonQuery();
                return true;
            }
        }

        public Schedule Get(int id)
        {
            using (SqlConnection conn = new SqlConnection(DBConstant.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"SELECT IdShedule, IdSubject, IdStudent, IdProfesor, 
                                           Day, Time, Type, Cabinet 
                                    FROM Schedules WHERE IdShedule=@id";
                cmd.Parameters.AddWithValue("@id", id);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                        return MapSchedule(reader);
                }
                return null;
            }
        }

        public List<Schedule> GetAll()
        {
            var list = new List<Schedule>();
            using (SqlConnection conn = new SqlConnection(DBConstant.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"SELECT IdShedule, IdSubject, IdStudent, IdProfesor, 
                                           Day, Time, Type, Cabinet 
                                    FROM Schedules";
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        list.Add(MapSchedule(reader));
                }
            }
            return list;
        }

        public List<ScheduleDTO> GetAllDetailed()
        {
            var list = new List<ScheduleDTO>();
            using (SqlConnection conn = new SqlConnection(DBConstant.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    SELECT s.IdShedule,
                           s.Day,
                           s.Time,
                           s.Type,
                           s.Cabinet,
                           ISNULL(sub.Name, 'N/A')                       AS SubjectName,
                           ISNULL(u.FirstName + ' ' + u.LastName, 'N/A') AS ProfessorName
                    FROM Schedules s
                    LEFT JOIN Subjects  sub ON s.IdSubject  = sub.IdSubject
                    LEFT JOIN Profesors p   ON s.IdProfesor = p.IdProfesor
                    LEFT JOIN Users     u   ON p.IdUser     = u.IdUser";

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new ScheduleDTO
                        {
                            Id            = reader.GetInt32(reader.GetOrdinal("IdShedule")),
                            Day           = reader.IsDBNull(reader.GetOrdinal("Day"))
                                                ? "/" : reader.GetString(reader.GetOrdinal("Day")),
                            TimeDisplay   = reader.IsDBNull(reader.GetOrdinal("Time"))
                                                ? "00:00" : reader.GetTimeSpan(reader.GetOrdinal("Time")).ToString(@"hh\:mm"),
                            Type          = reader.IsDBNull(reader.GetOrdinal("Type"))
                                                ? "/" : reader.GetString(reader.GetOrdinal("Type")),
                            Cabinet       = reader.IsDBNull(reader.GetOrdinal("Cabinet"))
                                                ? 0 : reader.GetInt32(reader.GetOrdinal("Cabinet")),
                            SubjectName   = reader.GetString(reader.GetOrdinal("SubjectName")),
                            ProfessorName = reader.GetString(reader.GetOrdinal("ProfessorName"))
                        });
                    }
                }
            }
            return list;
        }

        public bool Update(Schedule item)
        {
            using (SqlConnection conn = new SqlConnection(DBConstant.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = @"UPDATE Schedules SET
                                    IdSubject=@IdSubject, IdStudent=@IdStudent, IdProfesor=@IdProfesor,
                                    Day=@Day, Time=@Time, Type=@Type, Cabinet=@Cabinet
                                    WHERE IdShedule=@Id";
                cmd.Parameters.AddWithValue("@Id",         item.IdSchedule);
                cmd.Parameters.AddWithValue("@IdSubject",  (object)item.IdSubject  ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IdStudent",  (object)item.IdStudent  ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IdProfesor", (object)item.IdProfesor ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Day",        (object)item.Day        ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Time",       (object)item.Time       ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Type",       (object)item.Type       ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Cabinet",    (object)item.Cabinet    ?? DBNull.Value);
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public void InsertSchedule(Schedule schedule) => Add(schedule);

        private static Schedule MapSchedule(SqlDataReader reader)
        {
            return new Schedule
            {
                IdSchedule = reader.GetInt32(reader.GetOrdinal("IdShedule")),
                IdSubject  = reader.IsDBNull(reader.GetOrdinal("IdSubject"))  ? null : reader.GetInt32(reader.GetOrdinal("IdSubject")),
                IdStudent  = reader.IsDBNull(reader.GetOrdinal("IdStudent"))  ? null : reader.GetInt32(reader.GetOrdinal("IdStudent")),
                IdProfesor = reader.IsDBNull(reader.GetOrdinal("IdProfesor")) ? null : reader.GetInt32(reader.GetOrdinal("IdProfesor")),
                Day        = reader.IsDBNull(reader.GetOrdinal("Day"))        ? null : reader.GetString(reader.GetOrdinal("Day")),
                Time       = reader.IsDBNull(reader.GetOrdinal("Time"))       ? null : reader.GetTimeSpan(reader.GetOrdinal("Time")),
                Type       = reader.IsDBNull(reader.GetOrdinal("Type"))       ? null : reader.GetString(reader.GetOrdinal("Type")),
                Cabinet    = reader.IsDBNull(reader.GetOrdinal("Cabinet"))    ? null : reader.GetInt32(reader.GetOrdinal("Cabinet"))
            };
        }
    }
}