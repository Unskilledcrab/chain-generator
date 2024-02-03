using OpenAI.Interfaces;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.ResponseModels.ImageResponseModel;

namespace ChainGenerator.Services
{
    public class ImageGenerator
    {
        private readonly IOpenAIService openAIService;  // OpenAI service for making API calls
        private readonly IServiceProvider serviceProvider;  // Service provider for creating new scopes
        private readonly string defaultModel;  // Default model to use for the image generator

        // Constructor
        public ImageGenerator(IOpenAIService openAIService, IServiceProvider serviceProvider)
        {
            this.openAIService = openAIService;
            this.serviceProvider = serviceProvider;
            this.defaultModel = OpenAI.ObjectModels.Models.Dall_e_3;  // Set the default model to DALL-E
        }

        // Method to create an image with the OpenAI API
        public async Task<ImageCreateResponse> CreateImageAsync(ImageCreateRequest request)
        {
            // Limit the prompt length to 4000 characters because of the API limit
            // https://platform.openai.com/docs/api-reference/images/create#images-create-prompt
            if (request.Prompt.Length > 4000)
            {
                request.Prompt = request.Prompt.Substring(0, 4000);
            }

            // If no model is specified, use the default model
            if (string.IsNullOrWhiteSpace(request.Model))
            {
                request.Model = defaultModel;
            }

            // Create the image
            var response = await openAIService.Image.CreateImage(request);

            // If the response is not successful, throw an exception
            if (!response.Successful)
            {
                throw new Exception(response.Error?.Message);
            }

            // Set the revised prompt for each result in the response
            foreach (var item in response.Results)
            {
                item.RevisedPrompt ??= request.Prompt;
            }

            return response;
        }

        // Method to create an image with a revised prompt
        public async Task<ImageCreateResponse> CreateImageWithPromptRevisionAsync(ImageCreateRequest request)
        {
            var revisedPrompt = string.Empty;

            // Create a new scope
            using (var scope = serviceProvider.CreateScope())
            {
                // Get the chat session service
                var chatSession = scope.ServiceProvider.GetRequiredService<ChatSession>();

                // Get the revised prompt from the chat session
                revisedPrompt = await chatSession.GetResponse($"Revise the following prompt for an image generator AI to include extra vivid visual details and remove anything that doesn't contribute to the visual details: ```{request.Prompt}```");
            }
            // Set the prompt to the revised prompt
            request.Prompt = revisedPrompt;

            // Create the image with the revised prompt
            return await CreateImageAsync(request);
        }
    }
}
