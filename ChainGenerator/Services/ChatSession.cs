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
        private readonly IOpenAIService openAIService;
        private List<ChatMessage> messages = new List<ChatMessage>();

        public ChatSession(IOpenAIService openAIService)
        {
            this.openAIService = openAIService;
            openAIService.SetDefaultModelId(Models.Gpt_3_5_Turbo_1106);
        }

        public void SetSystemPrompt(string prompt)
        {
            AddMessage(ChatMessage.FromSystem(prompt));
        }

        public List<ChatMessage> GetMessages()
        {
            return messages;
        }

        public async IAsyncEnumerable<string> GetResponseStream(string userPrompt, string? model = null)
        {
            await foreach (var response in InternalGetReponseStream(userPrompt, model))
            {
                yield return response;
            }
        }

        private async IAsyncEnumerable<string> InternalGetReponseStream(string userPrompt, string? model = null, int recursiveCount = 0)
        {
            // We don't want to get stuck in an infinite loop 
            // and charge our users for it.
            if (recursiveCount > 5)
            {
				throw new Exception("Recursive count exceeded.");
			}

			var request = new ChatCompletionCreateRequest
			{
				Messages = messages
			};

			if (model != null)
			{
				request.Model = model;
			}

			var response = openAIService.ChatCompletion.CreateCompletionAsStream(request);
			AddMessage(ChatMessage.FromUser(userPrompt));

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
						// recursively call this function to stream results
						await foreach (var recursiveResponse in InternalGetReponseStream(userPrompt, model, recursiveCount++))
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

			AddMessage(ChatMessage.FromAssistant(completeResponse.ToString()));
		}

        public async Task<string> GetResponse(string userPrompt, string? model = null)
        {
            var request = new ChatCompletionCreateRequest
            {
                Messages = messages
            };

            if (model != null)
            {
                request.Model = model;
            }

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

            AddMessage(ChatMessage.FromUser(userPrompt));
            AddMessage(responseMessage);

            return responseText;
        }

        private void AddMessage(ChatMessage message)
        {
            messages.Add(message);
        }
    }
}
