using System.Text.Json.Serialization;

namespace TODO.Core.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public List<Note>? Notes { get; set; } = new List<Note>();
        [JsonIgnore]
        public List<Reminder>? Reminders { get; set; } = new List<Reminder>();
    }
}
