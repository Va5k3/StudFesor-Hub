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
                cmd.CommandText = "SELECT IdUser, IdRole, FirstName, LastName, Email, PasswordHash FROM Users WHERE IdUser=@id";
                cmd.Parameters.AddWithValue("@id", id);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapUser(reader);
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
                cmd.CommandText = "SELECT IdUser, IdRole, FirstName, LastName, Email, PasswordHash FROM Users";

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(MapUser(reader));
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

        private static User MapUser(SqlDataReader reader)
        {
            int idUserOrd = reader.GetOrdinal("IdUser");
            int idRoleOrd = reader.GetOrdinal("IdRole");
            int firstOrd = reader.GetOrdinal("FirstName");
            int lastOrd = reader.GetOrdinal("LastName");
            int emailOrd = reader.GetOrdinal("Email");
            int passOrd = reader.GetOrdinal("PasswordHash");

            return new User
            {
                IdUser = reader.GetInt32(idUserOrd),
                IdRole = reader.GetInt32(idRoleOrd),
                FirstName = reader.IsDBNull(firstOrd) ? null : reader.GetString(firstOrd),
                LastName = reader.IsDBNull(lastOrd) ? null : reader.GetString(lastOrd),
                Email = reader.IsDBNull(emailOrd) ? null : reader.GetString(emailOrd),
                PasswordHash = reader.IsDBNull(passOrd) ? null : reader.GetString(passOrd)
            };
        }
    }
}