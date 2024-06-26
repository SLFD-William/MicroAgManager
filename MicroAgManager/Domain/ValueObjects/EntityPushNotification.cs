﻿using Domain.Interfaces;

namespace Domain.ValueObjects
{
    public class EntityPushNotification : IModifiedEntityPushNotification
    {
        public EntityPushNotification(Guid tenantId, string modelJson, string modelType, DateTime serverModifiedTime )
        {
            ModelJson = modelJson ?? throw new ArgumentNullException(nameof(modelJson));
            ModelType = modelType ?? throw new ArgumentNullException(nameof(modelType));
            TenantId = tenantId;
            ServerModifiedTime = serverModifiedTime;
        }

        public string ModelJson { get; set; }
        public string ModelType { get; set; }
        public Guid TenantId { get; set; }
        public DateTime ServerModifiedTime { get; set; }
    }
}
