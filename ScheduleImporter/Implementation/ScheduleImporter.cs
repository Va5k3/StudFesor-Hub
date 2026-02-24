using DAL.Abstraction;
using DAL.Implementation;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Entities;
using ScheduleImporter.Interface;
using System;
using System.Collections.Generic;
using System.IO;
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

            using var logWriter = new StreamWriter("import_debug.log", false, System.Text.Encoding.UTF8);

            string currentDay = "";

            foreach (var row in table.Elements<TableRow>().Skip(1))
            {
                var cells = row.Elements<TableCell>().ToList();

                // Preskoči redove koji nemaju dovoljno ćelija (spojeni redovi, separatori, itd.)
                if (cells.Count < 4)
                {
                    logWriter.WriteLine($"[SKIP] Red preskočen – samo {cells.Count} ćelija/e.");
                    continue;
                }

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
                string subjectCell = GetCellText(cells[2]);
                string subjectName = ExtractSubjectName(subjectCell);

                string profesorPart = ExtractProfesorPart(subjectCell);
                string profesorFirstName = ExtractProfesorFirstName(profesorPart);
                string profesorLastName = ExtractProfesorLastName(profesorPart);

                // cabinet
                string cabinetCell = cells[3].InnerText.Trim();
                int cabinet = ParseCabinet(cabinetCell);

                // Lookup IDs
                int subjectId = Lookup.GetSubjectId(subjectName);
                int profesorId = Lookup.GetProfesorId(profesorFirstName, profesorLastName);

                logWriter.WriteLine($"RAW CELL: '{subjectCell}'");
                logWriter.WriteLine($"  Parsed subject: '{subjectName}' -> Id={subjectId}");
                logWriter.WriteLine($"  Parsed profesor part: '{profesorPart}'");
                logWriter.WriteLine($"  Parsed profesor: '{profesorFirstName} {profesorLastName}' -> Id={profesorId}");
                logWriter.WriteLine();

                Console.WriteLine($"  [DEBUG] Subject: '{subjectName}' -> Id={subjectId}");
                Console.WriteLine($"  [DEBUG] Profesor: '{profesorFirstName} {profesorLastName}' -> Id={profesorId}");

                if (subjectId == 0)
                {
                    Console.WriteLine($"Upozorenje: Predmet '{subjectName}' nije pronađen u bazi.");
                    continue;
                }

                if (profesorId == 0)
                {
                    Console.WriteLine($"Upozorenje: Profesor '{profesorFirstName} {profesorLastName}' nije pronađen u bazi.");
                }

                // Validacija TimeSpan formata pre parsiranja
                if (!TimeSpan.TryParse(time, out TimeSpan parsedTime))
                {
                    logWriter.WriteLine($"[SKIP] Neispravan format vremena: '{time}'");
                    continue;
                }

                var schedule = new Schedule
                {
                    Day = currentDay,
                    Time = parsedTime,
                    Type = ExtractType(subjectCell),
                    Cabinet = cabinet,
                    IdSubject = subjectId > 0 ? (int?)subjectId : null,
                    IdProfesor = profesorId > 0 ? (int?)profesorId : null,
                    IdStudent = null
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

            logWriter.Flush();
            Console.WriteLine("\n>> Debug log saved to: import_debug.log");
        }

        private string GetCellText(TableCell cell)
        {
            var paragraphs = cell.Elements<Paragraph>();
            var parts = new List<string>();
            foreach (var p in paragraphs)
            {
                string pText = string.Join("", p.Elements<Run>().Select(r => r.InnerText));
                if (!string.IsNullOrWhiteSpace(pText))
                    parts.Add(pText.Trim());
            }
            return string.Join(" ", parts);
        }

        public string ExtractProfesorPart(string raw)
        {
            int closeParenIndex = raw.IndexOf(')');
            string profesorRaw;

            if (closeParenIndex >= 0 && closeParenIndex < raw.Length - 1)
            {
                profesorRaw = raw.Substring(closeParenIndex + 1).Trim();
            }
            else
            {
                var fallbackParts = raw.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (fallbackParts.Length >= 2)
                    profesorRaw = fallbackParts[^2] + " " + fallbackParts[^1];
                else
                    return raw;
            }

            var titlesToRemove = new[] { "Др", "др", "Др.", "др.", "Проф", "проф", "Проф.", "проф." };
            var words = profesorRaw.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                   .Where(w => !titlesToRemove.Contains(w))
                                   .ToArray();

            return string.Join(" ", words);
        }

        public string ExtractProfesorFirstName(string profesorPart)
        {
            var parts = profesorPart.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 2) return parts[^2];
            if (parts.Length == 1) return parts[0];
            return string.Empty;
        }

        public string ExtractProfesorLastName(string profesorPart)
        {
            var parts = profesorPart.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 1) return parts[^1];
            return string.Empty;
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
                return num;
            return 0;
        }
    }
}