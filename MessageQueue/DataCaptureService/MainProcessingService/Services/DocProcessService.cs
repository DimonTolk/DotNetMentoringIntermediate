using MainProcessingService.Interfaces;

namespace MainProcessingService.Services
{
    public class DocProcessService : IProcessingService
    {
        private FileContentProcessor _contentProcessor;

        public DocProcessService()
        {
            _contentProcessor = new FileContentProcessor();
        }

        public void Process((string FileName, byte[] Content) message)
        {
            // TODO: Add docx-specific logic

            _contentProcessor.ProcessFile(message);
        }
    }
}
