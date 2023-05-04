using MainProcessingService.Interfaces;

namespace MainProcessingService.Services
{
    public class PdfProcessService : IProcessingService
    {
        private FileContentProcessor _contentProcessor;

        public PdfProcessService()
        {
            _contentProcessor = new FileContentProcessor();
        }

        public void Process((string FileName, byte[] Content) message)
        {
            // TODO: Add pdf-specific logic

            _contentProcessor.ProcessFile(message);
        }
    }
}
