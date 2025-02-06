using System.ComponentModel;
using System.IO.Hashing;
using System.Text;
using Humanizer;
using MeilidexSharp.Outputs;
using Serilog;
using Spectre.Console.Cli;

namespace MeilidexSharp.Commands;

class IndexCreateCommand : Command<IndexCreateCommand.Settings> {
    internal class Settings : MeilisearchBaseSettings {
        [CommandArgument(0, "<BASE-DIR>")]
        public required string BaseDir { get; init; }

        [CommandOption("-l|--base-url <BASE-URL>")]
        public required string? BaseUrl { get; set; }

        [CommandOption("-i|--index <INDEX>")]
        public required string? Index { get; init; }

        [CommandOption("-b|--batch <BATCH>")]
        [DefaultValue(10000)]
        public required int Batch { get; init; }
    }

    private static string HashString(string value) {
        var hasher = new XxHash64();
        hasher.Append(Encoding.UTF8.GetBytes(value));
        return Convert.ToHexStringLower(hasher.GetCurrentHash());
    }

    private static async Task Walk(string dir, string baseDir, IFileEntryOutput output, string? baseUrl) {
        foreach (string filePath in Directory.GetFiles(dir)) {
            try {
                var fileInfo = new FileInfo(filePath);
                string fileSize = fileInfo.Length.Bytes().Humanize();
                string fileName = Path.GetFileName(filePath);
                string? url = null;
                string id = HashString(filePath);
                if (baseUrl != null) {
                    url = baseUrl + filePath[baseDir.Length..];
                }
                var lastAccess = File.GetLastAccessTime(filePath);
                var fileEntry = new FileEntry(id, fileName, filePath, url, fileInfo.Length, fileSize, lastAccess);
                await output.UpdateEntry(fileEntry);
            } catch (Exception e) {
                Log.Error(e, "Walk through {file_path} failed", filePath);
            }
        }
        foreach (string directory in Directory.GetDirectories(dir)) {
            await Walk(directory, baseDir, output, baseUrl);
        }
    }

    private async Task<int> ExecuteAsync(CommandContext context, Settings settings) {
        if (settings.BaseUrl != null && !settings.BaseUrl.EndsWith('/')) {
            settings.BaseUrl += "/";
        }
        await using var output = !string.IsNullOrWhiteSpace(settings.MeilisearchUrl)
            ? (IFileEntryOutput)new MeilisearchOutput(
                settings.MeilisearchUrl, settings.MasterKey,
                settings.Index, settings.Batch)
            : new ConsoleOutput();
        await Walk(settings.BaseDir, settings.BaseDir, output, settings.BaseUrl);
        return 0;
    }

    public override int Execute(CommandContext context, Settings settings) {
        return ExecuteAsync(context, settings).Result;
    }
}
