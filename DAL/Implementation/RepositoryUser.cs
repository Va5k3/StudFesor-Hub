using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Constant;
using DAL.Abstraction;
using Entities;
using Microsoft.Data.SqlClient;

namespace DAL.Implementation
{
    public class RepositoryUser : IRepositoryUser
    {
        public void Add(User user)
        {
            using (SqlConnection conn = new SqlConnection(DBConstant.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "INSERT INTO Users (IdRole, FirstName, LastName, Email, PasswordHash) VALUES (@role, @first, @last, @email, @pass)";
                cmd.Parameters.AddWithValue("@role", user.IdRole);
                cmd.Parameters.AddWithValue("@first", (object)user.FirstName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@last", (object)user.LastName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@email", (object)user.Email ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@pass", (object)user.PasswordHash ?? DBNull.Value);
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(DBConstant.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "DELETE FROM Users WHERE IdUser=@id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        public User? Get(int id)
        {
            using (SqlConnection conn = new SqlConnection(DBConstant.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM Users WHERE IdUser=@id";
                cmd.Parameters.AddWithValue("@id", id);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new User
                        {
                            IdUser = reader.GetInt32(0),
                            IdRole = reader.GetInt32(1),
                            FirstName = reader.IsDBNull(2) ? null : reader.GetString(2),
                            LastName = reader.IsDBNull(3) ? null : reader.GetString(3),
                            Email = reader.IsDBNull(4) ? null : reader.GetString(4),
                            PasswordHash = reader.IsDBNull(5) ? null : reader.GetString(5)
                        };
                    }
                }
            }
            return null;
        }

        public List<User> GetAll()
        {
            List<User> users = new List<User>();
            using (SqlConnection conn = new SqlConnection(DBConstant.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM Users";

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            IdUser = reader.GetInt32(0),
                            IdRole = reader.GetInt32(1),
                            FirstName = reader.IsDBNull(2) ? null : reader.GetString(2),
                            LastName = reader.IsDBNull(3) ? null : reader.GetString(3),
                            Email = reader.IsDBNull(4) ? null : reader.GetString(4),
                            PasswordHash = reader.IsDBNull(5) ? null : reader.GetString(5)
                        });
                    }
                }
            }
            return users;
        }

        public void Update(User user)
        {
            using (SqlConnection conn = new SqlConnection(DBConstant.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE Users SET IdRole=@role, FirstName=@first, LastName=@last, Email=@email, PasswordHash=@pass WHERE IdUser=@id";
                cmd.Parameters.AddWithValue("@id", user.IdUser);
                cmd.Parameters.AddWithValue("@role", user.IdRole);
                cmd.Parameters.AddWithValue("@first", (object)user.FirstName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@last", (object)user.LastName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@email", (object)user.Email ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@pass", (object)user.PasswordHash ?? DBNull.Value);
                cmd.ExecuteNonQuery();
            }
        }
    }
}