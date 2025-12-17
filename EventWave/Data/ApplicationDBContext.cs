using EventWave.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EventWave.Data
{
    public class ApplicationDBContext : IdentityDbContext<User>
    {
        public ApplicationDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Registration> Registrations { get; set; }
       // public DbSet<WaitlistEntry> Waitlist { get; set; }
        public DbSet<Speaker> Speakers { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<WaitList> WaitLists { get; set; }
        public DbSet<TicketTypeCapacity> TicketTypeCapacities { get; set; }
        public DbSet<Venue> Venues { get; set; }

        public DbSet<Profile> Profiles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Ticket>()
                .Property(t => t.Type)
                .HasConversion<string>();

            builder.Entity<Ticket>()
                .HasOne(t => t.Registration)
                .WithMany(r => r.Tickets)
                .HasForeignKey(t => t.RegistrationId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<TicketTypeCapacity>()
                .Property(t => t.TicketType)
                .HasConversion<string>();

            builder.Entity<Registration>()
                .Property(r => r.PaymentMethod)
                .HasConversion<string>();

            builder.Entity<Event>()
                .Property(e => e.Status)
                .HasConversion<string>();

            builder.Entity<WaitList>()
                .Property(w => w.TicketType)
                .HasConversion<string>();

            builder.Entity<WaitList>()
                .Property(w => w.PaymentMethod)
                .HasConversion<string>();

            builder.Entity<Event>()
                .HasOne(e => e.Venue)
                .WithMany(v => v.Events)
                .HasForeignKey(e => e.VenueId);

            builder.Entity<Profile>()
                .HasOne(p => p.User)
                .WithOne()
                .HasForeignKey<Profile>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }



    }


}

