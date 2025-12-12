using EventWave.Data;
using EventWave.DTOs;
using EventWave.Models;
using Microsoft.EntityFrameworkCore;

namespace EventWave.Repositories
{
    public class RegistrationRepository : IRegistrationRepository
    {
        private readonly ApplicationDBContext _context;

        public RegistrationRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<object> GetRegistrations()
        {
         var result = await _context.Registrations
        .Include(r => r.User)
        .Include(r => r.Event)
        .Select(r => new
        {
            Id = r.Id,
            UserId = r.UserId,
            UserName = r.User.FullName,
            EventId = r.EventId,
            EventTitle = r.Event.Title,
            RegisteredAt = r.RegisteredAt
        })
        .ToListAsync();

            return result;
        }

        public async Task<object?> GetRegistration(int id)
        {
            var result = await _context.Registrations
                .Include(r => r.User)
                .Include(r => r.Event)
                .Where(r => r.Id == id)
                .Select(r => new
                {
                    Id = r.Id,
                    UserId = r.UserId,

                    UserName = r.User.FullName,
                    UserEmail = r.User.Email,
                    UserPhone = r.User.PhoneNumber,

                    EventId = r.EventId,
                    EventTitle = r.Event.Title,

                    RegisteredAt = r.RegisteredAt
                })
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<Registration> AddRegistration(RegistrationDTO dto)
        {
            var registration = new Registration
            {
                UserId = dto.UserId,
                EventId = dto.EventId,
                RegisteredAt = DateTime.Now
            };

            _context.Registrations.Add(registration);
            await _context.SaveChangesAsync();
            //await _context.Entry(registration).Reference(r => r.User).LoadAsync();
            //await _context.Entry(registration).Reference(r => r.Event).LoadAsync();

            return registration;
        }

        public async Task<bool> DeleteRegistration(int id)
        {
            var reg = await _context.Registrations.FindAsync(id);
            if (reg == null)
                return false;

            _context.Registrations.Remove(reg);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

