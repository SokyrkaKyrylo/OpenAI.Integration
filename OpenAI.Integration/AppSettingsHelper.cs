using Newtonsoft.Json;

namespace OpenAI.Integration;
public static class AppSettingsHelper
{
    const string FilePath = "appSettings.json";

    public static AppSettings GetAppSettings()
    {
        var directory = AppDomain.CurrentDomain.BaseDirectory;

        var indexOfBin = directory?.IndexOf("bin");

        if (indexOfBin is not null && indexOfBin != -1)
        {
            directory = directory?.Remove(indexOfBin.Value);
        }

        var appSettingsPath = Path.Combine(directory ?? string.Empty, FilePath);

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
}
