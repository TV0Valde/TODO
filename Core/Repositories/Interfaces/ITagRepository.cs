using TODO.Core.Models;

namespace TODO.Core.Repositories.Interfaces { 
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetAllAsync();

        Task<Tag?> GetByIdAsync(int id);
        Task<Tag> CreateAsync(Tag tag);

        void Update(Tag tag);
        void Delete(int id);
        Task<Tag?> GetByNameAsync(string name);
        Task SaveAsync();

        bool NameExist(string name);
      
    }
}
