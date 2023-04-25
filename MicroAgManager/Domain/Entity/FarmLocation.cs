﻿using Domain.Abstracts;

namespace Domain.Entity
{
    public class FarmLocation : BaseEntity
    {
        public FarmLocation(Guid createdBy, Guid tenantId) : base(createdBy, tenantId)
        {
        }
        public string? Name { get; set; }
        public string? Longitude { get; set; }
        public string? Latitude { get; set; }
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
        public string? Country { get; set; }
        public ICollection<LandPlot> Plots { get; set; }
    }
}