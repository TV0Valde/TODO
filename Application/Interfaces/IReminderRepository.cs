using TODO.Core.Models;

namespace TODO.Application.Interfaces
{
    public interface IReminderRepository
    {
        Task<IEnumerable<Reminder>> GetAllAsync();

        Task<Reminder?> GetByIdAsync(int id);

        Task<Reminder> CreateAsync(Reminder reminder);

        void Update(Reminder reminder);

        void Delete(int id);

        Task SaveAsync();

        Task SetTagsAsync(int reminderId, List<int> TagIds);

        bool TitleExist(string title);

    }
}
