using System;

namespace ScheduleImporter.Interface
{
    public interface IScheduleImporter
    {
        public void Import(string filePath);
        public int ParseCabinet(string raw);
        public string ExtractType(string raw);
        public string ExtractSubjectName(string raw);
        public string ExtractProfesorFirstName(string raw);  // PROMENJEN
        public string ExtractProfesorLastName(string raw);   // DODAT
    }
}