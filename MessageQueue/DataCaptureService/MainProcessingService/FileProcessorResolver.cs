using MainProcessingService.Enums;
using MainProcessingService.Helpers;
using MainProcessingService.Interfaces;
using MainProcessingService.Services;

namespace MainProcessingService
{
    public static class FileProcessorResolver
    {
        public static IProcessingService ResolveProcessor(string name)
        {
            var extension = EnumHelper.GetValueFromDescription<ExtensionsEnum>(Path.GetExtension(name).ToLower());
            switch (extension)
            {
                case ExtensionsEnum.Txt:
                    return new TxtProcessService();
                case ExtensionsEnum.Docx:
                    return new DocProcessService();
                case ExtensionsEnum.Pdf:
                    return new PdfProcessService();
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
