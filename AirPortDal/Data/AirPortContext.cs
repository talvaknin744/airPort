using AirPortCommon.DBModels;
using AirPortCommon.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AirPortDal.Data
{
    public class AirPortContext: DbContext
    {
        public AirPortContext(DbContextOptions<AirPortContext> options) : base(options)
        {

        }

        public virtual DbSet<Airplane> Airplanes { get; set; }
        public virtual DbSet<Station> Stations { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<HistoryInfo> Histroy { get; set; }
        public virtual DbSet<PlannedArrivals> PlannedArrivals { get; set; }
        public virtual DbSet<PlannedDepartures> PlannedDepartures { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            optionsBuilder.UseLazyLoadingProxies();
            optionsBuilder.EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Station>().HasData(
                new Station("Ben Gurion", 6, new List<Course>()) { Id = 1 },
                new Station("NY", 8, new List<Course>()) { Id = 2 },
                new Station("Prague", 9, new List<Course>()) { Id = 3 },
                new Station("Berlin", 6, new List<Course>()) { Id = 4 },
                new Station("Shenzhen", 7, new List<Course>()) { Id = 5 },
                new Station("JFK", 9, new List<Course>()) { Id = 6 },
                new Station("Kansai", 8, new List<Course>()) { Id = 7 });

            base.OnModelCreating(modelBuilder);
        }
    }
}
