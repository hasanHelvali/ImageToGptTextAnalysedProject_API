using IronOcr;
using GIPAPI.Abstracts.Services;
using GIPAPI.DTOs;

namespace GIPAPI.Services
{
    public class ImageToTextService : IImageToTextService
    {
        readonly IConfiguration _configuration;
        public async Task<string> StartAnalyse()
        {

            string result = "";
            var ocr = new IronTesseract();
            ocr.Language = OcrLanguage.Turkish;
            var ocrInput = new OcrInput(ImagePaths.ImagesPaths);

            var ocrResult = ocr.Read(ocrInput);
            result = ocrResult.Text;

            return result;
        }
    }
}
