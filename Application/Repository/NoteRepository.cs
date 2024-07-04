using Microsoft.EntityFrameworkCore;
using TODO.Core.Models;
using TODO.Persistence.Interfaces;
using TODO.Application.Common.Exceptions;
using TODO.Persistence;

namespace TODO.Application.Repository
{
    public class NoteRepository : INoteRepository
    {
        private readonly AppDbContext _context;

        public NoteRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Delete(int id)
        {
            var note = _context.Notes.Find(id);
            if (note != null)
            {
                _context.Remove(note);
            }
        }

        public async Task<IEnumerable<Note>> GetAllAsync()
        {
            var query = _context.Notes.Include("Tags");
            return await query.ToListAsync();
        }

        public async Task<Note?> GetByIdAsync(int id)
        {
            var note = await _context.Notes.Include("Tags").FirstOrDefaultAsync(x => x.Id == id);
            return note;
        }

        public async Task<Note> CreateAsync(Note note)
        {
            await _context.Notes.AddAsync(note);
            await SaveAsync();
            return note;
        }

        public void Update(Note note)
        {
            var noteInDb = _context.Notes.Find(note.Id);
            if (noteInDb != null)
            {
                noteInDb.Title = note.Title;
                noteInDb.Description = note.Description;
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task SetTagsAsync(int noteId, List<int> tagIds)
        {
            var note = await _context.Notes.Include(n => n.Tags).FirstOrDefaultAsync(n => n.Id == noteId);
            if (note == null)
            {
                throw new NotFoundException(nameof(note), noteId);
            }
            var tags = await _context.Tags.Where(t => tagIds.Contains(t.Id)).ToListAsync();
            note.Tags = tags;

            await SaveAsync();
        }

        public bool TitleExist(string title)
        {
            return _context.Notes.Any(note => note.Title == title);
        }
    }
}
