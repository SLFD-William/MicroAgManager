﻿using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.BusinessLogic.FarmLocation
{
    public class CreateFarmLocation:BaseCommand, ICreateCommand
    {

        public Guid CreatedBy { get => ModifiedBy; set => ModifiedBy = value; }
        [Required]public FarmLocationModel Farm { get; set; }
        public class Handler : BaseCommandHandler<CreateFarmLocation>
        {
            public Handler(IMicroAgManagementDbContext context, IMediator mediator) : base(context, mediator)
            {
            }

            public override async Task<long> Handle(CreateFarmLocation request, CancellationToken cancellationToken)
            {

                var farm = new Domain.Entity.FarmLocation(request.ModifiedBy, request.TenantId);
                farm = request.Farm.MapToEntity(farm);

                farm.ModifiedOn = farm.Created = DateTime.Now;
                farm.ModifiedBy = farm.CreatedBy = request.ModifiedBy;
                farm.TenantId = request.TenantId;

                _context.Farms.Add(farm);
                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                return farm.Id;
            }

        }

    }
}
