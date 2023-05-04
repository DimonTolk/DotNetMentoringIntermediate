using MainProcessingService.Models;

namespace MainProcessingService.Interfaces
{
    public interface IProcessingService
    {
        void Process((string FileName, byte[] Content) message);
    }
}
