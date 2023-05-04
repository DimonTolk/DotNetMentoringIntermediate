using MainProcessingService.Interfaces;

namespace MainProcessingService.Services
{
    public class TxtProcessService : IProcessingService
    {
        private FileContentProcessor _contentProcessor;

        public TxtProcessService()
        {
            _contentProcessor = new FileContentProcessor();
        }

        public void Process((string FileName, byte[] Content) message)
        {
            // TODO: Add txt-specific logic

            _contentProcessor.ProcessFile(message);
        }
    }
}
