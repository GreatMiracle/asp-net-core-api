﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Core.Entities;

namespace WebApplication1.Infrastructure.Data
{
    //public class AuthWalksDbContext : IdentityDbContext<ApplicationUser>
    public class AuthWalksDbContext : IdentityDbContext
    {
        //public AuthWalksDbContext(DbContextOptions<AuthWalksDbContext> options) : base(options)
        public AuthWalksDbContext(DbContextOptions options) : base(options)
        {
        }

        //// Định nghĩa các DbSet cho các thực thể
        //public DbSet<Difficulty> Difficulties { get; set; }
        //public DbSet<Region> Regions { get; set; }
        //public DbSet<Walk> Walks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed data for Difficulties
            // Easy, Medium, Hard
            //var difficulties = new List<Difficulty>()
            //{
            //    new Difficulty()
            //    {
            //        Id = Guid.Parse("54466f17-02af-48e7-8ed3-5a4a8bfacf6f"),
            //        Name = "Easy"
            //    },
            //    new Difficulty()
            //    {
            //        Id = Guid.Parse("ea294873-7a8c-4c0f-bfa7-a2eb492cbf8c"),
            //        Name = "Medium"
            //    },
            //    new Difficulty()
            //    {
            //        Id = Guid.Parse("f808ddcd-b5e5-4d80-b732-1ca523e48434"),
            //        Name = "Hard"
            //    }
            //};

            //// Seed difficulties to the database
            //modelBuilder.Entity<Difficulty>().HasData(difficulties);



            //// Seed data for Regions
            //var regions = new List<Region>
            //{
            //    new Region
            //    {
            //        Id = Guid.Parse("f7248fc3-2585-4efb-8d1d-1c555f4087f6"),
            //        Name = "Auckland",
            //        Code = "AKL",
            //        ImageUrl = "https://images.pexels.com/photos/5169056/pexels-photo-5169056.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
            //    },
            //    new Region
            //    {
            //        Id = Guid.Parse("6884f7d7-ad1f-4101-8df3-7a6fa7387d81"),
            //        Name = "Northland",
            //        Code = "NTL",
            //        ImageUrl = null
            //    },
            //    new Region
            //    {
            //        Id = Guid.Parse("14ceba71-4b51-4777-9b17-46602cf66153"),
            //        Name = "Bay Of Plenty",
            //        Code = "BOP",
            //        ImageUrl = null
            //    },
            //    new Region
            //    {
            //        Id = Guid.Parse("cfa06ed2-bf65-4b65-93ed-c9d286ddb0de"),
            //        Name = "Wellington",
            //        Code = "WGN",
            //        ImageUrl = "https://images.pexels.com/photos/4350631/pexels-photo-4350631.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
            //    },
            //    new Region
            //    {
            //        Id = Guid.Parse("906cb139-415a-4bbb-a174-1a1faf9fb1f6"),
            //        Name = "Nelson",
            //        Code = "NSN",
            //        ImageUrl = "https://images.pexels.com/photos/13918194/pexels-photo-13918194.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
            //    },
            //    new Region
            //    {
            //        Id = Guid.Parse("f077a22e-4248-4bf6-b564-c7cf4e250263"),
            //        Name = "Southland",
            //        Code = "STL",
            //        ImageUrl = null
            //    },
            //};

            //modelBuilder.Entity<Region>().HasData(regions);


            // Tạo ID (GUID) cho các vai trò
            var readerRoleId = Guid.NewGuid().ToString();
            var writerRoleId = Guid.NewGuid().ToString();

            // Tạo danh sách các vai trò
            var roles = new List<IdentityRole>
        {
            new IdentityRole
            {
                Id = readerRoleId,
                Name = "Reader",
                NormalizedName = "READER",
                ConcurrencyStamp = readerRoleId
            },
            new IdentityRole
            {
                Id = writerRoleId,
                Name = "Writer",
                NormalizedName = "WRITER",
                ConcurrencyStamp = writerRoleId
            }
        };

            // Seed các vai trò vào cơ sở dữ liệu
            modelBuilder.Entity<IdentityRole>().HasData(roles);

        }
    }
}
