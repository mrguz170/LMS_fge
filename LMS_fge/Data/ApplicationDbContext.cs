using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LMS_fge.Models;
using LMS_fge.Models.User;
using System.Linq;

namespace LMS_fge.Data
{
    public class ApplicationDbContext : IdentityDbContext<
            ApplicationUser, ApplicationRole, Guid,
            ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin,
            ApplicationRoleClaim, ApplicationUserToken>
    {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options)
            {
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);

                //modelBuilder.HasDefaultSchema("SCVA");
                // To force a same collation database default user 
                //migrationBuilder.Sql("alter database character set latin1 collate latin1_swedish_ci");

                // Force all set restrict relationships
                var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                   .SelectMany(t => t.GetForeignKeys())
                   .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

                foreach (var fk in cascadeFKs)
                    fk.DeleteBehavior = DeleteBehavior.Restrict;

                modelBuilder.Entity<ApplicationUser>(b =>
                {
                    // Each User can have many UserClaims
                    b.HasMany(e => e.Claims)
                        .WithOne(e => e.User)
                        .HasForeignKey(uc => uc.UserId)
                        .IsRequired();

                    // Each User can have many UserLogins
                    b.HasMany(e => e.Logins)
                        .WithOne(e => e.User)
                        .HasForeignKey(ul => ul.UserId)
                        .IsRequired();

                    // Each User can have many UserTokens
                    b.HasMany(e => e.Tokens)
                        .WithOne(e => e.User)
                        .HasForeignKey(ut => ut.UserId)
                        .IsRequired();

                    // Each User can have many entries in the UserRole join table
                    b.HasMany(e => e.UserRoles)
                        .WithOne(e => e.User)
                        .HasForeignKey(ur => ur.UserId)
                        .IsRequired();
                });

                modelBuilder.Entity<ApplicationRole>(b =>
                {
                    // Each Role can have many entries in the UserRole join table
                    b.HasMany(e => e.UserRoles)
                        .WithOne(e => e.Role)
                        .HasForeignKey(ur => ur.RoleId)
                        .IsRequired();

                    // Each Role can have many associated RoleClaims
                    b.HasMany(e => e.RoleClaims)
                        .WithOne(e => e.Role)
                        .HasForeignKey(rc => rc.RoleId)
                        .IsRequired();
                });

                modelBuilder.Entity<ApplicationUser>(b =>
                {
                    b.ToTable("Identity_Users");
                });

                modelBuilder.Entity<ApplicationUserClaim>(b =>
                {
                    b.ToTable("Identity_UserClaims");
                });

                modelBuilder.Entity<ApplicationUserLogin>(b =>
                {
                    b.ToTable("Identity_UserLogins");
                });

                modelBuilder.Entity<ApplicationUserToken>(b =>
                {
                    b.ToTable("Identity_UserTokens");
                });

                modelBuilder.Entity<ApplicationRole>(b =>
                {
                    b.ToTable("Identity_Roles");
                });

                modelBuilder.Entity<ApplicationRoleClaim>(b =>
                {
                    b.ToTable("Identity_RoleClaims");
                });

                modelBuilder.Entity<ApplicationUserRole>(b =>
                {
                    b.ToTable("Identity_UserRoles");
                });
            }

            /*
            // For Items
            public DbSet<GeneralItem> GeneralItems { get; set; }
            public DbSet<Item> Items { get; set; }
            public DbSet<MeasurementUnit> MeasurementUnits { get; set; }
            */

            /*
            // For Purchases
            public DbSet<PurchaseStatus> PurchaseStatuses { get; set; }
            public DbSet<Purchase> Purchases { get; set; }
            public DbSet<PurchaseItem> PurchaseItems { get; set; }
            */

            /*
            // For Sales
            public DbSet<SaleStatus> SaleStatuses { get; set; }
            public DbSet<Sale> Sales { get; set; }
            public DbSet<SalePayment> SalePayments { get; set; }
            public DbSet<SaleItem> SaleItems { get; set; }
            public DbSet<SaleItemChangePrice> SaleItemChangePrices { get; set; }
            public DbSet<SaleItemDelivery> SaleItemDeliveries { get; set; }
            public DbSet<SaleCancel> SaleCancels { get; set; }
            public DbSet<SaleCancelDiscounts> SaleCancelDiscounts { get; set; }
            public DbSet<PayMethod> PayMethods { get; set; }
            */


            /*
            // For Clients
            public DbSet<Client> Clients { get; set; }
            public DbSet<ClientDiscount> ClientDiscount { get; set; }
            public DbSet<Egress> Egresses { get; set; }
            public DbSet<EgressCancel> EgressCancels { get; set; }
            */

            // For Printer
            //public DbSet<Printer> Printers { get; set; }
        }
    
}
