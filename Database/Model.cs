using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace SasnoBot.Database
{
    public class SasnoBotDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=sasnobot.db");
        }
    }

    public class User
    {
        public int Id { get; set; }
        public ulong? DiscordUserId { get; set; }
        public bool? ConfigurationPriveleges { get; set; }
    }
}
