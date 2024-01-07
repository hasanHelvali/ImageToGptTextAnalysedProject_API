using GIPAPI.Abstracts.Services;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels;
using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels.ResponseModels;

namespace GIPAPI.Services
{
    public class ChatGptService : IChatGptService
    {
        readonly IOpenAIService _openAIService;

        public ChatGptService(IOpenAIService openAIService)
        {
            _openAIService = openAIService;
        }

        public async Task<string> StartAnalyse(string prompt)
        {
            prompt = "Senden sadece vercegim yazılarla ilgili bir duygu analizi yapmanı istiyorum. Yazılar: "+prompt + " seklindedir. " +
                "Bana bu yazıların duygu analizini yapar mısın?";
            CompletionCreateResponse response = await _openAIService.Completions.CreateCompletion(new CompletionCreateRequest()
            {
                Prompt = prompt,
                MaxTokens = 500,
            }, Models.TextDavinciV3);
            string analyse = response.Choices[0].Text;
            return analyse;
        }
    }
}
