using MainProcessingService.Models;

namespace MainProcessingService.Interfaces
{
    public interface IProcessingService
    {
        void Process(DCSMessage message);
    }
}
