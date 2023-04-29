﻿using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;

namespace BackEnd.BusinessLogic.Livestock
{
    public class LivestockStatusQueries : BaseQuery
    {
        public long? LivestockTypeId { get; set; }
        public string? Status { get; set; }

        public LivestockStatusModel? NewLivestockStatus { get => (LivestockStatusModel?)NewModel; set => NewModel = value; }
        public IQueryable<Domain.Entity.LivestockStatus> GetQuery(IMicroAgManagementDbContext context)
        {
            var query = PopulateBaseQuery(context.LivestockStatuses.AsQueryable());
            if (query is null) throw new ArgumentNullException(nameof(query));

            if (LivestockTypeId.HasValue) query = query.Where(l => l.LivestockType.Id == LivestockTypeId);
            if(!string.IsNullOrEmpty(Status)) query = query.Where(l => l.Status.Contains(Status));

            query = query.OrderByDescending(_ => _.ModifiedOn);
            return query;
        }

    }
}
