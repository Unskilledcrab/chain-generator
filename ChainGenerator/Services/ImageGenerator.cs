using OpenAI.Interfaces;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.ResponseModels.ImageResponseModel;

namespace ChainGenerator.Services
{
    public class ImageGenerator
    {
        private readonly IOpenAIService openAIService;
        private readonly IServiceProvider serviceProvider;
        private readonly string defaultModel;

        public ImageGenerator(IOpenAIService openAIService, IServiceProvider serviceProvider)
        {
            this.openAIService = openAIService;
            this.serviceProvider = serviceProvider;
            this.defaultModel = Models.Dall_e_3;
        }

        public async Task<ImageCreateResponse> CreateImageAsync(ImageCreateRequest request)
        {
            if (request.Prompt.Length > 1000)
            {
                request.Prompt = request.Prompt.Substring(0, 1000);
            }

            if (string.IsNullOrWhiteSpace(request.Model))
            {
                request.Model = defaultModel;
            }

            var response = await openAIService.Image.CreateImage(request);

            if (!response.Successful)
            {
                throw new Exception(response.Error?.Message);
            }

            foreach (var item in response.Results)
            {
                item.RevisedPrompt ??= request.Prompt;
            }
            return response;
        }

        public async Task<ImageCreateResponse> CreateImageWithPromptRevisionAsync(ImageCreateRequest request)
        {
            var revisedPrompt = string.Empty;
            using (var scope = serviceProvider.CreateScope())
            {
                var chatSession = scope.ServiceProvider.GetRequiredService<ChatSession>();
                revisedPrompt = await chatSession.GetResponse($"Revise the following prompt for an image generator AI to include extra vivid visual details and remove anything that doesn't contribute to the visual details: ```{request.Prompt}```");
            }
            request.Prompt = revisedPrompt;
            return await CreateImageAsync(request);
        }
    }
}
