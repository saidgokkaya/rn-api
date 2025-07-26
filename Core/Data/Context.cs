using Core.Domain.Ruhsat;
using Core.Domain.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Numerics;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    public class Context : DbContext
    {
        public Context()
        {
        }

        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                var connectionString = configuration.GetConnectionString("DefaultConnection");

                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("Default Connection bulunmamaktadır.");
                }

                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        public DbSet<Organization> Organization { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<Ruhsat> Ruhsat { get; set; }
        public DbSet<RuhsatSinifi> RuhsatSinifi { get; set; }
        public DbSet<RuhsatTuru> RuhsatTuru { get; set; }
        public DbSet<FaaliyetKonusu> FaaliyetKonusu { get; set; }
        public DbSet<Depo> Depo { get; set; }
        public DbSet<DepoBilgi> DepoBilgi { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRole)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRole)
                .HasForeignKey(ur => ur.RoleId);

            modelBuilder.Entity<Ruhsat>()
                .HasOne(r => r.Organization)
                .WithMany(o => o.Ruhsat)
                .HasForeignKey(r => r.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ruhsat>()
                .HasOne(r => r.RuhsatSinifi)
                .WithMany(rs => rs.Ruhsat)
                .HasForeignKey(r => r.RuhsatSinifiId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DepoBilgi>()
                .HasOne(db => db.Organization)
                .WithMany(o => o.DepoBilgi)
                .HasForeignKey(db => db.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DepoBilgi>()
                .HasOne(db => db.Ruhsat)
                .WithMany(r => r.DepoBilgi)
                .HasForeignKey(db => db.RuhsatId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Depo>()
                .HasOne(d => d.RuhsatSinifi)
                .WithMany(rs => rs.Depo)
                .HasForeignKey(d => d.RuhsatSinifiId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
