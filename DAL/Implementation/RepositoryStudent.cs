using DAL.Abstraction;
using Entities;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Core.Constant;

namespace DAL.Implementation
{
    public class RepositoryStudent : IRepositoryStudent
    {
        public bool Add(Student item)
        {
            using (SqlConnection sqlConnection = new SqlConnection(DBConstant.ConnectionString))
            {
                sqlConnection.Open();
                SqlCommand cmd = sqlConnection.CreateCommand();
                cmd.CommandText = "INSERT INTO Students (StudIndex, IdUser, Year, Major) VALUES (@StudIndex, @IdUser, @Year, @Major)";

                cmd.Parameters.AddWithValue("@StudIndex", item.StudIndex);
                cmd.Parameters.AddWithValue("@IdUser", item.IdUser);
                cmd.Parameters.AddWithValue("@Year", item.Year);
                cmd.Parameters.AddWithValue("@Major", item.Major);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Delete(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(DBConstant.ConnectionString))
            {
                sqlConnection.Open();
                SqlCommand cmd = sqlConnection.CreateCommand();
                cmd.CommandText = "DELETE FROM Students WHERE IdStudent=@id";
                cmd.Parameters.AddWithValue("@id", id);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public Student Get(int id)
        {
            using (SqlConnection sqlConnection = new SqlConnection(DBConstant.ConnectionString))
            {
                sqlConnection.Open();
                SqlCommand cmd = sqlConnection.CreateCommand();
                cmd.CommandText = "SELECT * FROM Students WHERE IdStudent=@id";
                cmd.Parameters.AddWithValue("@id", id);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Student
                        {
                            IdStudent = reader.GetInt32(0),
                            StudIndex = reader.IsDBNull(1) ? null : reader.GetString(1),
                            Year = reader.IsDBNull(2) ? 0 : reader.GetInt32(2),
                            Major = reader.IsDBNull(3) ? null : reader.GetString(3),
                            IdUser = reader.IsDBNull(4) ? 0 : reader.GetInt32(4)
                        };
                    }
                }
                return null;
            }
        }

        public List<Student> GetAll()
        {
            List<Student> students = new List<Student>();
            using (SqlConnection sqlConnection = new SqlConnection(DBConstant.ConnectionString))
            {
                sqlConnection.Open();
                SqlCommand cmd = sqlConnection.CreateCommand();
                cmd.CommandText = "SELECT * FROM Students";

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        students.Add(new Student
                        {
                            IdStudent = reader.GetInt32(0),
                            StudIndex = reader.IsDBNull(1) ? null : reader.GetString(1),
                            Year = reader.IsDBNull(2) ? 0 : reader.GetInt32(2),
                            Major = reader.IsDBNull(3) ? null : reader.GetString(3),
                            IdUser = reader.IsDBNull(4) ? 0 : reader.GetInt32(4)
                        });
                    }
                }
            }
            return students;
        }

        public bool Update(Student item)
        {
            using (SqlConnection sqlConnection = new SqlConnection(DBConstant.ConnectionString))
            {
                sqlConnection.Open();
                SqlCommand cmd = sqlConnection.CreateCommand();
                cmd.CommandText = @"UPDATE Students SET 
                                    StudIndex=@StudIndex, Year=@Year, Major=@Major, IdUser=@IdUser 
                                    WHERE IdStudent=@Id";

                cmd.Parameters.AddWithValue("@Id", item.IdStudent);
                cmd.Parameters.AddWithValue("@StudIndex", item.StudIndex);
                cmd.Parameters.AddWithValue("@Year", item.Year);
                cmd.Parameters.AddWithValue("@Major", item.Major);
                cmd.Parameters.AddWithValue("@IdUser", item.IdUser);

                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public void InsertStudent(Student student)
        {
            Add(student);
        }
    }
}