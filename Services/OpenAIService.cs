using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenAI_API;
using OpenAI_API.Completions;
using OpenAI_API.Images;
using OpenAI_API.Models;

namespace CookGPT_MVVM.Services
{
    public class OpenAIService
    {
        private string _openApiKey = Environment.GetEnvironmentVariable("api_openapi_key");
        private OpenAIAPI? _openApiClient;

        public OpenAIService()
        {
            _openApiClient = new OpenAI_API.OpenAIAPI(_openApiKey);
        }

        private CompletionRequest GetGPTCompletionRequest(string message)
        {
            return new CompletionRequest(message, Model.DavinciText, 200, 0.5, presencePenalty: 0.1, frequencyPenalty: 0.1);
        }

        private ImageGenerationRequest GetDALLEImageRequest(string message)
        {
            var request = new ImageGenerationRequest();
            request.Prompt = message;
            request.NumOfImages = 1;
            request.ResponseFormat = ImageResponseFormat.Url;
            request.Size = ImageSize._256;

            return request;

        }


        public async Task<string> SendGPTRequest(string question)
        {
            if (_openApiClient is null) { throw new NullReferenceException(); }
            string answer = string.Empty;
            CompletionRequest completion = GetGPTCompletionRequest(question);
            try
            {
                var result = await _openApiClient.Completions.CreateCompletionAsync(completion);
                if (result != null)
                {
                    foreach (var item in result.Completions)
                    {
                        answer = item.Text;
                    }
                    return answer;
                }
                else
                {
                    return "Failed to generate query!";
                }
            }
            catch
            {
                return string.Empty;
            }


        }

        public async Task<string> SendDALLERequest(string prompt)
        {
            if (_openApiClient is null) { throw new NullReferenceException(); }
            var request = GetDALLEImageRequest(prompt);
            try
            {
                var result = await _openApiClient.ImageGenerations.CreateImageAsync(request);
                if (result != null)
                {
                    return result.ToString();
                }
                else
                {
                    return "Failed to generate query!";
                }
            }
            catch
            {
                return "pack://application:,,,/Assets/loadingicon.png";
            }


        }
    }
}
