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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Ticket>()
                .Property(t => t.Type)
                .HasConversion<string>();

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Registration)
                .WithMany(r => r.Tickets)
                .HasForeignKey(t => t.RegistrationId)
                .OnDelete(DeleteBehavior.NoAction); // ⬅️ IMPORTANT

            modelBuilder.Entity<TicketTypeCapacity>()
                .Property(t => t.TicketType)
                .HasConversion<string>(); // 👈 clé du problème
           

            modelBuilder.Entity<Registration>()
                .Property(r => r.PaymentMethod)
                .HasConversion<string>();

            modelBuilder.Entity<Event>()
                .Property(e => e.Status)
                .HasConversion<string>();

            modelBuilder.Entity<WaitList>()
               .Property(w => w.TicketType)
               .HasConversion<string>();

            modelBuilder.Entity<WaitList>()
                .Property(w => w.PaymentMethod)
                .HasConversion<string>();

        }


        public DbSet<Profile> Profiles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Profile>()
                .HasOne(p => p.User)
                .WithOne()
                .HasForeignKey<Profile>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }


    }


}

