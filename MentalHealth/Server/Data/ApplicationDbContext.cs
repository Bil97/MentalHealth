using MentalHealth.Server.Mpesa;
using MentalHealth.Shared.Models;
using MentalHealth.Shared.Models.UserAccount;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MentalHealth.Server.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Seed database
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    Id = "a129b23e-a4c9-4bf6-9643-12e6ef5f5987",
                    ConcurrencyStamp = "dd95e6b4-0dab-4e5b-a9bb-c58b7b1d1855"
                });
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Name = "HealthOfficer",
                    NormalizedName = "HEALTHOFFICER",
                    Id = "2c690374-4121-446f-ab4a-1309c06ce441",
                    ConcurrencyStamp = "f90a04c6-8ffa-45bc-a4eb-281874e40e3b"
                });
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "USER",
                    Id = "b3ef9d40-56ad-462a-9081-86cdb5162a61",
                    ConcurrencyStamp = "2c690374-4121-446f-ab4a-1309c06ce441"
                });
            builder.Entity<Profession>().HasData(
                new Profession
                {
                    Id = "4afe0d86-c744-4b83-8e47-2dfea49b0569",
                    Name = "Therapist"
                });
        }

        public DbSet<Profession> Professions { get; set; }
        public DbSet<UserProfession> UserProfessions { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<SessionRecord> SessionRecords { get; set; }
        public DbSet<PatientHealthRecord> PatientHealthRecords { get; set; }
        public DbSet<MpesaTransaction> MpesaTransactions { get; set; }
        public DbSet<MpesaAccount> MpesaAccounts { get; set; }
    }
}