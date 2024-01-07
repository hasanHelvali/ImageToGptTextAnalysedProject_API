using GIPAPI.Abstracts.Services;
using GIPAPI.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Text.Json;

namespace GIPAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageAnalysisController : ControllerBase
    {
        readonly IWebHostEnvironment _webHostEnvironment;
        readonly IImageToTextService _imageToTextService;
        readonly IChatGptService _chatGptService;

        public ImageAnalysisController(IWebHostEnvironment webHostEnvironment, IImageToTextService imageToTextService, IChatGptService chatGptService)
        {
            _webHostEnvironment = webHostEnvironment;
            _imageToTextService = imageToTextService;
            _chatGptService = chatGptService;
        }
        [HttpGet]
        public async Task<IActionResult> Analyse()
        {
            string analiz = await _imageToTextService.StartAnalyse();
            string analiz2 = await _chatGptService.StartAnalyse(analiz);
            return Ok(JsonSerializer.Serialize(analiz2));
        }
        [HttpPost]
        public async Task<IActionResult> Upload()
        {
            await FileClear();
            string uplaodPath = Path.Combine(_webHostEnvironment.WebRootPath, "images");

            //Gonderilen File ler birer form data oldugundan dolayı parametrede yakalanamaz. Bu yuzdne request in icindeki form un icindeki files lerden yakalıyoruz.

            Random rnd = new();
            IFormFileCollection? files = Request.Form.Files;
            foreach (IFormFile file in files)
            {
                string dosyaYolu = $"{rnd.Next()}{Path.GetExtension(file.FileName)}";
                string fullPath = Path.Combine(uplaodPath, dosyaYolu);

                ImagePaths.ImagesPaths.Add(fullPath);
                using FileStream fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None, 256 * 256, useAsync: false);
                await file.CopyToAsync(fileStream);
                await fileStream.FlushAsync();//stream i bosaltıyoruz.
            }

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult>FileClear()
        {
            var webRootPath = _webHostEnvironment.WebRootPath;
            var directory = new DirectoryInfo(webRootPath);
            foreach (var file in directory.EnumerateFiles("*", SearchOption.AllDirectories))
            {
                file.Delete();
            }
            return Ok();
        }
    }
}
