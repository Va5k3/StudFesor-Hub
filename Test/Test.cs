using DAL.Abstraction;
using DAL.Implementation;
using Entities;
using ScheduleImporter.Implementation;
using System;

namespace Test
{
    public class Test
    {
        static void Main(string[] args)
        {
            // Test importa rasporeda iz Word dokumenta
            Console.WriteLine("=== IMPORT SCHEDULE FROM WORD ===");
            IRepositorySchedule scheduleRepository = new RepositorySchedule();
            var importer = new ScheduleImporter.Implementation.ScheduleImporter(scheduleRepository);
            string filePath = @"C:\Users\Vaske\Downloads\IT-VII.docx";
            //TEEEEST PUUUUUSHHH
            try
            {
                importer.Import(filePath);
                Console.WriteLine("Import completed successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Import failed: {ex.Message}");
            }

            Console.WriteLine("\n=== TEST STUDENT DATA ===");
            // Test studenta
            IRepositoryStudent stud = new RepositoryStudent();
            Student user = stud.Get(9);
            Console.WriteLine("Index : " + user.StudIndex);
            Console.WriteLine("Major : " + user.Major);
            Console.WriteLine("Year : " + user.Year);
            Console.WriteLine("IdStudent : " + user.IdStudent);

            Console.WriteLine("\n=== TEST SCHEDULE DATA ===");
            // Test rasporeda
            IRepositorySchedule schedule = new RepositorySchedule();
            Schedule user2 = schedule.Get(10);
            Console.WriteLine("IdSchedule : " + user2.IdSchedule);
            Console.WriteLine("IdSubject : " + user2.IdSubject);
            Console.WriteLine("IdStudent : " + user2.IdStudent);
            Console.WriteLine("IdProfesor : " + user2.IdProfesor);
            Console.WriteLine("Day : " + user2.Day);
            Console.WriteLine("Time : " + user2.Time);
            Console.WriteLine("Cabinet : " + user2.Cabinet);
            Console.WriteLine("Type : " + user2.Type);

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}