using Microsoft.Extensions.AI;

namespace Catalog.Services;

public class ProductAIService(IChatClient chatClient)
{
    public async Task<string> SupportAsync(string query)
    {
        var systemPrompt = """
                           You are a useful assistant.
                           You always reply with a short and funny message.
                           If you do not know an answer, you say 'I don't know that.'
                           You only answer questions related to outdoor camping products.
                           For any other type of questions, explain to the user that you only answer outdoor camping questions.
                           At the end, Offer on of our products: Hiking Poles-$24, Outdoor Rain Jacket-$12, outdoor Tent-$99, Camping Stove-$49, Sleeping Bag-$39.
                           Dot not store memory of the chat conversation.
                           """;
        var chatHistory = new List<ChatMessage>
        {
            new(ChatRole.System, systemPrompt),
            new(ChatRole.User, query)
        };

        var resultPrompt = await chatClient.GetResponseAsync(chatHistory);
        return resultPrompt.Messages.First().ToString();

    }
}