using EventWave.Data;
using EventWave.DTOs;
using EventWave.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventWave.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly ApplicationDBContext _context;

        public EventRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Event> AddAsync(Event evt)
        {
            _context.Events.Add(evt);
            await _context.SaveChangesAsync();
            return evt;
        }

        public async Task<Event> GetByIdAsync(int id)
        {
            return await _context.Events
                .Include(e => e.Speaker)
                .Include(e => e.Venue)
                .Include(e => e.TicketCapacities)        // Ajoutez ceci
                .Include(e => e.Registrations)           // Ajoutez ceci
                 .Include(e => e.Speakers)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<List<Event>> GetAllAsync()
        {
            return await _context.Events
                .Include(e => e.Speaker)
                .Include(e => e.Registrations)
                .Include(e => e.Speakers)
                .Include(e => e.Venue)
                .OrderByDescending(e => e.Id)
                .ToListAsync();
        }

        public async Task<Event?> UpdateAsync(Event evt)
        {
            var existingEvent = await _context.Events
                .Include(e => e.TicketCapacities)
                .FirstOrDefaultAsync(e => e.Id == evt.Id);

            if (existingEvent == null)
                return null;

            bool change = false;

            // 🔹 Mise à jour partielle de Event
            if (evt.Title != null && evt.Title != existingEvent.Title)
            {
                existingEvent.Title = evt.Title;
                change = true;
            }

            if (evt.Description != null && evt.Description != existingEvent.Description)
            {
                existingEvent.Description = evt.Description;
                change = true;
            }

            if (evt.Start != default && evt.Start != existingEvent.Start)
            {
                existingEvent.Start = evt.Start;
                change = true;
            }

            if (evt.End != default && evt.End != existingEvent.End)
            {
                existingEvent.End = evt.End;
                change = true;
            }

            if (evt.Category != null && evt.Category != existingEvent.Category)
            {
                existingEvent.Category = evt.Category;
                change = true;
            }

            if (evt.VenueId != 0 && evt.VenueId != existingEvent.VenueId)
            {
                existingEvent.VenueId = evt.VenueId;
                change = true;
            }


            if (evt.ImageUrl != null && evt.ImageUrl != existingEvent.ImageUrl)
            {
                existingEvent.ImageUrl = evt.ImageUrl;
                change = true;
            }

            if (evt.SpeakerId != 0 && evt.SpeakerId != existingEvent.SpeakerId)
            {
                existingEvent.SpeakerId = evt.SpeakerId;
                change = true;
            }

            // 🔹 Mise à jour des TicketCapacities
            if (evt.TicketCapacities != null)
            {
                foreach (var updatedTc in evt.TicketCapacities)
                {
                    var existingTc = existingEvent.TicketCapacities
                        .FirstOrDefault(tc => tc.TicketType == updatedTc.TicketType);

                    if (existingTc == null)
                        throw new Exception($"TicketType {updatedTc.TicketType} inexistant pour cet événement");

                    int sold = existingTc.Capacity - existingTc.TicketsRemaining;

                    // 🔒 Validation CAPACITY
                    if (updatedTc.Capacity != existingTc.Capacity)
                    {
                        if (updatedTc.Capacity < sold)
                        {
                            throw new Exception(
                                $"Capacité invalide pour {updatedTc.TicketType}. " +
                                $"Déjà vendus : {sold}, nouvelle capacité : {updatedTc.Capacity}"
                            );
                        }

                        existingTc.Capacity = updatedTc.Capacity;
                        existingTc.TicketsRemaining = updatedTc.Capacity - sold;
                        change = true;
                    }

                    // 💰 Mise à jour du prix
                    if (updatedTc.Price != existingTc.Price)
                    {
                        existingTc.Price = updatedTc.Price;
                        change = true;
                    }
                }
            }

            // 🔹 Aucun changement → pas de SaveChanges
            if (!change)
                return existingEvent; // ou throw new Exception("Aucun changement détecté")

            await _context.SaveChangesAsync();
            return existingEvent;
        }


        public async Task<bool> DeleteEventAsync(int eventId)
        {
            var existingEvent = await _context.Events
                .Include(e => e.Registrations)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (existingEvent == null)
                return false;

            // If tickets sold → refuse cancellation
            if (existingEvent.Registrations != null && existingEvent.Registrations.Count > 0)
                return false;

            existingEvent.Status = EventStatus.Cancelled;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<OrganizerStatsDTO?> GetOrganizerStatsAsync(string organizerId)
        {
            var organizer = await _context.Users
                .Where(u => u.Id == organizerId)
                .Select(u => new
                {
                    u.Id,
                    u.FullName,
                    u.Email
                })
                .FirstOrDefaultAsync();

            if (organizer == null)
                return null;

            var events = await _context.Events
                .Where(e => e.OrganizerId == organizerId)
                .Select(e => new EventSalesDTO
                {
                    EventId = e.Id,
                    Title = e.Title,

                    Capacity = e.TicketCapacities.Sum(tc => tc.Capacity),

                    TicketsSold = e.TicketCapacities.Sum(tc =>
                        tc.Capacity - tc.TicketsRemaining
                    ),

                    IsSoldOut = e.TicketCapacities.All(tc => tc.TicketsRemaining == 0),

                    SoldPercentage = e.TicketCapacities.Sum(tc => tc.Capacity) == 0
                        ? 0
                        : Math.Round(
                            (double)(
                                e.TicketCapacities.Sum(tc => tc.Capacity - tc.TicketsRemaining)
                            ) / e.TicketCapacities.Sum(tc => tc.Capacity) * 100,
                            2
                        )
                })
                .ToListAsync();

            return new OrganizerStatsDTO
            {
                OrganizerId = organizer.Id,
                FullName = organizer.FullName,
                Email = organizer.Email,
                TotalEvents = events.Count,
                SoldOutEvents = events.Count(e => e.IsSoldOut),
                Events = events
            };
        }



        public async Task<List<Event>> GlobalSearchAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return await _context.Events
                    .Include(e => e.Venue)
                    .Include(e => e.Speaker)
                    .ToListAsync();

            keyword = keyword.ToLower();

            return await _context.Events
                .Include(e => e.Venue)
                .Include(e => e.Speaker)
                .Where(e =>
                    e.Title.ToLower().Contains(keyword) ||
                    e.Description.ToLower().Contains(keyword) ||
                    e.Category.ToLower().Contains(keyword) ||
                    e.Venue.Name.ToLower().Contains(keyword) ||
                    e.Venue.City.ToLower().Contains(keyword) ||
                    e.Speaker.Name.ToLower().Contains(keyword)
                )
                .OrderBy(e => e.Start)
                .ToListAsync();
        }




        public async Task<List<Event>> AdvancedSearchAsync(
        int? speakerId,
        string? category,
        DateTime? start,
        string? venueName,
        string? city,
        string? description,
        string? title)
        {
            var query = _context.Events
                .Include(e => e.Speaker)
                .Include(e => e.Venue)
                .AsQueryable();

            if (speakerId.HasValue)
                query = query.Where(e => e.SpeakerId == speakerId.Value);

            if (!string.IsNullOrWhiteSpace(category))
                query = query.Where(e => e.Category.Contains(category));

            if (start.HasValue)
                query = query.Where(e => e.Start >= start.Value);

            if (!string.IsNullOrWhiteSpace(venueName))
                query = query.Where(e => e.Venue.Name.Contains(venueName));

            if (!string.IsNullOrWhiteSpace(city))
                query = query.Where(e => e.Venue.City.Contains(city));

            if (!string.IsNullOrWhiteSpace(description))
                query = query.Where(e => e.Description.Contains(description));

            if (!string.IsNullOrWhiteSpace(title))
                query = query.Where(e => e.Title.Contains(title));

            return await query
                .OrderBy(e => e.Start)
                .ToListAsync();
        }



    }
}
