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
                    Id = r.Id,
                    UserName = r.User.FullName,
                    UserEmail = r.User.Email,

                    EventTitle = r.Event.Title,

                    TotalAmount = r.TotalAmount,
                    PaymentMethod = r.PaymentMethod.ToString(),

                    RegisteredAt = r.RegisteredAt,

                    Tickets = r.Tickets.Select(t => new
                    {
                        Id = t.Id,
                        Type = t.Type,
                        Price = t.Price,
                        TicketNumber = t.TicketNumber
                    }).ToList()
                })
                .ToListAsync();
        }


        public async Task<object?> GetRegistration(int id)
        {
            var result = await _context.Registrations
                .Include(r => r.User)
                .Include(r => r.Event)
                .Include(r => r.Tickets)
                .Where(r => r.Id == id)
                .Select(r => new
                {
                    Id = r.Id,

                    // USER
                    UserId = r.UserId,
                    UserName = r.User.FullName,
                    UserEmail = r.User.Email,
                    UserPhone = r.User.PhoneNumber,

                    // EVENT
                    EventId = r.EventId,
                    EventTitle = r.Event.Title,

                    // PAYMENT
                    TotalAmount = r.TotalAmount,
                    PaymentMethod = r.PaymentMethod.ToString(),

                    // REGISTRATION DATE
                    RegisteredAt = r.RegisteredAt,

                    // TICKETS
                    Tickets = r.Tickets.Select(t => new
                    {
                        t.Id,
                        t.Type,
                        t.Price,
                        t.TicketNumber
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            return result;
        }


        public async Task<object> AddRegistration(RegistrationDTO reg)
        {
            if (reg.TicketCount <= 0)
            {
                return new
                {
                    Status = "ERROR",
                    Message = "Nombre de tickets invalide"
                };
            }

            // 1️⃣ Charger l’événement
            var ev = await _context.Events
                .Include(e => e.TicketCapacities)
                .FirstOrDefaultAsync(e => e.Id == reg.EventId);

            if (ev == null)
                return new { Status = "ERROR", Message = "Event not found" };
            if (ev.Status == EventStatus.Cancelled)
            {
                return new
                {
                    Status = "ERROR",
                    Message = "Cet événement est annulé"
                };
            }


            // 2️⃣ Récupérer la capacité du type demandé
            var capacity = ev.TicketCapacities
                .FirstOrDefault(c => c.TicketType == reg.TicketType);

            if (capacity == null)
                return new { Status = "ERROR", Message = "Ticket type not available" };

            // 3️⃣ Vérifier disponibilité
            if (capacity.TicketsRemaining < reg.TicketCount)
            {
                _context.WaitLists.Add(new WaitList
                {
                    UserId = reg.UserId,
                    EventId = reg.EventId,
                    TicketType = reg.TicketType,
                    TicketCount = reg.TicketCount,
                    PaymentMethod=reg.PaymentMethod
                });

                await _context.SaveChangesAsync();

                return new
                {
                    Status = "WAITLIST",
                    Message = "Vous êtes ajouté à la liste d’attente"
                };
            }

            // 4️⃣ Créer la registration
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

            // 5️⃣ Réassigner les tickets libres existants
            foreach (var t in availableTickets)
            {
                t.RegistrationId = registration.Id;
                assignedTickets.Add(t);
            }
            


            int missingTickets = reg.TicketCount - availableTickets.Count;

           
            if (missingTickets > 0)
            {
                int startIndex = await _context.Tickets
                    .CountAsync(t => t.EventId == reg.EventId);

                var newTickets = new List<Ticket>();

                for (int i = 1; i <= missingTickets; i++)
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

            // 6️⃣ Mettre à jour la capacité
            capacity.TicketsRemaining -= reg.TicketCount;

            await _context.SaveChangesAsync();

            return new
            {
                Status = "CONFIRMED",
                Message = "Réservation confirmée",
                Id = registration.Id,
                Data = new
                {
                    RegistrationId = registration.Id,
                    EventId = ev.Id,
                    TicketType = reg.TicketType,
                    TicketCount = reg.TicketCount,
                    TotalAmount = total,
                    Tickets = assignedTickets.Select(t => new 
                        {   t.TicketNumber, 
                            t.Price, 
                            t.Type })
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
            var ticketType = reg.Tickets.First().Type;

            // 1️⃣ Récupérer les tickets libérés
            var freedTickets = reg.Tickets.ToList();

            // 2️⃣ Supprimer la réservation et ses tickets
           
            foreach (var t in freedTickets)
            {
                t.RegistrationId = null;
            }
            _context.Registrations.Remove(reg);
            await _context.SaveChangesAsync();

            // 3️⃣ Restaurer le stock
            var tc = await _context.TicketTypeCapacities
                .FirstAsync(t => t.EventId == eventId && t.TicketType == ticketType);
            tc.TicketsRemaining += freedTickets.Count;

            await _context.SaveChangesAsync();

            // 4️⃣ Traiter la waitlist FIFO
            var waitlist = await _context.WaitLists
                .Where(w => w.EventId == eventId && w.TicketType == ticketType)
                .OrderBy(w => w.CreatedAt)
                .ToListAsync();

            bool moved = false;

            foreach (var wait in waitlist)
            {
                if (freedTickets.Count < wait.TicketCount)
                    continue; 

                // 5️⃣ Créer la nouvelle réservation
                var newReg = new Registration
                {
                    UserId = wait.UserId,
                    EventId = wait.EventId,
                    RegisteredAt = DateTime.Now,
                    PaymentMethod = wait.PaymentMethod,
                    TotalAmount = wait.TicketCount * tc.Price
                };
                _context.Registrations.Add(newReg);
                await _context.SaveChangesAsync();

                // 6️⃣ Réutiliser les tickets libérés pour cette personne
                var ticketsToAssign = freedTickets.Take(wait.TicketCount).ToList();
                foreach (var t in ticketsToAssign)
                {
                    t.RegistrationId = newReg.Id;
                }

                _context.Tickets.UpdateRange(ticketsToAssign);

                // Retirer ces tickets de la liste des tickets libres
                freedTickets = freedTickets.Skip(wait.TicketCount).ToList();

                // 7️⃣ Si besoin, créer de nouveaux tickets si freedTickets insuffisants
                int remaining = wait.TicketCount - ticketsToAssign.Count;
                if (remaining > 0)
                {
                    var newTickets = new List<Ticket>();
                    for (int i = 0; i < remaining; i++)
                    {
                        newTickets.Add(new Ticket
                        {
                            EventId = eventId,
                            Type = ticketType,
                            Price = tc.Price,
                            RegistrationId = newReg.Id,
                            TicketNumber = $"E{eventId}T{Guid.NewGuid():N}".Substring(0, 12)
                        });
                    }
                    _context.Tickets.AddRange(newTickets);
                }

                // 8️⃣ Mettre à jour le stock
                tc.TicketsRemaining -= wait.TicketCount;

                // 9️⃣ Supprimer de la waitlist
                _context.WaitLists.Remove(wait);

                await _context.SaveChangesAsync();
                moved = true;
            }

            return moved
                ? "Réservation supprimée. Des utilisateurs ont été transférés depuis la liste d’attente."
                : "Réservation supprimée avec succès.";
        }










    }
}

