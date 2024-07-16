using TODO.Core.Models;

namespace TODO.Application.Interfaces
{
    public interface INoteRepository
    {
        Task<IEnumerable<Note>> GetAllAsync();

        Task<Note?> GetByIdAsync(int id);

        Task<Note> CreateAsync(Note note);

        void Update(Note note);

        void Delete(int id);
        Task SaveAsync();

        Task SetTagsAsync(int noteId, List<int> TagIds);

        bool TitleExist(string title);

    }
}
