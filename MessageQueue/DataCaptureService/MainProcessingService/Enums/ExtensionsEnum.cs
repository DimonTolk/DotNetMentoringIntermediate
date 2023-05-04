using System.ComponentModel;

namespace MainProcessingService.Enums
{
    public enum ExtensionsEnum
    {
        [Description(".txt")]
        Txt,
        [Description(".docx")]
        Docx,
        [Description(".pdf")]
        Pdf,
    }
}
