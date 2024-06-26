﻿using Domain.Abstracts;
using Domain.Entity;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{

    public class TenantModel:BaseModel
    {

        [Required] public Guid GuidId { get; set; }
        [Required] public string Name { get; set; }
        [Required] public Guid TenantUserAdminId { get; set; }
        public string? WeatherServiceQueryURL { get; set; }
        public static TenantModel Create(Tenant tenant)
        {
            if (tenant == null) throw new ArgumentNullException(nameof(tenant));
            return new TenantModel
            {
                Id = tenant.Id,
                GuidId = tenant.GuidId,
                Name = tenant.Name,
                TenantUserAdminId = tenant.TenantUserAdminId,
                Deleted = false,
                WeatherServiceQueryURL = tenant.WeatherServiceQueryURL,
                EntityModifiedOn = tenant.ModifiedOn,
                ModifiedBy = tenant.ModifiedBy
            };
        }

        public override BaseModel Map(BaseModel tenant)
        {
            if (tenant is not TenantModel) return null;
            ((TenantModel)tenant).Name = Name;
            ((TenantModel)tenant).WeatherServiceQueryURL = WeatherServiceQueryURL;
            ((TenantModel)tenant).GuidId= GuidId;
            ((TenantModel)tenant).TenantUserAdminId = TenantUserAdminId;
            ((TenantModel)tenant).Deleted = false;
            ((TenantModel)tenant).EntityModifiedOn = EntityModifiedOn;
            ((TenantModel)tenant).ModifiedBy = ModifiedBy;
            return tenant;
        }

        public override BaseEntity Map(BaseEntity model)
        {
            throw new NotImplementedException();
        }
    }
}
