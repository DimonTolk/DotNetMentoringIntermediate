namespace DataCaptureService.Models
{
    public class DCSMessage
    {
        public string FileName { get; set; }

        public int Position { get; set; }

        public int Size { get; set; }


        public byte[] Content { get; set; }
    }
}
