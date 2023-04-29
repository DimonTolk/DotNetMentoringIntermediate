using MainProcessingService.Interfaces;
using MainProcessingService.Models;

namespace MainProcessingService
{
    public class DocProcessService : IProcessingService
    {
        public void Process(DCSMessage message)
        {
            Console.WriteLine("Message delivered");
            using var stream = new MemoryStream(message.Content);
            using var fileStream = new FileStream(string.Concat(Constants.ProcessedFolderPath, message.FileName), FileMode.CreateNew, FileAccess.Write);
            stream.WriteTo(fileStream);
        }
    }
}
