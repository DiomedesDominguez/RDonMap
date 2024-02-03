using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using src.Data.Entities;

namespace src.Data.Context
{
    public class AppDbContext: DbContext
    {
        public DbSet<mCountry> Countries { get; set; }
        public DbSet<mProvince> Provinces { get; set; }
        public AppDbContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("postgis");

            base.OnModelCreating(modelBuilder);
        }
    }
}