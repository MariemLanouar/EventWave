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

        public async Task<object?> GetRegistrations()
        {
            return await _context.Registrations
                .Include(r => r.User)
                .Include(r => r.Event)
                .Include(r => r.Tickets)
                .Select(r => new
                {
                    r.Id,
                    UserName = r.User.FullName,
                    UserEmail = r.User.Email,
                    EventTitle = r.Event.Title,
                    r.TotalAmount,
                    PaymentMethod = r.PaymentMethod.ToString(),
                    r.RegisteredAt,
                    Tickets = r.Tickets.Select(t => new
                    {
                        t.Id,
                        t.Type,
                        t.Price,
                        t.TicketNumber
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<object?> GetRegistrationsByUser(string userId)
        {
            return await _context.Registrations
                .Include(r => r.User)
                .Include(r => r.Event)
                .Include(r => r.Tickets)
                .Where(r => r.UserId == userId)
                .Select(r => new
                {
                    r.Id,
                    r.EventId,
                    EventTitle = r.Event.Title,
                    EventStart = r.Event.Start,
                    EventEnd = r.Event.End,
                    EventImageUrl = r.Event.ImageUrl,
                    EventVenue = r.Event.Venue.Name + ", " + r.Event.Venue.City,
                    r.TotalAmount,
                    PaymentMethod = r.PaymentMethod.ToString(),
                    r.RegisteredAt,
                    Tickets = r.Tickets.Select(t => new
                    {
                        t.Id,
                        t.Type,
                        t.Price,
                        t.TicketNumber
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<object?> GetRegistration(int id)
        {
            return await _context.Registrations
                .Include(r => r.User)
                .Include(r => r.Event)
                .Include(r => r.Tickets)
                .Where(r => r.Id == id)
                .Select(r => new
                {
                    r.Id,
                    r.UserId,
                    UserName = r.User.FullName,
                    UserEmail = r.User.Email,
                    UserPhone = r.User.PhoneNumber,
                    r.EventId,
                    EventTitle = r.Event.Title,
                    r.TotalAmount,
                    PaymentMethod = r.PaymentMethod.ToString(),
                    r.RegisteredAt,
                    Tickets = r.Tickets.Select(t => new
                    {
                        t.Id,
                        t.Type,
                        t.Price,
                        t.TicketNumber
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<object> AddRegistration(RegistrationDTO reg)
        {
            if (reg.TicketCount <= 0)
                return new { Status = "ERROR", Message = "Nombre de tickets invalide" };

            var ev = await _context.Events
                .Include(e => e.Venue)
                .Include(e => e.TicketCapacities)
                .FirstOrDefaultAsync(e => e.Id == reg.EventId);

            if (ev == null)
                return new { Status = "ERROR", Message = "Event not found" };

            if (ev.Status == EventStatus.Cancelled)
                return new { Status = "ERROR", Message = "Cet événement est annulé" };

            if (ev.Venue == null)
                return new { Status = "ERROR", Message = "Event venue not assigned" };

            var capacity = ev.TicketCapacities
                .FirstOrDefault(c => c.TicketType == reg.TicketType);

            if (capacity == null)
                return new { Status = "ERROR", Message = "Ticket type not available" };

            int totalSold = ev.TicketCapacities.Sum(tc => tc.Capacity - tc.TicketsRemaining);
            if (totalSold + reg.TicketCount > ev.Venue.Capacity)
                return new { Status = "ERROR", Message = "Venue capacity exceeded" };

            if (capacity.TicketsRemaining < reg.TicketCount)
            {
                _context.WaitLists.Add(new WaitList
                {
                    UserId = reg.UserId,
                    EventId = reg.EventId,
                    TicketType = reg.TicketType,
                    TicketCount = reg.TicketCount,
                    PaymentMethod = reg.PaymentMethod
                });

                await _context.SaveChangesAsync();

                return new
                {
                    Status = "WAITLIST",
                    Message = "Vous êtes ajouté à la liste d’attente"
                };
            }

            decimal total = capacity.Price * reg.TicketCount;

            var registration = new Registration
            {
                UserId = reg.UserId,
                EventId = reg.EventId,
                PaymentMethod = reg.PaymentMethod,
                TotalAmount = total,
                RegisteredAt = DateTime.Now
            };

            _context.Registrations.Add(registration);
            await _context.SaveChangesAsync();

            var availableTickets = await _context.Tickets
                .Where(t =>
                    t.EventId == reg.EventId &&
                    t.Type == reg.TicketType &&
                    t.RegistrationId == null)
                .Take(reg.TicketCount)
                .ToListAsync();

            var assignedTickets = new List<Ticket>();

            foreach (var t in availableTickets)
            {
                t.RegistrationId = registration.Id;
                assignedTickets.Add(t);
            }

            int missing = reg.TicketCount - availableTickets.Count;

            if (missing > 0)
            {
                int startIndex = await _context.Tickets
                    .CountAsync(t => t.EventId == reg.EventId);

                for (int i = 1; i <= missing; i++)
                {
                    var newTicket = new Ticket
                    {
                        EventId = reg.EventId,
                        RegistrationId = registration.Id,
                        Type = reg.TicketType,
                        Price = capacity.Price,
                        TicketNumber = $"E{reg.EventId}T{startIndex + i:D3}"
                    };
                    _context.Tickets.Add(newTicket);
                    assignedTickets.Add(newTicket);
                }
            }

            capacity.TicketsRemaining -= reg.TicketCount;
            await _context.SaveChangesAsync();

            return new
            {
                Status = "CONFIRMED",
                Message = "Réservation confirmée",
                Id = registration.Id,
                Data = new
                {
                    registration.Id,
                    EventId = ev.Id,
                    reg.TicketType,
                    reg.TicketCount,
                    TotalAmount = total,
                    Tickets = assignedTickets.Select(t => new
                    {
                        t.TicketNumber,
                        t.Price,
                        t.Type
                    })
                }
            };
        }

        public async Task<string> DeleteRegistration(int id)
        {
            var reg = await _context.Registrations
                .Include(r => r.Tickets)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reg == null)
                return "Cette réservation n’a pas été trouvée.";

            int eventId = reg.EventId;
            var groupedTickets = reg.Tickets.GroupBy(t => t.Type).ToList();
            var freedTickets = reg.Tickets.ToList();

            foreach (var t in freedTickets)
                t.RegistrationId = null;

            _context.Registrations.Remove(reg);
            await _context.SaveChangesAsync();

            foreach (var group in groupedTickets)
            {
                var tc = await _context.TicketTypeCapacities
                    .FirstAsync(t => t.EventId == eventId && t.TicketType == group.Key);
                tc.TicketsRemaining += group.Count();
            }

            await _context.SaveChangesAsync();

            var waitlist = await _context.WaitLists
                .Where(w => w.EventId == eventId)
                .OrderBy(w => w.CreatedAt)
                .ToListAsync();

            foreach (var wait in waitlist)
            {
                var ev = await _context.Events
                    .Include(e => e.Venue)
                    .Include(e => e.TicketCapacities)
                    .FirstAsync(e => e.Id == eventId);

                int sold = ev.TicketCapacities.Sum(tc => tc.Capacity - tc.TicketsRemaining);
                if (sold + wait.TicketCount > ev.Venue.Capacity)
                    break;

                var tc = ev.TicketCapacities.First(t => t.TicketType == wait.TicketType);
                if (tc.TicketsRemaining < wait.TicketCount)
                    continue;

                var newReg = new Registration
                {
                    UserId = wait.UserId,
                    EventId = eventId,
                    PaymentMethod = wait.PaymentMethod,
                    TotalAmount = tc.Price * wait.TicketCount,
                    RegisteredAt = DateTime.Now
                };

                _context.Registrations.Add(newReg);
                await _context.SaveChangesAsync();

                var reused = freedTickets.Take(wait.TicketCount).ToList();
                foreach (var t in reused)
                    t.RegistrationId = newReg.Id;

                freedTickets = freedTickets.Skip(wait.TicketCount).ToList();
                tc.TicketsRemaining -= wait.TicketCount;
                _context.WaitLists.Remove(wait);

                await _context.SaveChangesAsync();
            }

            return "Réservation supprimée avec succès.";
        }
    }
}
