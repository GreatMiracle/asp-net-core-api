using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Collections.Generic;
using WebApplication1.Core.Entities;

namespace WebApplication1.Infrastructure.Data
{
    public class WalksDbContext : DbContext
    {
        public WalksDbContext(DbContextOptions<WalksDbContext> options) : base(options)
        {
           
        }

        // Định nghĩa các DbSet cho các thực thể
        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }
        public DbSet<Image> Images{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed data for Difficulties
            // Easy, Medium, Hard
            var difficulties = new List<Difficulty>()
            {
                new Difficulty()
                {
                    Id = Guid.Parse("54466f17-02af-48e7-8ed3-5a4a8bfacf6f"),
                    Name = "Easy"
                },
                new Difficulty()
                {
                    Id = Guid.Parse("ea294873-7a8c-4c0f-bfa7-a2eb492cbf8c"),
                    Name = "Medium"
                },
                new Difficulty()
                {
                    Id = Guid.Parse("f808ddcd-b5e5-4d80-b732-1ca523e48434"),
                    Name = "Hard"
                }
            };

            // Seed difficulties to the database
            modelBuilder.Entity<Difficulty>().HasData(difficulties);



            // Seed data for Regions
            var regions = new List<Region>
            {
                new Region
                {
                    Id = Guid.Parse("f7248fc3-2585-4efb-8d1d-1c555f4087f6"),
                    Name = "Auckland",
                    Code = "AKL",
                    ImageUrl = "https://images.pexels.com/photos/5169056/pexels-photo-5169056.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                },
                new Region
                {
                    Id = Guid.Parse("6884f7d7-ad1f-4101-8df3-7a6fa7387d81"),
                    Name = "Northland",
                    Code = "NTL",
                    ImageUrl = null
                },
                new Region
                {
                    Id = Guid.Parse("14ceba71-4b51-4777-9b17-46602cf66153"),
                    Name = "Bay Of Plenty",
                    Code = "BOP",
                    ImageUrl = null
                },
                new Region
                {
                    Id = Guid.Parse("cfa06ed2-bf65-4b65-93ed-c9d286ddb0de"),
                    Name = "Wellington",
                    Code = "WGN",
                    ImageUrl = "https://images.pexels.com/photos/4350631/pexels-photo-4350631.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                },
                new Region
                {
                    Id = Guid.Parse("906cb139-415a-4bbb-a174-1a1faf9fb1f6"),
                    Name = "Nelson",
                    Code = "NSN",
                    ImageUrl = "https://images.pexels.com/photos/13918194/pexels-photo-13918194.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                },
                new Region
                {
                    Id = Guid.Parse("f077a22e-4248-4bf6-b564-c7cf4e250263"),
                    Name = "Southland",
                    Code = "STL",
                    ImageUrl = null
                },
            };

            modelBuilder.Entity<Region>().HasData(regions);
        }
    }
}


/*INSERT INTO "Walks" ("Id", "Name", "Description", "LenghthInKm", "WalkImageUrl", "DifficultyId", "RegionId")
VALUES
('327aa9f7-26f7-4ddb-8047-97464374bb63', 'Mount Victoria Loop', 'This scenic walk takes you around the top of Mount Victoria, offering stunning views of Wellington and its harbor.', 3.5, 'https://images.pexels.com/photos/4350631/pexels-photo-4350631.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1', '54466F17-02AF-48E7-8ED3-5A4A8BFACF6F','CFA06ED2-BF65-4B65-93ED-C9D286DDB0DE');

INSERT INTO "Walks" ("Id", "Name", "Description", "LenghthInKm", "WalkImageUrl", "DifficultyId", "RegionId")
VALUES
('1cc5f2bc-ff4b-47c0-a475-1add56c6497b', 'Makara Beach Walkway', 'This walk takes you along the wild and rugged coastline of Makara Beach, with breathtaking views of the Tasman Sea.', 8.2, 'https://images.pexels.com/photos/4350631/pexels-photo-4350631.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1', 'EA294873-7A8C-4C0F-BFA7-A2EB492CBF8C','CFA06ED2-BF65-4B65-93ED-C9D286DDB0DE');

INSERT INTO "Walks" ("Id", "Name", "Description", "LenghthInKm", "WalkImageUrl", "DifficultyId", "RegionId")
VALUES
('09601132-f92d-457c-b47e-da90e117b33c', 'Botanic Garden Walk', 'Explore the beautiful Botanic Garden of Wellington on this leisurely walk, with a wide variety of plants and flowers to admire.', 2, 'https://images.pexels.com/photos/4350631/pexels-photo-4350631.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1', '54466F17-02AF-48E7-8ED3-5A4A8BFACF6F','CFA06ED2-BF65-4B65-93ED-C9D286DDB0DE');

INSERT INTO "Walks" ("Id", "Name", "Description", "LenghthInKm", "WalkImageUrl", "DifficultyId", "RegionId")
VALUES
('30d654c7-89ac-4704-8333-5065b740150b', 'Mount Eden Summit Walk', 'This walk takes you to the summit of Mount Eden, the highest natural point in Auckland, with panoramic views of the city.', 2, 'https://images.pexels.com/photos/5342974/pexels-photo-5342974.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1', '54466F17-02AF-48E7-8ED3-5A4A8BFACF6F','F7248FC3-2585-4EFB-8D1D-1C555F4087F6');

INSERT INTO "Walks" ("Id", "Name", "Description", "LenghthInKm", "WalkImageUrl", "DifficultyId", "RegionId")
VALUES
('f7578324-f025-4c86-83a9-37a7f3d8fe81', 'Cornwall Park Walk', 'Explore the beautiful Cornwall Park on this leisurely walk, with a wide variety of trees, gardens, and animals to admire.', 3, 'https://images.pexels.com/photos/5342974/pexels-photo-5342974.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1', '54466F17-02AF-48E7-8ED3-5A4A8BFACF6F','F7248FC3-2585-4EFB-8D1D-1C555F4087F6');

INSERT INTO "Walks" ("Id", "Name", "Description", "LenghthInKm", "WalkImageUrl", "DifficultyId", "RegionId")
VALUES
('bdf28703-6d0e-4822-ad8b-e2923f4e95a2', 'Takapuna to Milford Coastal Walk', 'This coastal walk takes you along the beautiful beaches of Takapuna and Milford, with stunning views of Rangitoto Island.', 5, 'https://images.pexels.com/photos/5342974/pexels-photo-5342974.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1', 'EA294873-7A8C-4C0F-BFA7-A2EB492CBF8C','F7248FC3-2585-4EFB-8D1D-1C555F4087F6');

INSERT INTO "Walks" ("Id", "Name", "Description", "LenghthInKm", "WalkImageUrl", "DifficultyId", "RegionId")
VALUES
('43132402-3d5e-467a-8cde-351c5c7c5dde', 'Centre of New Zealand Walkway', 'This walk takes you to the geographical centre of New Zealand, with stunning views of Nelson and its surroundings.', 1.0 , 'https://images.pexels.com/photos/808466/pexels-photo-808466.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1', 'EA294873-7A8C-4C0F-BFA7-A2EB492CBF8C','906CB139-415A-4BBB-A174-1A1FAF9FB1F6');
*/