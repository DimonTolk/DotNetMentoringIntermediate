namespace MainProcessingService
{
    public class FileContentProcessor
    {
        public void ProcessFile((string FileName, byte[] Content) message)
        {
            Console.WriteLine("Message delivered");
            using var stream = new MemoryStream(message.Content);
            using var fileStream = new FileStream(string.Concat(Constants.ProcessedFolderPath, message.FileName), FileMode.CreateNew, FileAccess.Write);
            stream.WriteTo(fileStream);
        }
    }
}
