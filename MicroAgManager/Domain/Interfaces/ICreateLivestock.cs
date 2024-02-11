using Domain.Models;

namespace Domain.Interfaces
{
    public interface ICreateLivestock
    {
        Guid CreatedBy { get; set; }
        string CreationMode { get; set; }
        LivestockModel Livestock { get; set; }
    }
}