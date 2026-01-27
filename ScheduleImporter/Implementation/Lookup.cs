using Core.Constant;
using Microsoft.Data.SqlClient;
using System;

namespace ScheduleImporter.Implementation
{
    public class Lookup
    {
        static public int GetSubjectId(string subjectName)
        {
            using var conn = new SqlConnection(DBConstant.ConnectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT IdSubject FROM Subjects WHERE Name LIKE @x";
            cmd.Parameters.AddWithValue("@x", "%" + subjectName + "%");

            var result = cmd.ExecuteScalar();
            return result == null ? 0 : (int)result;
        }

        static public int GetProfesorId(string firstName, string lastName)
        {
            using var conn = new SqlConnection(DBConstant.ConnectionString);
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT IdUser FROM [Users] WHERE FirstName LIKE @f AND LastName LIKE @l";
            cmd.Parameters.AddWithValue("@f", "%" + firstName + "%");
            cmd.Parameters.AddWithValue("@l", "%" + lastName + "%");

            var userResult = cmd.ExecuteScalar();
            if (userResult == null)
                return 0;

            int idUser = (int)userResult;

            var cmdProf = conn.CreateCommand();
            cmdProf.CommandText = @"SELECT IdProfesor FROM Profesors WHERE IdUser=@u";
            cmdProf.Parameters.AddWithValue("@u", idUser);

            var profResult = cmdProf.ExecuteScalar();
            return profResult == null ? 0 : (int)profResult;
        }
    }
}