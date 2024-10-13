﻿namespace WebApplication1.DTOs.Request
{
    public class CreateWalkRequest : RequestBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double LenghthInKm { get; set; }
        public string? WalkImageUrl { get; set; }
        public Guid DifficultyId { get; set; }
        public Guid RegionId { get; set; }
    }
}
