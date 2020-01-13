using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BankaBackend.Models
{
    public partial class M4ABankContext : DbContext
    {
        public M4ABankContext()
        {
        }

        public M4ABankContext(DbContextOptions<M4ABankContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<Credit> Credit { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Hgs> Hgs { get; set; }
        public virtual DbSet<Transactions> Transactions { get; set; }
        public virtual DbSet<Account_Type> Account_Type { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=89.252.180.81\MSSQLSERVER2012;Initial Catalog=m4abankdb;Persist Security Info=False;User ID=tesisat2_m4abank;Password=Fb58190758;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False;Connection Timeout=30;Pooling=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.AccountNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.AddAccName).HasMaxLength(150);

                entity.Property(e => e.AddAccNumber).HasMaxLength(50);

                entity.Property(e => e.Balance).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.EditDate).HasColumnType("datetime");

                entity.Property(e => e.SaveDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Credit>(entity =>
            {
                entity.Property(e => e.CreditId).HasColumnName("CreditID");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.CreditBalance).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.EditDate).HasColumnType("datetime");

                entity.Property(e => e.SaveDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.CustomerTckn)
                    .IsRequired()
                    .HasColumnName("CustomerTCKN")
                    .HasMaxLength(50);

                entity.Property(e => e.EditDate).HasColumnType("datetime");

                entity.Property(e => e.NameSurname).IsRequired();

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(12);

                entity.Property(e => e.SaveDate).HasColumnType("datetime");
            });

          

            modelBuilder.Entity<Transactions>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 4)");

                entity.Property(e => e.EditDate).HasColumnType("datetime");

                entity.Property(e => e.SaveDate).HasColumnType("datetime");

                entity.Property(e => e.AType)
                    .IsRequired()
                    .HasMaxLength(150);
            });
        }
    }
}
