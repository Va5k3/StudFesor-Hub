using DAL.Abstraction;
using DAL.Implementation;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Entities;
using ScheduleImporter.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ScheduleImporter.Implementation
{
    public class ScheduleImporter : IScheduleImporter
    {
        private readonly IRepositorySchedule _scheduleRepository;

        public ScheduleImporter(IRepositorySchedule scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }

        public void Import(string filePath)
        {
            using var doc = WordprocessingDocument.Open(filePath, false);
            var table = doc.MainDocumentPart.Document.Body.Elements<Table>().First();

            if (table == null)
            {
                throw new Exception("Table doesn't exist");
            }

            string currentDay = "";

            foreach (var row in table.Elements<TableRow>().Skip(1))
            {
                var cells = row.Elements<TableCell>().ToList();

                // day
                string dayCell = cells[0].InnerText.Trim();
                if (!string.IsNullOrWhiteSpace(dayCell))
                {
                    currentDay = dayCell;
                }

                // time
                string timeCell = cells[1].InnerText.Trim();
                if (string.IsNullOrWhiteSpace(timeCell))
                {
                    continue;
                }

                string time = timeCell.Replace(",", ":");

                // subject + profesor
                string subjectCell = cells[2].InnerText.Trim();
                string subjectName = ExtractSubjectName(subjectCell);
                string profesorFirstName = ExtractProfesorFirstName(subjectCell);
                string profesorLastName = ExtractProfesorLastName(subjectCell);

                // cabinet
                string cabinetCell = cells[3].InnerText.Trim();
                int cabinet = ParseCabinet(cabinetCell);

                // Lookup IDs
                int subjectId = Lookup.GetSubjectId(subjectName);
                int profesorId = Lookup.GetProfesorId(profesorFirstName, profesorLastName);

                if (subjectId == 0)
                {
                    Console.WriteLine($"Upozorenje: Predmet '{subjectName}' nije pronađen u bazi.");
                    continue;
                }

                if (profesorId == 0)
                {
                    Console.WriteLine($"Upozorenje: Profesor '{profesorFirstName} {profesorLastName}' nije pronađen u bazi.");
                }

                var schedule = new Schedule
                {
                    Day = currentDay,
                    Time = TimeSpan.Parse(time),
                    Type = ExtractType(subjectCell),
                    Cabinet = cabinet,
                    IdSubject = subjectId > 0 ? (int?)subjectId : null,
                    IdProfesor = null, //TREBA UBACITI PROFESORE, OVO JE ZA TEST
                    IdStudent = null  // POSTAVLJENO NA NULL 
                };

                try
                {
                    _scheduleRepository.Add(schedule);
                    Console.WriteLine($"   Added: {currentDay} {time}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  Failed: {ex.Message}");
                }
            }
        }

        public string ExtractProfesorFirstName(string raw)
        {
            var parts = raw.Split(" ");
            if (parts.Length < 2) return string.Empty;
            return parts[^2];  // Pretposlednji element (ime)
        }

        public string ExtractProfesorLastName(string raw)
        {
            var parts = raw.Split(" ");
            if (parts.Length < 1) return string.Empty;
            return parts[^1];  // Poslednji element (prezime)
        }

        public string ExtractSubjectName(string raw)
        {
            if (raw.Contains("("))
                return raw.Split("(")[0].Trim();
            return raw;
        }

        public string ExtractType(string raw)
        {
            int start = raw.IndexOf("(");
            int end = raw.IndexOf(")");
            if (start >= 0 && end > start)
                return raw.Substring(start + 1, end - start - 1);
            return "";
        }

        public int ParseCabinet(string raw)
        {
            if (raw.ToLower().Contains("лаб"))
                return -1;
            if (int.TryParse(raw, out int num))
            {
                return num;
            }
            return 0;
        }
    }
}