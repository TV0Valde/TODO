using Microsoft.EntityFrameworkCore;
using TODO.Core.Models;
using TODO.Application.Interfaces;
using TODO.Application.Common.Exceptions;
using TODO.Persistence;

namespace TODO.Persistence.Repository
{
    public class ReminderRepository : IReminderRepository
    {
        private readonly AppDbContext _context;
        public ReminderRepository(AppDbContext context)
        {
            _context = context;
        }
        public void Delete(int id)
        {
            var reminder = _context.Reminders.Find(id);
            if (reminder != null)
            {
                _context.Remove(reminder);
            }
        }

        public async Task<IEnumerable<Reminder>> GetAllAsync()
        {
            var query = _context.Reminders.Include("Tags");
            return await query.ToListAsync();
        }

        public async Task<Reminder?> GetByIdAsync(int id)
        {
            var reminder = await _context.Reminders.Include("Tags").FirstOrDefaultAsync(x => x.Id == id);
            return reminder;
        }

        public async Task<Reminder> CreateAsync(Reminder reminder)
        {
               await _context.Reminders.AddAsync(reminder);
               await SaveAsync();
            return reminder;
        }

        public void Update(Reminder reminder)
        {
            var reminderInDb = _context.Reminders.Find(reminder.Id);
            if (reminderInDb != null) { 
            reminderInDb.Title = reminder.Title;
            reminderInDb.Description = reminder.Description;
            reminderInDb.Reminder_time = reminder.Reminder_time;
            }
        }

        public  async Task SaveAsync() { 
            await _context.SaveChangesAsync();
        }

        public async Task SetTagsAsync(int reminderId,List<int> tagIds) {
            var reminder = await _context.Reminders.Include("Tags").FirstOrDefaultAsync(x=>x.Id == reminderId);
            if (reminder == null) {
                throw new NotFoundException(nameof(reminder), reminderId);
            }
            var tags = await _context.Tags.Where(t=> tagIds.Contains(t.Id)).ToListAsync();
            reminder.Tags = tags;

            await SaveAsync();

        }

        public bool TitleExist(string title) { 
            return _context.Reminders.Any(x=>x.Title == title);
        }
       
    }
}
