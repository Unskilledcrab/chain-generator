using Microsoft.Extensions.Logging.Abstractions;
using OpenAI.Interfaces;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.ResponseModels;
using System.Text;

namespace ChainGenerator.Services
{
    public class ChatSession
    {
        private readonly IOpenAIService openAIService;  // OpenAI service for making API calls
        private readonly string defaultModel;  // Default model to use for the chat session
        private List<ChatMessage> messages = new List<ChatMessage>();  // List of messages in the chat session

        // Constructor
        public ChatSession(IOpenAIService openAIService)
        {
            this.openAIService = openAIService;
            this.defaultModel = OpenAI.ObjectModels.Models.Gpt_3_5_Turbo_1106;
        }

        // Method to set the system prompt
        public void SetSystemPrompt(string prompt)
        {
            AddMessage(ChatMessage.FromSystem(prompt));
        }

        // Method to get the messages in the chat session
        public List<ChatMessage> GetMessages()
        {
            return messages;
        }

        // Method to get the response stream from the OpenAI API
        public async IAsyncEnumerable<string> GetResponseStream(string userPrompt, string? model = null)
        {
            await foreach (var response in InternalGetReponseStream(userPrompt, model))
            {
                yield return response;
            }
        }

        // Internal method to get the response stream from the OpenAI API
        private async IAsyncEnumerable<string> InternalGetReponseStream(string userPrompt, string? model = null, int recursiveCount = 0)
        {
            // We don't want to get stuck in an infinite loop 
            // and charge our users for it.
            if (recursiveCount > 5)
            {
                throw new Exception("Recursive count exceeded.");
            }

            // Process the request
            ChatCompletionCreateRequest request = ConvertPromptToChatCompletionRequest(userPrompt, model);

            // Create the completion as a stream
            // https://platform.openai.com/docs/api-reference/chat/create
            var response = openAIService.ChatCompletion.CreateCompletionAsStream(request);

            var completeResponse = new StringBuilder();
            await foreach (var completion in response)
            {
                if (completion.Successful == false)
                {
                    yield return completion.Error?.Message ?? throw new Exception("Response error was null.");
                    break;
                }
                var responseChoice = completion.Choices.FirstOrDefault() ?? throw new Exception("Response choice was null.");

                switch (responseChoice.FinishReason)
                {
                    case "stop":
                        // We're done here. exit the function.
                        yield break;
                    case "length":
                        // recursively call this function to stream results if we hit the max length in this response
                        await foreach (var recursiveResponse in InternalGetReponseStream(completeResponse.ToString(), model, recursiveCount++))
                        {
                            yield return recursiveResponse;
                        }
                        break;
                    default:
                        break;
                }

                var responseText = responseChoice.Message?.Content;
                if (responseText == null)
                {
                    throw new Exception("Response message was null.");
                }

                completeResponse.Append(responseText);
                yield return responseText;
            }

            // Add the complete response to the messages
            AddMessage(ChatMessage.FromAssistant(completeResponse.ToString()));
        }

        // Method to get the response from the OpenAI API
        public async Task<string> GetResponse(string userPrompt, string? model = null)
        {
            // Process the request
            ChatCompletionCreateRequest request = ConvertPromptToChatCompletionRequest(userPrompt, model);

            // Create the completion
            // https://platform.openai.com/docs/api-reference/chat/create
            var response = await openAIService.ChatCompletion.CreateCompletion(request);

            if (response.Successful == false)
            {
                throw new Exception(response.Error?.Message);
            }

            var responseMessage = response.Choices.First().Message;
            var responseText = responseMessage.Content;

            if (responseText == null)
            {
                throw new Exception("Response message was null.");
            }

            // Add the response message to the messages
            AddMessage(responseMessage);

            return responseText;
        }

        // Method to convert the user prompt to a chat completion request
        private ChatCompletionCreateRequest ConvertPromptToChatCompletionRequest(string userPrompt, string? model)
        {
            // Add the user prompt to the messages
            AddMessage(ChatMessage.FromUser(userPrompt));
            var request = new ChatCompletionCreateRequest
            {
                Messages = messages,
                Model = defaultModel
            };

            if (model != null)
            {
                request.Model = model;
            }

            return request;
        }

        // Method to add a message to the local cached messages
        private void AddMessage(ChatMessage message)
        {
            messages.Add(message);
        }
    }
}
