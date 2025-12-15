using EventWave.Data;
using EventWave.Models;
using Microsoft.EntityFrameworkCore;

namespace EventWave.Repositories
{
    public class WaitListRepository : IWaitListRepository
    {
        private readonly ApplicationDBContext _context;

        public WaitListRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        // 🔹 Consulter toute la waitlist
        public async Task<List<WaitList>> GetAll()
        {
            return await _context.WaitLists
                .OrderBy(w => w.CreatedAt) // FIFO
                .ToListAsync();
        }

        // 🔹 Consulter la waitlist d’un événement spécifique
        public async Task<List<WaitList>> GetByEvent(int eventId)
        {
            return await _context.WaitLists
                .Where(w => w.EventId == eventId)
                .OrderBy(w => w.CreatedAt) // FIFO
                .ToListAsync();
        }
    }
}
