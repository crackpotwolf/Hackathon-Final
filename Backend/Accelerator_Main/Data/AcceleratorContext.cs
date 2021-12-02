using Data.Models.DB.Account;
using Data.Models.DB.Files;
using Data.Models.DB.Project;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    /// <summary>
    /// Context Db
    /// </summary>
    public class AcceleratorContext : DbContext
    {
        /// <inheritdoc />
        public AcceleratorContext(DbContextOptions options) : base(options)
        {
        }

        /// <inheritdoc />
        public AcceleratorContext()
        {
        }

        #region Таблицы в БД

        #region Account
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }
        #endregion

        #region Project

        public DbSet<Project> Projects { get; set; }

        public DbSet<Activities> Activities { get; set; }

        public DbSet<Budget> Budgets { get; set; }

        public DbSet<Effect> Effects { get; set; }

        public DbSet<Material> Materials { get; set; }

        public DbSet<Meeting> Meetings { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Stage> Stages { get; set; }

        public DbSet<Status> Statuses { get; set; }

        public DbSet<Team> Teams { get; set; }

        #endregion

        #region Files

        public virtual DbSet<DocumentInfo> DocumentInfos { get; set; }
        public virtual DbSet<FileVersion> FileVersions { get; set; }

        #endregion

        #endregion

        /// <summary>
        /// Связи
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Связи: один к одному
            #endregion

            #region Связи: многие ко многим

            #region Пользователи и роли

            modelBuilder.Entity<UserRoles>()
                .HasKey(p => new { p.UserGuid, p.RoleGuid });

            modelBuilder.Entity<UserRoles>()
                .HasOne(p => p.Role)
                .WithMany(p => p.UserRoles)
                .HasForeignKey(p => p.RoleGuid);

            modelBuilder.Entity<UserRoles>()
                .HasOne(p => p.User)
                .WithMany(p => p.UserRoles)
                .HasForeignKey(p => p.UserGuid);

            #endregion

            #region Проекты

            #endregion

            #endregion
        }
    }
}