﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VillaProject.Domain.Entities;

namespace VillaProject.Infrastructure.Data
{
    
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> 
    {
       
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        } 
        public DbSet<Villa> Villas { get; set; } 
        public DbSet<VillaNumber> VillaNumbers { get; set; }
        public DbSet<Amenity> Amenities { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<VillaImages> VillaImages { get; set; }
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<Category> Categories { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // использование Fluent API
            base.OnModelCreating(modelBuilder);
            

            modelBuilder.Entity<Villa>().HasData(
                  new Villa
                  {
                      Id = 1,
                      Name = "Best Villa",
                      Description = "Only for rich person",
                      Occupancy = 4,
                      Price = 200,
                      Sqft = 550,
                  },
                 new Villa
                 {
                      Id = 2,
                      Name = "Premium House",
                      Description = "Premium villa.",
                      Occupancy = 4,
                      Price = 300,
                      Sqft = 1550,
                 },
                 new Villa
                 {
                      Id = 3,
                      Name = "Luxury Pool",
                      Description = "Empty description.",
                      Occupancy = 4,
                      Price = 1400,
                      Sqft = 1750,
                 });
            modelBuilder.Entity<VillaNumber>().HasData(
                new VillaNumber
                {
                    Villa_Number = 101,
                    VillaId = 1,
                },
                new VillaNumber
                {
                    Villa_Number = 102,
                    VillaId = 1,
                },
                new VillaNumber
                {
                    Villa_Number = 103,
                    VillaId = 1,
                },
                new VillaNumber
                {
                    Villa_Number = 104,
                    VillaId = 1,
                },
                new VillaNumber
                {
                    Villa_Number = 201,
                    VillaId = 2,
                },
                new VillaNumber
                {
                    Villa_Number = 202,
                    VillaId = 2,
                },
                new VillaNumber
                {
                    Villa_Number = 203,
                    VillaId = 2,
                },
                new VillaNumber
                {
                    Villa_Number = 301,
                    VillaId = 3,
                },
                new VillaNumber
                {
                    Villa_Number = 302,
                    VillaId = 3,
                }
                );
            modelBuilder.Entity<Amenity>().HasData(
          new Amenity
          {
              Id = 1,
              VillaId = 1,
              //Name = "Private Pool"
          }, new Amenity
          {
              Id = 2,
              VillaId = 1,
             // Name = "Microwave"
          }, new Amenity
          {
              Id = 3,
              VillaId = 1,
              //Name = "Private Balcony"
          }, new Amenity
          {
              Id = 4,
              VillaId = 1,
              //Name = "1 king bed and 1 sofa bed"
          },

          new Amenity
          {
              Id = 5,
              VillaId = 2,
              //Name = "Private Plunge Pool"
          }, new Amenity
          {
              Id = 6,
              VillaId = 2,
              //Name = "Microwave and Mini Refrigerator"
          }, new Amenity
          {
              Id = 7,
              VillaId = 2,
              //Name = "Private Balcony"
          }, new Amenity
          {
              Id = 8,
              VillaId = 2,
              //Name = "king bed or 2 double beds"
          },

          new Amenity
          {
              Id = 9,
              VillaId = 3,
              //Name = "Private Pool"
          }, new Amenity
          {
              Id = 10,
              VillaId = 3,
              //Name = "Jacuzzi"
          }, new Amenity
          {
              Id = 11,
              VillaId = 3,
              //Name = "Private Balcony"
          });
        }
    }
}
