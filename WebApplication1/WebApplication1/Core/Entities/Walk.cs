﻿namespace WebApplication1.Core.Entities
{
    public class Walk
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double LenghthInKm { get; set; }
        public string? WalkImageUrl {  get; set; }
        public Guid DifficultyId { get; set; }
        public Guid RegionId { get; set; }

        public Difficulty Difficulty{ get; set; }

        public Region Region { get; set; }

    }
}
