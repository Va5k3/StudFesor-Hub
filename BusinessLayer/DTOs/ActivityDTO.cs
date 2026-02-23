using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DTOs
{
    public class ActivityDTO
    {
        public string Header { get; set; } = string.Empty;
        public string Paragraph { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public DateTime Deadline { get; set; }
    }
}