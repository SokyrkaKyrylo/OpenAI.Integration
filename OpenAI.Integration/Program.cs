using OpenAI.Managers;
using OpenAI;
using OpenAI.ObjectModels.RequestModels;
using Newtonsoft.Json;
using OpenAI.ObjectModels;

var appSettings = GetAppSettings();

var openAiService = new OpenAIService(new OpenAiOptions
{
    ApiKey = appSettings.ChatGPTApiKey
});

var prompt = string.Empty;

while (true)
{
    Console.WriteLine("Enter a prompt to start the conversation (or 'exit' to quit):");

    prompt = Console.ReadLine();

    if (prompt.ToLower() == "exit")
    {
        break;
    }

    var response = await openAiService.Completions.CreateCompletion(new CompletionCreateRequest
    {
        Model = Models.Gpt_3_5_Turbo,
        MaxTokens = 50,
        Prompt = prompt
    });

    if (response.Successful)
    {
        var message = response?.Choices[0];

        Console.WriteLine($"ChatGPT: {message?.Text}\n");
    }
    else
    {
        Console.WriteLine($"Error: {response?.Error?.Message}\n");
    }
}

static AppSettings GetAppSettings()
{
    var directory = AppDomain.CurrentDomain.BaseDirectory;

    var indexOfBin = directory?.IndexOf("bin");

    if (indexOfBin is not null && indexOfBin != -1)
    {
        directory = directory?.Remove(indexOfBin.Value);
    }

    var filePath = "appSettings.json";

    var appSettingsPath = Path.Combine(directory, filePath);

    if (!File.Exists(appSettingsPath))
    {
        throw new FileNotFoundException("appSettings.json file not found.");
    }

    var json = File.ReadAllText(appSettingsPath);

    var appSettings = JsonConvert.DeserializeObject<AppSettings>(json);

    if (appSettings is null)
    {
        throw new ArgumentNullException(nameof(appSettings), "The appSettings.json data is missing");
    }

    return appSettings;
}
public class AppSettings
{
    public string ChatGPTApiKey { get; set; }
}