using LibGit2Sharp;
using System;
using System.IO;
using Newtonsoft.Json.Linq;

string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".config", "nvim");

// Load repositories from config file
var configFilePath = "configrepositories.json";
if (!File.Exists(configFilePath))
{
    Console.WriteLine("Config file not found.");
    return;
}

var configContent = File.ReadAllText(configFilePath);
var config = JObject.Parse(configContent);
var repositories = config["repositories"].ToObject<string[]>();

// Display the list of repositories and get user selection
Console.WriteLine("Select a Neovim config to clone:");
for (int i = 0; i < repositories.Length; i++)
{
    Console.WriteLine($"{i + 1}. {repositories[i]}");
}

Console.Write("Enter the number of the repository: ");
if (!int.TryParse(Console.ReadLine(), out int selection) || selection < 1 || selection > repositories.Length)
{
    Console.WriteLine("Invalid selection.");
    return;
}

string selectedRepo = repositories[selection - 1];

try
{
    if (Directory.Exists(folderPath))
    {
        Directory.Delete(folderPath, true);
        Console.WriteLine($"Folder '{folderPath}' deleted successfully.");
    }
    else
    {
        Console.WriteLine($"Folder '{folderPath}' does not exist.");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred while deleting the folder: {ex.Message}");
}

Repository.Clone("https://github.com/" + selectedRepo + ".git", folderPath);