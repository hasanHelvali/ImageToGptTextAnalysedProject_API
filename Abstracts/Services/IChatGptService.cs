using Microsoft.AspNetCore.Mvc;

namespace GIPAPI.Abstracts.Services
{
    public interface IChatGptService
    {
        Task<string> StartAnalyse(string prompt);
    }
}
