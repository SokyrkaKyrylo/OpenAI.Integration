using OpenAI;
using OpenAI.Integration;
using OpenAI.Managers;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;

var appSettings = AppSettingsHelper.GetAppSettings();

try
{
    var openAiService = new OpenAIService(new OpenAiOptions
    {
        ApiKey = appSettings.ChatGptApiKey
    });

    while (true)
    {
        Console.WriteLine("Enter a prompt to start the conversation (or 'exit' to quit):");

        var prompt = Console.ReadLine();

        if (prompt?.ToLower() != "exit")
        {
            var response = await openAiService.Completions.CreateCompletion(new CompletionCreateRequest
            {
                Model = Models.Gpt_3_5_Turbo,
                MaxTokens = 50,
                Prompt = prompt
            });

            string resultMessage;

            if (response.Successful)
            {
                var message = response?.Choices[0];

                resultMessage = $"ChatGPT: {message?.Text}\n";
            }
            else
            {
                resultMessage = $"Error: {response?.Error?.Message}\n";
            }

            Console.WriteLine(resultMessage);
        }
    }
}
catch (Exception e)
{
    Console.WriteLine("Something went wrong -> " + e.Message);
}
