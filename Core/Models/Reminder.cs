
namespace TODO.Core.Models
{
    public class Reminder
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime Reminder_time { get; set; }
       
        public List<Tag>? Tags { get; set; } = new List<Tag>();
    }
}
