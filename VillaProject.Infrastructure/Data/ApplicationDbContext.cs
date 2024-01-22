using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VillaProject.Domain.Entities;

namespace VillaProject.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        //через опции мы вшиваем использование SQL сервера.
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        //так мы сообщаем EF что класс Villa это будет таблицей в БД с названием Villas
        //создаем таблицу (пока что только в С#, не в БД) командой Add-migration (выбрав нужный проект)
        //далее уже доабвляем эту таблицу с C# в саму базу данных в SQL - Update Database
        //в папке Migration файл Snapshot помогает понять нужно ли пересоздавать все с нуля или только обновить что-то и т д. Он помогает EF
        //НИКОГДА НЕ МЕНЯТЬ/УДАЛЯТЬ ПАПКУ MIGRATIONS, ЕСЛИ НУЖНО ЧТО-ТО ИЗМЕНИТЬ, ПРОСТО МЕНЯЕМ И ДАЛЕЕ add-migration => update-database
        public DbSet<Villa> Villas { get; set; } //тут хранятся все данные 

        public DbSet<VillaNumber> VillaNumbers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Villa>().HasData(
                  new Villa
                  {
                      Id = 1,
                      Name = "Royal Villa",
                      Description = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                      ImageUrl = "https://placehold.co/600x400",
                      Occupancy = 4,
                      Price = 200,
                      Sqft = 550,
                  },
                 new Villa
                 {
                      Id = 2,
                      Name = "Premium Pool Villa",
                      Description = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                      ImageUrl = "https://placehold.co/600x401",
                      Occupancy = 4,
                      Price = 300,
                      Sqft = 550,
                 },
                 new Villa
                 {
                      Id = 3,
                      Name = "Luxury Pool Villa",
                      Description = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                      ImageUrl = "https://placehold.co/600x402",
                      Occupancy = 4,
                      Price = 400,
                      Sqft = 750,
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
        }
    }
}
