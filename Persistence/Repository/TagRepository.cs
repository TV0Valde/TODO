using Microsoft.EntityFrameworkCore;
using TODO.Core.Models;
using TODO.Application.Interfaces;
using TODO.Application.Common.Exceptions;
using TODO.Persistence;

namespace TODO.Persistence.Repository
{
    public class TagRepository : ITagRepository
    {
        private readonly AppDbContext _context;

        public TagRepository(AppDbContext context)
        {
            _context = context;
        }
        public void Delete(int id)
        {
            var tag = _context.Tags.Find(id);
            if (tag != null)
            {
                _context.Remove(tag);
            }
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await _context.Tags.Include(tag=>tag.Notes).ToListAsync();
        }

        public async Task<Tag?> GetByIdAsync(int id)
        {
            var tag = await _context.Tags.Include("Notes").Include("Reminders").FirstOrDefaultAsync(x => x.Id == id);
            return tag;
        }

        public async Task<Tag> CreateAsync(Tag tag)
        {
            await _context.Tags.AddAsync(tag);
            await SaveAsync();
            return tag;
        }

      

        public void Update(Tag tag)
        {
            var tagInDb = _context.Tags.Find(tag.Id);
            if (tagInDb != null)
            {
                tagInDb.Name = tag.Name;
            }
        }
        public async Task SaveAsync() { 
            await _context.SaveChangesAsync();
        }
        public async Task<Tag?> GetByNameAsync(string name)
        {
            return await _context.Tags.FirstOrDefaultAsync(t => t.Name == name);
        }
        public bool NameExist(string name) { 
            return _context.Tags.Any(t => t.Name == name);
        }
    }
}
